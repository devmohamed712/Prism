using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prism.BL.Dtos;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Order;
using Prism.BL.Managers.Order.OrderSamples;
using Prism.BL.Managers.Order.OrderSamplesTests;
using QRCodeResults.BL.Enums;
using System.Security.Claims;

namespace Prism.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSamplesController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IOrderManager _orderManager;
        public readonly IOrderSamplesManager _orderSamplesManager;
        public readonly ICommonManager _commonManager;
        
        public OrderSamplesController(IMapper mapper, IConfiguration configuration, IOrderManager orderManager, IOrderSamplesManager orderSamplesManager, ICommonManager commonManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _orderManager = orderManager;
            _orderSamplesManager = orderSamplesManager;
            _commonManager = commonManager;
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
        [HttpGet("GetOrderSamplesBySamplerId")]
        public IActionResult GetOrderSamplesBySamplerId()
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                return Ok(_orderSamplesManager.GetOrderSamples(0, userId));
            }
            return BadRequest();
        }

        [Authorize(Roles = Roles.Sampler)]
        [HttpGet("GetFormData/{orderId}")]
        public IActionResult GetFormData(int? orderId = null)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            OrderDto order = new OrderDto();
            if (orderId != null)
            {
                order = _orderManager.GetOrder(orderId.Value, userRole, userId);
            }
            else
            {
                order = _orderManager.GetOrderBySamplerId(userId);
            }
            if (order.Id > 0)
            {
                return Ok(new
                {
                    Order = order,
                    SampleTypes = _commonManager.GetSampleTypesList(),
                    TestTypes = _commonManager.GetTestTypesList(),
                    TestSubTypes = _commonManager.GetTestSubTypesList()
                });
            }
            return BadRequest();
        }

        [Authorize(Roles = Roles.Sampler)]
        [HttpPost("CreateOrEditOrderSample")]
        public IActionResult CreateOrEditOrderSample()
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var form = Request.Form;
            var model = JsonConvert.DeserializeObject<OrderSamplesDto>(form.ToList().Where(x => x.Key == "model").FirstOrDefault().Value);
            string? userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole.Equals(Roles.LabTechnician))
            {
                model.LabTechId = userId;
            }
            if (ModelState.IsValid)
            {
                string OrderSampleImagesPath = _configuration.GetSection("UploadedFiles")?.GetSection("OrderSamplesImages")?.Value;
                return Ok(_orderSamplesManager.CreateOrEditOrderSample(model, form.Files, OrderSampleImagesPath));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetOrderSamples/{pageNumber}/{pageSize}/{orderId}")]
        public IActionResult GetOrderSamples(int pageNumber, int pageSize, int orderId)
        {
            return Ok(_orderSamplesManager.GetOrderSamples(pageNumber, pageSize, orderId));
        }

        [Authorize(Roles = Roles.Sampler)]
        [HttpGet("AcceptOrderSamples/{orderId}")]
        public IActionResult AcceptOrderSamples(int orderId)
        {
            return Ok(_orderSamplesManager.AcceptOrderSamples(orderId));
        }

        [Authorize(Roles = Roles.LabTechnician)]
        [HttpGet("SplitSample/{id}/{isSplit}")]
        public IActionResult SplitSample(int id, bool isSplit)
        {
            string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(_orderSamplesManager.SplitSample(id, isSplit, userRole, userId));
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
        [HttpGet("SamplesSearch/{pageNumber}/{pageSize}/{orderName}/{testId}/{sampleId}/{pendingSplit}/{pendingForeignMatterTesting}/{pendingWaterActivity}/{totalYeastAndMoldCount}/{totalColiform}/{eColi}/{salmonella}/{aspergillus}/{pendingPesticidesTesting}/{pendingMetalTesting}/{pendingPotencyTesting}/{pendingTerpensTesting}")]
        public IActionResult SamplesSearch(int pageNumber, int pageSize, string orderName = null, int testId = 0, int sampleId = 0, bool pendingSplit = false, bool pendingForeignMatterTesting = false, bool pendingWaterActivity = false, bool totalYeastAndMoldCount = false, bool totalColiform = false, bool eColi = false, bool salmonella = false, bool aspergillus = false, bool pendingPesticidesTesting = false, bool pendingMetalTesting = false, bool pendingPotencyTesting = false, bool pendingTerpensTesting = false)
        {
            return Ok(_orderSamplesManager.SamplesSearch(pageNumber, pageSize, orderName, testId, sampleId, pendingSplit, pendingForeignMatterTesting, pendingWaterActivity, totalYeastAndMoldCount, totalColiform, eColi, salmonella, aspergillus, pendingPesticidesTesting, pendingMetalTesting, pendingPotencyTesting, pendingTerpensTesting));
        }
    }
}
