
using System.Reflection;
using BusinessChat.Application.Common.Interfaces;
using BusinessChat.Application.Stock.DTO;
using BusinessChat.Infrastructure.Identity;
using BusinessChat.Infrastructure.Messaging;
using BusinessChat.Infrastructure.Persistence;
using BusinessChat.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessChat.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddTransient<IStockQuery, StockQuery>();
            services.AddTransient<IStockResponse, StockResponse>();
            services.AddTransient<IStooqService, StooqService>();

            return services;
        }
    }
}
