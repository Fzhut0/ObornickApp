

using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {          
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LogUserActivity>();
            services.AddSignalR();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFacebookMessageService, FacebookMessageService>();

            return services;
        }
    }
}