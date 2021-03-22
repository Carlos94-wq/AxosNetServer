using Core.Interfaces;
using Core.Services;
using Core.Services.IServices;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class ServicesColllection
    {
        public static IServiceCollection AddServicers(this IServiceCollection services) 
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IinvoiceRepository, InvoiceRepository>();
            services.AddTransient<IInvoiceService, InvoiceService>();

            services.AddTransient<IInvoiceDetailRepository, InvoiceDetailRepository>();
            services.AddTransient<iDetailService, DetailService>();

            //no hago un servicio porque no hay logica de negocio que ejecutar
            services.AddTransient<ICatalogueRepository, CatalogueRepository>();

            return services;
        }
    }
}
