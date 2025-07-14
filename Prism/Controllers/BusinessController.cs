using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prism.BL.Dtos;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Common;
using QRCodeResults.BL.Enums;

namespace Prism.API.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IBusinessManager _businessManager;
        public readonly ICommonManager _commonManager;

        public BusinessController(IMapper mapper, IConfiguration configuration, IBusinessManager businessManager, ICommonManager commonManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _businessManager = businessManager;
            _commonManager = commonManager;
        }

        [HttpGet("GetBusinesses/{pageNumber}/{pageSize}")]
        public IActionResult GetBusinesses(int pageNumber, int pageSize)
        {
            return Ok(_businessManager.GetBusinesses(pageNumber, pageSize));
        }

        [HttpGet("GetBusiness/{id}")]
        public IActionResult GetBusiness(int id)
        {
            return Ok(_businessManager.GetBusiness(id));
        }

        [HttpGet("GetBusinessesWithOutPagination")]
        public IActionResult GetBusinessesWithOutPagination()
        {
            return Ok(_businessManager.GetBusinesses());
        }

        [HttpPost("CreateOrEdit")]
        public IActionResult CreateOrEdit(BusinessDto model)
        {
            if (ModelState.IsValid)
            {
                return Ok(_businessManager.CreateOrEditBusiness(model));
            }
            return BadRequest(ModelState);
        }

        [HttpGet("DeleteOrRestoreBusiness/{id}/{isDelete}")]
        public IActionResult DeleteOrRestoreBusiness(int id, bool isDelete)
        {
            return Ok(_businessManager.DeleteOrRestoreBusiness(id, isDelete));
        }

        [HttpGet("ChangeMRlicenseStatus/{id}/{status}")]
        public IActionResult ChangeMRlicenseStatus(int id, bool status)
        {
            return Ok(_businessManager.ChangeMRlicenseStatus(id, status));
        }

        [HttpGet("GetBusinessesTestsAndSamples")]
        public IActionResult GetBusinessesTestsAndSamples()
        {
            return Ok(new
            {
                Businesses = _businessManager.GetBusinesses(),
                Tests = _commonManager.GetTestTypesList(),
                SubTests = _commonManager.GetTestSubTypesList()
            });
        }
    }
}
