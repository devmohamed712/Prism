using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Prism.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QRCodeResults.BL.Enums;
using Prism.Repository;
using iTextSharp.text;
using Microsoft.IdentityModel.Tokens;
using Prism.BL.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Prism.BL.Managers.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Prism.BL.AutoMapper;
using Hangfire;
using Hangfire.SqlServer;


namespace Prism.BL.Helpers.Configrations
{
    public class ConfigrationsManager : IConfigrationsManager
    {
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IUserManager _userManager;

        public ConfigrationsManager(IMapper mapper, IConfiguration configuration, IUserManager userManager)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
        }

        public void UpdateDatabase(WebApplication app)
        {
            using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<PrismContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public async Task CreateRolesAndAdmin(IServiceProvider serviceProvider, IMapper mapper)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManger = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roles = { Roles.Admin.ToString(), Roles.LabAssistant.ToString(), Roles.Sampler.ToString(), Roles.LabTechnician.ToString(), Roles.QA.ToString(), Roles.User.ToString() };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            string email = _configuration["AdminEmail"];
            string password = _configuration["AdminPassword"];
            string number = _configuration["AdminPhoneNumber"];
            var admin = await userManger.FindByNameAsync(number);
            if (admin == null)
            {
                var newAdmin = new IdentityUser { PhoneNumber = number, UserName = number, Email = email };
                var isAdminAdded = await userManger.CreateAsync(newAdmin, password);
                if (isAdminAdded.Succeeded)
                {
                    await userManger.AddToRoleAsync(newAdmin, Roles.Admin.ToString());
                    admin = await userManger.FindByNameAsync(number);
                    AccountDto account = new AccountDto
                    {
                        Id = admin.Id,
                        Name = "Admin",
                        PhoneNumber = number,
                        IsActive = true
                    };
                    _userManager.CreateOrEdit(account);
                }
            }

        }

        public void AddAutoMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void AddHangfire(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(_configuration.GetConnectionString("SqlConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));
            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        public void AuthenticationBuilder(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero

                };
            });
        }

        public void PasswordConfiguration(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });
        }
    }
}
