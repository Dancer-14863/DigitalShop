using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Order.API.Authorization;
using Order.API.Data;
using Order.API.Policies;
using Order.API.Repositories;

namespace Order.API
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
            services.AddSwaggerGen();
            services.AddDbContext<OrderContext>(options =>
            {
                options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString") ?? string.Empty);
            });
            services.AddScoped<BasketRepository>();
            services.AddScoped<BasketProductRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<OrderRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("Jwt_Issuer"),
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Jwt_Secret") ?? string.Empty))
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OrderOwner", policy => policy.Requirements.Add(new OrderOwnerRequirement()));
                options.AddPolicy("BasketOwner", policy => policy.Requirements.Add(new BasketOwnerRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, BasketAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, OrderAuthorizationHandler>();

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}