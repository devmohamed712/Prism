using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Order;
using Prism.BL.Managers.Order.OrderSamples;
using QRCodeResults.BL.Enums;
using System.Security.Claims;

namespace Prism.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IOrderManager _orderManager;

        public OrderController(IMapper mapper, IConfiguration configuration, IOrderManager orderManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _orderManager = orderManager;
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
        [HttpGet("GetOrders/{pageNumber}/{pageSize}")]
        public IActionResult GetOrders(int pageNumber, int pageSize)
        {
            string? userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            string? userId = userRole == Roles.Sampler ? HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
            var form = Request.Form;
            List<int> statusIds = JsonConvert.DeserializeObject<List<int>>(form.ToList().Where(x => x.Key == "statusIds").FirstOrDefault().Value);
            return Ok(_orderManager.GetOrders(pageNumber, pageSize, statusIds, userId, userRole));
        }

        [Authorize]
        [HttpGet("GetOrder/{id}")]
        public IActionResult GetOrder(int id)
        {
            string? userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_orderManager.GetOrder(id, userRole, userId));
        }

        [Authorize(Roles = Roles.Sampler)]
        [HttpGet("GetOrderBySamplerId/{isConfirmationPage}")]
        public IActionResult GetOrderBySamplerId(bool isConfirmationPage = false)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_orderManager.GetOrderBySamplerId(userId, isConfirmationPage));
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
        [HttpPost("CreateOrEdit")]
        public IActionResult CreateOrEdit(OrderDto model)
        {
            if (ModelState.IsValid)
            {
                return Ok(_orderManager.CreateOrEditOrder(model));
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
        [HttpGet("DeleteOrder")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orderManager.GetOrder(id);
            if (order.Id > 0 && !order.StatusName.Equals("Placed"))
            {
                return Conflict();
            }
            return Ok(_orderManager.Delete(id));
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
        [HttpGet("CancelOrder/{id}")]
        public IActionResult CancelOrder(int id)
        {
            var order = _orderManager.GetOrder(id);
            if (order.Id > 0 && !order.StatusName.Equals(OrderStatus.Placed) && !order.StatusName.Equals(OrderStatus.Accepted))
            {
                return Conflict();
            }
            return Ok(_orderManager.Cancel(id));
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabTechnician)]
        [HttpGet("CompleteOrder/{orderId}")]
        public IActionResult CompleteOrder(int orderId)
        {
            _orderManager.CompleteOrder(orderId, true);
            return Ok(true);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.QA)]
        [HttpGet("ApproveOrReportOrReleaseOrder/{orderId}/{status}")]
        public IActionResult ApproveOrReportOrReleaseOrder(int orderId, string status)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_orderManager.ApproveOrReportOrReleaseOrder(orderId, status, userId));
        }
    }
}
