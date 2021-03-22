using Api.Interfaces;
using Api.Services;
using Api.Services.IServices;
using Core.CustomEntities;
using Infrastructure.Extensions;
using Infrastructure.Serivices;
using Infrastructure.Serivices.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //colleccion de servocios
            services.AddServicers();
            services.AddTransient<ITokenProvider, TokenProvider>();
            services.AddTransient<IEmailSender, EmailSender>();

            //bindings 
            services.Configure<PaginationOptions>( options => Configuration.GetSection("PaginationOptions").Bind(options));
            services.Configure<ConnectionConfig>( options => Configuration.GetSection("ConnectionStrings").Bind(options));
            services.Configure<SmptSettings>(options => Configuration.GetSection("SmptSettings").Bind(options));

            //jwt config
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Token:Key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //HttpAccesor
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            //Cross origins
            services.AddCors(options =>
            {
                options.AddPolicy(name: "InvocesReactApp", builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });


            //swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("AxosNet", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Invoice Api",
                    Version = "1"
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/AxosNet/swagger.json", "ApiInvoices");
                options.RoutePrefix = "";
            });
            app.UseRouting();
            app.UseCors("InvocesReactApp");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
