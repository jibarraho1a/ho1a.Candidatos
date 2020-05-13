using ho1a.Reclutamiento.BLL.Services.Interfaces;
using ho1a.Reclutamiento.DAL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.Reclutamiento.BLL
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IRequisicionService, RequisicionService>();
            return services;
        }
    }
}
