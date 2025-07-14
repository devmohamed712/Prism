using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prism.BL.Dtos;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Order.OrderSamplesTests;
using QRCodeResults.BL.Enums;
using System.Security.Claims;

namespace Prism.API.Controllers
{
    [Authorize(Roles = Roles.LabTechnician)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSamplesTestsController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IOrderSamplesTestsManager _orderSamplesTests;
        public readonly ICommonManager _commonManager;

        public OrderSamplesTestsController(IMapper mapper, IConfiguration configuration, IOrderSamplesTestsManager orderSamplesTests, ICommonManager commonManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _orderSamplesTests = orderSamplesTests;
            _commonManager = commonManager;
        }

        [HttpGet("GetSampleTests/{sampleId}")]
        public IActionResult GetSampleTests(int sampleId)
        {
            return Ok(new
            {
                SampleTestStatus = _commonManager.GetSampleTestStatusList(),
                GroupTests = _orderSamplesTests.GetGroupTestsList(sampleId)
            });
        }

        [HttpPost("AddTest")]
        public IActionResult AddTest(OrderSampleTestsDto model)
        {
            if (ModelState.IsValid)
            {
                model.LabTechId = GetCurrentUserId();
                return Ok(_orderSamplesTests.AddTest(model));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetGroupTestsList")]
        public IActionResult GetGroupTestsList()
        {
            return Ok(_orderSamplesTests.GetGroupTestsList());
        }

        private string GetCurrentUserId()
        {
            return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
