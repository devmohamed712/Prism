using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Newtonsoft.Json;
using Prism.BL.Dtos;
using Prism.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Utilities
{
    public class UtilitiesManager : IUtilitiesManager
    {

        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;

        public UtilitiesManager(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public string UploadImage(string imgBase64, string imgExtension, string subFolderName)
        {
            var imagesTypes = new List<string>() { "jpg", "jpe", "png", "jpeg", "webp" };
            if (String.IsNullOrEmpty(imgExtension) || !imagesTypes.Contains(imgExtension.ToLower()))
            {
                return "Invalid image Extenstion";
            }
            var base64Array = Convert.FromBase64String(imgBase64);
            var folderName = Path.Combine(subFolderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string time = DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString() + DateTime.UtcNow.Hour.ToString() + DateTime.UtcNow.Minute.ToString() + DateTime.UtcNow.Second.ToString();
            var fileName = time + "_" + Guid.NewGuid().ToString();
            var filePath = Path.Combine(pathToSave, fileName + "." + imgExtension);
            File.WriteAllBytes(filePath, base64Array);
            return fileName + "." + imgExtension;
        }

        public string UploadFile(IFormFile file, string fileExtension, string subFolderName, string entityId = "")
        {
            var filesTypes = file.ContentType.StartsWith("application/") ? new List<string>() { "pdf", "txt", "docx", "xlsx", "pptx" } : new List<string>() { "jpg", "jpe", "png", "jpeg", "webp" };
            if (String.IsNullOrEmpty(fileExtension) || !filesTypes.Contains(fileExtension.ToLower()))
            {
                return "Invalid File Extenstion";
            }
            var folderName = Path.Combine(subFolderName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string time = DateTime.UtcNow.Year.ToString() + DateTime.UtcNow.Month.ToString() + DateTime.UtcNow.Day.ToString() + DateTime.UtcNow.Hour.ToString() + DateTime.UtcNow.Minute.ToString() + DateTime.UtcNow.Second.ToString();
            var fileName = !string.IsNullOrEmpty(entityId) ? entityId + "_" + time + "_" + Guid.NewGuid().ToString() : time + "_" + Guid.NewGuid().ToString();
            if (fileName.Length > 120)
            {
                fileName = fileName.Substring(0, 120);
            }
            var filePath = Path.Combine(pathToSave, fileName + "." + fileExtension);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName + "." + fileExtension;
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        public object GetPropertyValue<T>(T obj, string propName)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, propName, obj.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            var val = site.Target(site, obj);
            return val;
        }

        public PropertyInfo GetProperty<T>(T obj, string propName)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            return prop;
        }

        public string ToTitleCase(string word)
        {
            return (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower()));
        }

        public object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }
    }
}
