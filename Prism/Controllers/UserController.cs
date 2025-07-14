using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Prism.BL.Configrations.FirebaseAuthentication;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Notification;
using Prism.BL.Managers.Order;
using Prism.BL.Managers.Order.OrderSamples;
using Prism.BL.Managers.User;
using QRCodeResults.BL.Enums;


namespace Prism.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IServiceProvider _serviceProvider;
        public readonly UserManager<IdentityUser> userManager;
        public readonly IUserManager _userManager;
        public readonly INotificationManager _notificationManager;
        public readonly ICommonManager _commonManager;
        public readonly IBusinessManager _businessManager;
        public readonly IOrderManager _orderManager;
        public readonly IOrderSamplesManager _orderSamplesManager;

        public UserController(IMapper mapper, IConfiguration configuration, IServiceProvider serviceProvider, IUserManager userManager, INotificationManager notificationManager, ICommonManager commonManager, IBusinessManager businessManager, IOrderManager orderManager, IOrderSamplesManager orderSamplesManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            this.userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _userManager = userManager;
            _notificationManager = notificationManager;
            _commonManager = commonManager;
            _businessManager = businessManager;
            _orderManager = orderManager;
            _orderSamplesManager = orderSamplesManager;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(model.Email);
                if (existingUser == null)
                {
                    return Ok(await _userManager.Register(model));
                }
                return BadRequest("User already exists. Please try with a nother one");
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var loginModel = await _userManager.Login(model);
                if (loginModel != null)
                {
                    return Ok(loginModel);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpGet("ForgetPassword/{email}")]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            var result = await _userManager.ForgetPassword(email);
            if (result == true)
            {
                return Ok(true);
            }
            return BadRequest("An error has been occured or this link has been expired!");
        }

        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {

                var result = await _userManager.ResetPassword(model);
                if (result.Succeeded)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("LoginByPhoneNumber")]
        public async Task<IActionResult> LoginByPhoneNumber([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var firebaseUser = await new PhoneNumberAuthentication().PhoneNumberAuth(model.FirebaseUserId);
            if (firebaseUser != null && firebaseUser.PhoneNumber == model.PhoneNumber)
            {
                var user = await userManager.FindByNameAsync(firebaseUser.PhoneNumber);
                if (user == null)
                {
                    user = await _userManager.RegisterByPhoneNumber(firebaseUser.PhoneNumber);
                }
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    var login = await userManager.GetLoginsAsync(user);
                    if (login == null || login.Count == 0)
                    {
                        var loginResult = await userManager.AddLoginAsync(user, new UserLoginInfo(firebaseUser.ProviderId, model.FirebaseUserId, ""));
                        if (!loginResult.Succeeded)
                        {
                            return BadRequest(loginResult);
                        }
                    }
                    var account = _userManager.GetUser(user.Id);
                    bool hasAccount = false;
                    if (account == null)
                    {
                        account = new AccountDto()
                        {
                            Id = user.Id,
                            PhoneNumber = user.PhoneNumber
                        };
                    }
                    else
                    {
                        hasAccount = true;
                        if (model.MobileNotificationToken != null)
                        {
                            model.MobileNotificationToken.UserId = account.Id;
                            _notificationManager.CreateEditMobileNotificationTokens(model.MobileNotificationToken);
                        }
                    }
                    var token = GenerateJSONWebToken(user, roles[0]);
                    account.Role = roles[0];
                    return Ok(new { Account = account, Token = token, HasAccount = hasAccount });
                }
                return BadRequest("can not create user ");
            }
            return BadRequest("invaled firebase user ");
        }

        [AllowAnonymous]
        [HttpGet("GetAccountByPhoneNumber")]
        public IActionResult GetAccountByPhoneNumber([FromQuery] string phoneNumber)
        {
            var account = _userManager.GetUserByPhoneNumber(phoneNumber);
            if (account != null)
            {
                if (!account.IsActive.Value)
                {
                    return Conflict();
                }
                return Ok(account);
            }
            return Ok();
        }

        private string GenerateJSONWebToken(IdentityUser userInfo, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub,  userInfo.Id),
                 new Claim(ClaimTypes.Role, role),
                 new Claim("DateOfJoing", DateTime.UtcNow.ToString("yyyy-MM-dd")),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddYears(1),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpGet("GetUser")]
        public ActionResult GetUser()
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var account = _userManager.GetUser(userId);
            if (account != null)
            {
                return Ok(account);
            }
            return BadRequest("invaled user Id");
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUser/{userId}")]
        public ActionResult GetUser(string userId)
        {
            var account = _userManager.GetUser(userId);
            if (account != null)
            {
                return Ok(account);
            }
            return BadRequest("invaled user Id");
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetUsers/{pageNumber}/{pageSize}/{role}/{hasOrders}")]
        public IActionResult GetUsers(int pageNumber, int pageSize, string? role = null, bool hasOrders = false)
        {
            return Ok(_userManager.GetUsers(pageNumber, pageSize, role, hasOrders));
        }

        [Authorize]
        [HttpPost("CreateOrEditUser")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CreateOrEditUser([FromForm] AccountDto model)
        {
            var form = Request.Form;
            if (ModelState.IsValid)
            {
                IdentityUser? user = !string.IsNullOrEmpty(model.Id) ? await userManager.FindByIdAsync(model.Id) : null;
                AccountDto? account = null;
                if (user == null)
                {
                    user = await _userManager.RegisterByPhoneNumber(model.PhoneNumber, model.Role);
                    model.Id = user.Id;
                }
                else
                {
                    user.UserName = model.PhoneNumber;
                    user.PhoneNumber = model.PhoneNumber;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        await userManager.RemoveFromRolesAsync(user, roles);
                        await userManager.AddToRoleAsync(user, model.Role);
                    }
                }
                account = _userManager.CreateOrEdit(model);
                if (account != null)
                {
                    if (form.Files.Count() > 0)
                    {
                        string SamplerDocsPath = _configuration.GetSection("UploadedFiles")?.GetSection("SamplerDocs")?.Value;
                        model.SamplerDocuments = model.SamplerDocuments == null ? new List<SamplerDocumentsDto>() : model.SamplerDocuments;
                        model.SamplerDocuments = _orderSamplesManager.CreateOrEditSamplerDocFiles(form.Files, model, SamplerDocsPath);
                    }
                    if (model.SamplerDocuments != null)
                    {
                        model.SamplerDocuments.ForEach(x =>
                        {
                            _orderSamplesManager.CreateOrEditSamplerDocument(x);
                        });
                    }
                    return Ok(account);
                }
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("ChangeUserActivation/{id}/{status}")]
        public async Task<IActionResult> ChangeUserActivation(string id, bool status)
        {
            return Ok(_userManager.ChangeUserActivation(id, status));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            bool isAccountDeleted = _userManager.DeleteUser(userId);
            if (isAccountDeleted)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.DeleteAsync(user);
                    return Ok(true);
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("RegisterByAdmin/{phoneNumber}/{role}")]
        public async Task<IActionResult> RegisterByAdmin(string phoneNumber, string role)
        {
            var userDB = await userManager.FindByNameAsync(phoneNumber);
            if (userDB == null)
            {
                var user = new IdentityUser() { UserName = phoneNumber, PhoneNumber = phoneNumber };
                IdentityResult result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    return Ok(user);
                }
            }
            else
            {
                userDB.UserName = phoneNumber;
                userDB.PhoneNumber = phoneNumber;
                IdentityResult result = await userManager.UpdateAsync(userDB);
                if (result.Succeeded)
                {
                    var existRoles = await userManager.GetRolesAsync(userDB);
                    if (!existRoles.Any(x => x.Equals(role)))
                    {
                        await userManager.RemoveFromRolesAsync(userDB, existRoles);
                        await userManager.AddToRoleAsync(userDB, role);
                    }
                    return Ok(userDB);
                }
            }
            return null;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            return Ok(_userManager.GetRoles());
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetSamplers/{pageNumber}/{pageSize}")]
        public IActionResult GetSamplers(int pageNumber, int pageSize)
        {
            return Ok(_userManager.GetSamplers(pageNumber, pageSize));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAdminDashboardData")]
        public IActionResult GetAdminDashboardData()
        {
            return Ok(new
            {
                Businesses = _businessManager.GetBusinesses(1, 3).Businesses,
                Samplers = _userManager.GetUsers(1, 3, Roles.Sampler.ToString(), true).Users,
                LabTechnicians = _userManager.GetUsers(1, 3, Roles.LabTechnician.ToString()).Users,
                Orders = _orderManager.GetOrders(1, 5).Orders,
                Users = _userManager.GetUsers(1, 5).Users
            });
        }

        [Authorize(Roles = Roles.Sampler)]
        [HttpGet("GetSamplerDashboardData")]
        public IActionResult GetSamplerDashboardData()
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            List<OrderStatusDto> orderStatus = _commonManager.GetOrderStatusList();
            return Ok(new
            {
                OrderAwaitingAcceptance = _orderManager.GetOrders(1, 5, new List<int>() { orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.Placed)).Id }).Orders,
                OrdersToBePickedUp = _orderManager.GetOrders(1, 5, new List<int>() { orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.Accepted)).Id }, userId, userRole).Orders,
                OrdersToBeDroppedOff = _orderManager.GetOrders(1, 5, new List<int>() { orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.PickedUp)).Id }, userId, userRole).Orders,
                OrdersDroppedOff = _orderManager.GetOrders(1, 5, new List<int>() { orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.DroppedOff)).Id }, userId, userRole).Orders,
                OrderStatus = _commonManager.GetOrderStatusList(),
                Order = _orderManager.GetOrderBySamplerId(userId)
            });
        }

        [Authorize(Roles = Roles.LabTechnician)]
        [HttpGet("GetLabTechDashboardData")]
        public IActionResult GetLabTechDashboardData()
        {
            List<OrderStatusDto> orderStatus = _commonManager.GetOrderStatusList();
            List<int> i = new List<int>(3);
            return Ok(new
            {
                OrdersDroppedOff = _orderManager.GetOrders(1, 5, new List<int>() { orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.DroppedOff)).Id }).Orders,
                OrdersInProgress = _orderManager.GetOrders(1, 5, new List<int>() {
                    orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.SampleAccepted)).Id,
                    orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.TestingStarted)).Id}).Orders,
                OrdersCompleted = _orderManager.GetOrders(1, 5, new List<int>() { orderStatus.FirstOrDefault(x => x.Name.Equals(OrderStatus.TestingCompleted)).Id }).Orders,
                OrderStatus = _commonManager.GetOrderStatusList()
            });
        }
    }
}
