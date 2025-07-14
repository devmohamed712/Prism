using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.BL.Helpers.Configrations
{
    public interface IConfigrationsManager
    {
        public void UpdateDatabase(WebApplication app);

        public Task CreateRolesAndAdmin(IServiceProvider serviceProvider, IMapper mapper);

        public void AddAutoMapper(IServiceCollection services);

        public void AuthenticationBuilder(IServiceCollection services);

        public void PasswordConfiguration(IServiceCollection services);

        public void AddHangfire(IServiceCollection services);
    }
}
