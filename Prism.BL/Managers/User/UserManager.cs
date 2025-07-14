using AutoMapper;
using iTextSharp.text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Prism.BL.Configrations;
using Prism.BL.Dtos;
using Prism.BL.Helpers;
using Prism.BL.Managers.Notification;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.User
{
    public class UserManager : IUserManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IServiceProvider _serviceProvider;
        public readonly UserManager<IdentityUser> userManager;
        public readonly IUnitOfWork _unitOfWork;
        public readonly INotificationManager _notificationManager;

        public UserManager(IMapper mapper, IConfiguration configuration, IServiceProvider serviceProvider, IUnitOfWork unitOfWork, INotificationManager notificationManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _unitOfWork = unitOfWork;
            _notificationManager = notificationManager;
        }

        public async Task<RegisterDto?> Register(RegisterDto model, bool isAddingByAdmin = false)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser == null)
            {
                var user = new IdentityUser { Email = model.Email, UserName = model.Phone, PhoneNumber = model.Phone };
                string password = model.Password;
                var chkUser = await userManager.CreateAsync(user, password);
                if (chkUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
                    model.Token = await GenerateSecuritytoken(user);
                    var roles = await userManager.GetRolesAsync(user);
                    model.Role = roles[0];

                    TblAccounts tblCustomers = new TblAccounts();
                    tblCustomers.Id = user.Id;
                    tblCustomers.IsDeleted = false;
                    tblCustomers.Name = model.FullName;
                    tblCustomers.CreateAt = DateTime.UtcNow;
                    tblCustomers.UpdateAt = DateTime.UtcNow;
                    tblCustomers.IsActive = true;
                    _unitOfWork.Accounts.Add(tblCustomers);
                    if (!isAddingByAdmin)
                    {
                        TblCustomerResults tblCustomerResults = new TblCustomerResults();
                        tblCustomerResults.UserId = user.Id;
                        tblCustomerResults.Time = DateTime.UtcNow;
                        tblCustomerResults.FullName = model.FullName;
                        tblCustomerResults.Phone = model.Phone;
                        tblCustomerResults.CreateAt = DateTime.UtcNow;
                        tblCustomerResults.UpdateAt = DateTime.UtcNow;
                        _unitOfWork.CustomerResults.Add(tblCustomerResults);
                    }
                    _unitOfWork.Complete();

                    model.UserId = user.Id;
                    //if (isAddingByAdmin && !string.IsNullOrEmpty(password))
                    //{
                    //    new PCRResultsManager(_mapper, _configuration).SendPasswordToUser(user.Email, password);
                    //}
                    return model;
                }
            }
            else
            {
                model.UserId = existingUser.Id;
                return model;
            }
            return null;
        }

        public async Task<IdentityUser?> RegisterByPhoneNumber(string phoneNumber, string roleId = "User")
        {
            var user = new IdentityUser() { UserName = phoneNumber, PhoneNumber = phoneNumber };
            IdentityResult result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleId);
                return user;
            }
            return null;
        }

        public async Task<LoginDto?> Login(LoginDto model)
        {
            var existedUser = await userManager.FindByEmailAsync(model.Email);
            if (existedUser != null && await userManager.CheckPasswordAsync(existedUser, model.Password))
            {
                var roles = await userManager.GetRolesAsync(existedUser);
                model.Role = roles[0];
                model.Token = await GenerateSecuritytoken(existedUser);
                return model;
            }
            return null;
        }

        private async Task<string> GenerateSecuritytoken(IdentityUser user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userRole));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public async Task<bool> ForgetPassword(string email)
        {
            var user = email != null ? await userManager.FindByEmailAsync(email) : null;
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
                var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                var passwordResetLink = _configuration["PasswordResetLink"] + "?Email=" + email + "&Token=" + codeEncoded.ToString();
                SendForgetPasswordEmail(user, passwordResetLink);
                return true;
            }
            return false;
        }

        private bool SendForgetPasswordEmail(IdentityUser user, string passwordLink)
        {
            bool isSended;
            var stringBuilder = new StringBuilder("<body style='margin: 0px;'>");
            stringBuilder.AppendFormat("<div><a href=" + passwordLink + ">Reset password</a></div>");
            stringBuilder.Append("</body>");
            var content = stringBuilder.ToString();
            var model = new UserContactDto
            {
                Email = _configuration["Smtp:SenderMail"],
                Subject = "Reset Password",
                Message = content
            };
            List<string> emails = new List<string>();
            emails.Add(user.ToString());
            isSended = new Mail(_configuration).SendEmail(model, emails);
            return isSended;
        }

        public async Task<IdentityResult?> ResetPassword(ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var codeDecodedBytes = WebEncoders.Base64UrlDecode(model.Token);
                var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
                var result = await userManager.ResetPasswordAsync(user, codeDecoded, model.Password);
                return result;
            }
            return null;
        }

        public List<RoleDto> GetRoles()
        {
            List<RoleDto> roles = new List<RoleDto>();
            var rolesDb = _unitOfWork.AspNetRoles.GetAll();
            if (rolesDb != null)
            {
                roles = _mapper.Map<List<RoleDto>>(rolesDb);
            }
            return roles;
        }

        public AccountDtoList GetUsers(int pageNumber, int pageSize, string? role = null, bool hasOrders = false)
        {
            AccountDtoList modelList = new AccountDtoList();
            modelList.Users = new List<AccountDto>();
            Func<AspNetUsers, bool> predicates = x => x.TblAccount == null || (x.TblAccount != null && !x.TblAccount.IsDeleted);
            if (!string.IsNullOrEmpty(role))
            {
                predicates = predicates.AndAlso(x => x.AspNetUserRoles.Any(c => c.Role.Name == role));
                if (hasOrders)
                {
                    predicates = predicates.AndAlso(x => x.TblAccount.Orders.Any(c => !c.IsDeleted && !c.IsCanceled &&
                    (c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted) || c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp))));
                }
            }
            var usersDB = _unitOfWork.AspNetUsers.FindByPage(predicates, pageNumber, pageSize);
            if (usersDB != null)
            {
                foreach (var user in usersDB)
                {
                    modelList.Users.Add(Mapping(user));
                }
            }
            modelList.PageNumber = pageNumber;
            modelList.IsLastPage = usersDB != null && usersDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public AccountDtoList GetSamplers(int pageNumber, int pageSize)
        {
            AccountDtoList modelList = new AccountDtoList();
            modelList.Users = new List<AccountDto>();
            var usersDB = _unitOfWork.Accounts.FindByPage(x => !x.IsDeleted && x.IsActive && x.AspNetUser.AspNetUserRoles.Any(c => c.Role.Name.Equals(Roles.Sampler) &&
            (x.Orders.Any(c => c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted)) || x.Orders.Any(c => c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp)))), pageNumber, pageSize);
            if (usersDB != null)
            {
                foreach (var user in usersDB)
                {
                    modelList.Users.Add(Mapping(user));
                }
            }
            modelList.PageNumber = pageNumber;
            modelList.IsLastPage = usersDB != null && usersDB.Count() < pageSize ? true : false;
            return modelList;
        }

        public AccountDto? GetUser(string id)
        {
            AccountDto? model = null;
            var user = _unitOfWork.AspNetUsers.FirstOrDefault(x => x.Id == id && (x.TblAccount == null || (x.TblAccount != null && !x.TblAccount.IsDeleted)));
            if (user != null)
            {
                model = Mapping(user);
            }
            return model;
        }

        public AccountDto? GetUserByEmail(string email)
        {
            AccountDto? model = null;
            var user = _unitOfWork.Accounts.FirstOrDefault(x => x.AspNetUser.Email == email);
            if (user != null)
            {
                model = Mapping(user);
            }
            return model;
        }

        public AccountDto? GetUserByPhoneNumber(string phoneNumber)
        {
            AccountDto? model = null;
            var user = _unitOfWork.Accounts.FirstOrDefault(x => string.Equals(x.AspNetUser.PhoneNumber.Replace("+", "").Trim(), phoneNumber.Replace("+", "").Trim()) && !x.IsDeleted);
            if (user != null)
            {
                model = Mapping(user);
                model.PhoneNumber = user.AspNetUser.PhoneNumber;
            }
            return model;
        }

        public AccountDto CreateOrEdit(AccountDto model)
        {
            var account = _unitOfWork.Accounts.FirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (account != null)
            {
                model.RegistrationDateTime = account.CreateAt;
                _mapper.Map(model, account);
                _unitOfWork.Complete();
            }
            else
            {
                model.RegistrationDateTime = DateTime.UtcNow;
                account = _mapper.Map<TblAccounts>(model);
                _unitOfWork.Accounts.Add(account);
                _unitOfWork.Complete();
                if (model.MobileNotificationToken != null)
                {
                    model.MobileNotificationToken.UserId = account.Id;
                    _notificationManager.CreateEditMobileNotificationTokens(model.MobileNotificationToken);
                }
            }
            model.Id = account.Id;
            return model;
        }

        public bool ChangeUserActivation(string id, bool status)
        {
            var accountDB = _unitOfWork.Accounts.FirstOrDefault(c => c.Id == id);
            if (accountDB != null)
            {
                accountDB.IsActive = status;
                _unitOfWork.Complete();
                return true;
            }
            return false;
        }

        public bool DeleteUser(string id)
        {
            var account = _unitOfWork.Accounts.FirstOrDefault(x => x.Id == id);
            if (account != null)
            {
                _unitOfWork.Accounts.Remove(account);
                _unitOfWork.Complete();
                return true;
            }
            return false;
        }

        public SamplerTracksDto AddSamplerTrack(SamplerTracksDto model, string samplerId)
        {
            var order = _unitOfWork.Order.FirstOrDefault(x => !x.IsDeleted && !x.IsCanceled && x.SamplerId == samplerId &&
            (x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted) || x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp)));
            if (order != null)
            {
                model.OrderId = order.Id;
                model.StatusId = order.OrderDetails.LastOrDefault().StatusId;
                model.DateTime = DateTime.UtcNow;
                TblSamplerTracks trackDB = _mapper.Map<TblSamplerTracks>(model);
                _unitOfWork.SamplerTracks.Add(trackDB);
                _unitOfWork.Complete();
                model.Id = trackDB.Id;
            }
            return model;
        }

        public SamplerTracksDtoList GetLastSamplerTrack(string samplerId)
        {
            SamplerTracksDtoList modeLst = new SamplerTracksDtoList();
            modeLst.SamplerTrack = new SamplerTracksDto();
            Func<TblSamplerTracks, bool> predicates = x => !x.IsDeleted && !x.Order.IsDeleted && !x.Order.IsCanceled && x.Order.SamplerId.Equals(samplerId) && (x.Order.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted) || x.Order.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp));
            var tracks = _unitOfWork.SamplerTracks.FindList(predicates);
            if (tracks != null && tracks.Count() > 0)
            {
                modeLst.SamplerTrack = _mapper.Map<SamplerTracksDto>(tracks.Last());
                var order = _unitOfWork.Order.FirstOrDefault(x => !x.IsDeleted && !x.IsCanceled && x.SamplerId.Equals(samplerId) &&
                (x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.Accepted) || x.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp)));
                modeLst.Order = _mapper.Map<OrderDto>(order);
                modeLst.Order.StatusName = order.OrderDetails.LastOrDefault().OrderStatus.Name;
                var businessAddress = order.Business.BusinessAddresses.FirstOrDefault(x => !x.IsDeleted && x.IsMain);
                modeLst.MainAddress = _mapper.Map<BusinessAddressesDto>(businessAddress);
                modeLst.OrderAcceptLatitudePoint = tracks.First().Latitude;
                modeLst.OrderAcceptLongitudePoint = tracks.First().Longitude;
            }
            return modeLst;
        }

        public AccountDto Mapping(TblAccounts account)
        {
            AccountDto model = new AccountDto();
            model = _mapper.Map<AccountDto>(account);
            model.PhoneNumber = account.AspNetUser.PhoneNumber;
            model.RoleId = account.AspNetUser.AspNetUserRoles.FirstOrDefault().RoleId;
            model.Role = account.AspNetUser.AspNetUserRoles.FirstOrDefault().Role?.Name;
            if (account.AspNetUser.AspNetUserRoles.Any(x => x.Role.Name == Roles.Sampler.ToString()))
            {
                model.SamplerDocuments = new List<SamplerDocumentsDto>();
                var userSamplerDocumentsDB = account.SamplerDocuments.Where(c => !c.IsDeleted).ToList();
                userSamplerDocumentsDB.ForEach(c =>
                {
                    SamplerDocumentsDto docModel = new SamplerDocumentsDto();
                    docModel = _mapper.Map<SamplerDocumentsDto>(c);
                    docModel.SamplerDocumentType = c.SamplerDocumentType.Name;
                    model.SamplerDocuments.Add(docModel);
                });
            }
            return model;
        }

        public AccountDto Mapping(AspNetUsers user)
        {
            AccountDto model = new AccountDto();
            model.Id = user.Id;
            model.PhoneNumber = user.PhoneNumber;
            model.Email = user.Email;
            model.Name = user.PhoneNumber != null ? user.TblAccount?.Name : null;
            model.Role = user.AspNetUserRoles?.FirstOrDefault()?.Role?.Name;
            model.RoleId = user.AspNetUserRoles?.FirstOrDefault()?.RoleId;
            model.IsActive = user.TblAccount?.IsActive;
            model.RegistrationDateTime = user.TblAccount?.CreateAt;
            if (user.TblAccount != null && user.TblAccount.AspNetUser.AspNetUserRoles.Any(x => x.Role.Name == Roles.Sampler.ToString()))
            {
                model.SamplerOrdersPickedUp = user.TblAccount.Orders != null ? user.TblAccount.Orders.Count(c => !c.IsDeleted && c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.PickedUp)) : 0;
                model.SamplerOrdersDroppedOff = user.TblAccount.Orders != null ? user.TblAccount.Orders.Count(c => !c.IsDeleted && c.OrderDetails.LastOrDefault().OrderStatus.Name.Equals(OrderStatus.DroppedOff)) : 0;
                model.SamplerDocuments = new List<SamplerDocumentsDto>();
                var userSamplerDocumentsDB = user.TblAccount.SamplerDocuments.Where(c => !c.IsDeleted).ToList();
                userSamplerDocumentsDB.ForEach(c =>
                {
                    SamplerDocumentsDto docModel = new SamplerDocumentsDto();
                    docModel = _mapper.Map<SamplerDocumentsDto>(c);
                    docModel.SamplerDocumentType = c.SamplerDocumentType.Name;
                    model.SamplerDocuments.Add(docModel);
                });
            }
            return model;
        }
    }
}
