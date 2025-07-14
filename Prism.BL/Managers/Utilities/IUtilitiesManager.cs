using Microsoft.AspNetCore.Http;
using MimeKit;
using Prism.BL.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Managers.Utilities
{
    public interface IUtilitiesManager
    {
        public string UploadImage(string imgBase64, string imgExtension, string subFolderName);

        public string UploadFile(IFormFile file, string fileExtension, string subFolderName, string entityId = "");

        public string RandomString(int size, bool lowerCase);

        public int RandomNumber(int min, int max);

        public string RandomPassword(int size = 0);

        public object GetPropertyValue<T>(T obj, string propName);

        public PropertyInfo GetProperty<T>(T obj, string propName);

        public string ToTitleCase(string word);

        public object GetProperty(object target, string name);
    }
}
