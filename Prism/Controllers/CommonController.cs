using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Prism.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("GetLatestBuildInfo")]
        public IActionResult GetLatestBuildInfo()
        {
            var serviceAccountPath = Path.Combine(Directory.GetCurrentDirectory() + "\\CredentialFiles\\", $"{"build-info.json"}");
            var serviceAccountJson = System.IO.File.ReadAllText(serviceAccountPath);
            return Ok(serviceAccountJson);
        }
    }
}
