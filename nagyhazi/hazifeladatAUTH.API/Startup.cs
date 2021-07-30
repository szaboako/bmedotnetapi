using hazifeladat.BLL.DTOs;
using hazifeladat.BLL.Exceptions;
using hazifeladat.BLL.Interfaces;
using hazifeladat.BLL.Services;
using hazifeladat.DAL;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Cosmos;

namespace hazifeladatAUTH.API
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DemoRead", policy =>
                policy.RequireClaim(
                "http://schemas.microsoft.com/identity/claims/scope",
                "demo.read"));

                options.AddPolicy("Admin", policy =>
                  policy.RequireClaim(
                  "http://schemas.microsoft.com/identity/claims/objectidentifier",
                  "d3edd241-c7e3-435e-aa9f-39ddbab988e2"));

            });

            services.AddControllers();
            services.AddDbContext<NorthwindContext>(o => o.UseMySQL(Configuration["ConnectionStrings:MysqlConnection"]));
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IPictureService, PictureService>();
            services.AddAutoMapper(typeof(WebApiProfile));
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => false;
                options.Map<EntityNotFoundException>(
                    (ctx, ex) =>
                    {
                        var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                        pd.Title = ex.Message;
                        return pd;
                    }
                );
            });

            services.AddOpenApiDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/
            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi(); // ez az MW szolgálja ki az OpenAPI JSON-t
            app.UseSwaggerUi3(); //ez az MW adja az OpenAPI felületet

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
