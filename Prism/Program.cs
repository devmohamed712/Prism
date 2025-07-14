using AutoMapper;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Prism.API;
using Prism.BL.AutoMapper;
using Prism.BL.Dtos;
using Prism.BL.Helpers;
using Prism.BL.Helpers.Configrations;
using Prism.BL.Managers.Business;
using Prism.BL.Managers.Business.BasinessSamples;
using Prism.BL.Managers.Business.BusinessAddresses;
using Prism.BL.Managers.Business.BusinessContacts;
using Prism.BL.Managers.Business.BusinessTests;
using Prism.BL.Managers.Common;
using Prism.BL.Managers.Notification;
using Prism.BL.Managers.Order;
using Prism.BL.Managers.Order.OrderDetails;
using Prism.BL.Managers.Order.OrderReports;
using Prism.BL.Managers.Order.OrderSamples;
using Prism.BL.Managers.Order.OrderSamplesTests;
using Prism.BL.Managers.User;
using Prism.BL.Managers.Utilities;
using Prism.DAL;
using Prism.Repository;
using QRCodeResults.BL.Enums;
using System.Text;


string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
var connectionString = configuration.GetConnectionString("SqlConnection");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region MyBlock
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins(
                              "http://localhost",
                              "https://localhost",
                              "http://localhost:8100",
                              "http://localhost:4200",
                              "capacitor://localhost",
                              "ionic://localhost",
                              "http://results.prismlabllc.com/",
                              "https://results.prismlabllc.com/"
                              ).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                      });
});
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddDbContext<PrismContext>();

// Dependancy Injection
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUtilitiesManager, UtilitiesManager>();
builder.Services.AddTransient<ICommonManager, CommonManager>();
builder.Services.AddTransient<INotificationManager, NotificationManager>();
builder.Services.AddTransient<IUserManager, UserManager>();
builder.Services.AddTransient<IBusinessManager, BusinessManager>();
builder.Services.AddTransient<IBusinessAddressesManager, BusinessAddressesManager>();
builder.Services.AddTransient<IBusinessContactsManager, BusinessContactsManager>();
builder.Services.AddTransient<IBusinessSamplesManager, BusinessSamplesManager>();
builder.Services.AddTransient<IBusinessTestsManager, BusinessTestsManager>();
builder.Services.AddTransient<IOrderManager, OrderManager>();
builder.Services.AddTransient<IOrderDetailsManager, OrderDetailsManager>();
builder.Services.AddTransient<IOrderSamplesManager, OrderSamplesManager>();
builder.Services.AddTransient<IOrderSamplesTestsManager, OrderSamplesTestsManager>();
builder.Services.AddTransient<IOrderReportsManager, OrderReportsManager>();


#region AutoMapper
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new Mapping());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion
#region AuthenticationBuilder
builder.Services.AddAuthentication(options =>
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
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero

    };
});
#endregion
#region Password Configrations
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
});
#endregion
#region Hangfire Configrations
// Add Hangfire services.
builder.Services.AddHangfire(configuration => configuration
.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(connectionString, new SqlServerStorageOptions
{
    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
    QueuePollInterval = TimeSpan.Zero,
    UseRecommendedIsolationLevel = true,
    DisableGlobalLocks = true
}));
// Add the processing server as IHostedService
builder.Services.AddHangfireServer();
#endregion
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region MyBlock
app.UseRouting();
app.UseHangfireDashboard();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"UploadedFiles")),
    RequestPath = new PathString("/UploadedFiles")
});
app.UseCors(MyAllowSpecificOrigins);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("api", "{controller=Base}/{action=Index}/{id?}");
    endpoints.MapControllerRoute("api", "{controller}/{action}/{id?}");
    endpoints.MapHangfireDashboard();
});
#region Update DB
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    using (var context = serviceScope.ServiceProvider.GetService<PrismContext>())
    {
        context.Database.Migrate();
    }
}
#endregion
#region Create Roles & Admin
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManger = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();
    string[] roles = { Roles.Admin.ToString(), Roles.LabAssistant.ToString(), Roles.Sampler.ToString(), Roles.LabTechnician.ToString(), Roles.QA.ToString(), Roles.User.ToString() };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            var newRole = new IdentityRole(role)
            {
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            await roleManager.CreateAsync(newRole);
        }
    }
    string email = configuration["AdminEmail"];
    string password = configuration["AdminPassword"];
    string number = configuration["AdminPhoneNumber"];
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
            using (var scope = app.Services.CreateScope())
            {
                var _userManager = scope.ServiceProvider.GetRequiredService<IUserManager>();
                _userManager.CreateOrEdit(account);
            }
        }
    }
}
#endregion
#endregion

app.Run();
