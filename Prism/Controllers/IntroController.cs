using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.IO;
using System;
using System.Security.Claims;

namespace Prism.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class IntroController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Index");
        }
    }
}
