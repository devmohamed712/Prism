using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prism.BL.Managers.Order.OrderReports;
using Prism.BL.Managers.Order.OrderSamples;
using QRCodeResults.BL.Enums;

namespace Prism.API.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.LabAssistant)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderReportsController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IOrderReportsManager _orderReports;
        public readonly IOrderSamplesManager _orderSamplesManager;
        public OrderReportsController(IMapper mapper, IConfiguration configuration, IOrderReportsManager orderReports, IOrderSamplesManager orderSamplesManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _orderReports = orderReports;
            _orderSamplesManager = orderSamplesManager;
        }

        [HttpGet("GetStatistics")]
        public IActionResult GetStatistics()
        {
            return Ok(new
            {
                SamplesCollected = _orderSamplesManager.GetSamplesWithStatus((int)StatisticsTypes.SamplesCollected, 1),
                SamplesHasBeenTested = _orderSamplesManager.GetSamplesWithStatus((int)StatisticsTypes.SamplesHasBeenTested, 1),
                CustomersServed = _orderReports.GetCustomersServed(1),
                AverageCompleteTesting = _orderReports.GetAverageCompleteTesting(),
                SamplesHasBeenReceived = _orderSamplesManager.GetSamplesWithStatus((int)StatisticsTypes.SamplesHasBeenReceived),
                SamplesInProcess = _orderSamplesManager.GetSamplesWithStatus((int)StatisticsTypes.SamplesInProcess),
                SamplesHasBeenFinished = _orderSamplesManager.GetSamplesWithStatus((int)StatisticsTypes.SamplesHasBeenFinished)
            });
        }

        [HttpGet("GetFilteredStatistics/{type}/{duration}")]
        public IActionResult GetFilteredStatistics(int type, int duration)
        {
            if (type == (int)StatisticsTypes.SamplesCollected)
            {
                return Ok(_orderSamplesManager.GetSamplesWithStatus(type, duration));
            }
            else if (type == (int)StatisticsTypes.SamplesHasBeenTested)
            {
                return Ok(_orderSamplesManager.GetSamplesWithStatus(type, duration));
            }
            else if (type == (int)StatisticsTypes.CustomersServed)
            {
                return Ok(_orderReports.GetCustomersServed(duration));
            }
            else
            {
                return Ok();
            }
        }
    }
}
