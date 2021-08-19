using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR_System_Backend.Model;
using HR_System_Backend.Model.Response;
using HR_System_Backend.Repository.Interface;
using HR_System_Backend.Repository.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
namespace HR_System_Backend
{
    public class Startup
    {
        private string _contentRootPath = "";
        public Startup(IConfiguration configuration,Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;
            Configuration = configuration;
             
           
        }
    

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .ConfigureApiBehaviorOptions(options =>
                {
                    //Make customize error response
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        return CustomErrorResponse(actionContext);
                    };
                }).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
            {
                Response<int> res = new Response<int>();
                res.status = false;
                res.message = actionContext.ModelState.Where(modelError => modelError.Value.Errors.Count > 0).FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage;
                return new BadRequestObjectResult(res);
            }
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HR_System API",
                    Version = "v1",
                    Description = "API To C/R/U/D Operation in HR System",
                });
            });
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            
            if (connectionString.Contains("%CONTENTROOTPATH%"))
            {
                connectionString = connectionString.Replace("%CONTENTROOTPATH%", _contentRootPath);
            }

            services.AddDbContext<HR_DBContext>(
                   options => options.UseSqlServer(connectionString)
               );
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();

                    });
            });
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IShiftRepository, ShiftRepository>();
            services.AddTransient<IDebitRepository, DebitRepository>();
            services.AddTransient<IFingerRepository, FingerRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("swagger/v1/swagger.json", "HR_System API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
