using FooRestaurantApi.APILayer.Authorization;
using FooRestaurantApi.APILayer.Filters;
using FooRestaurantApi.RepositoryLayer.Context;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Implemantations;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.Services.Implemantations;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;

namespace FooRestaurantApi.APILayer.Extensions
{
    public static class CustomExtensions
    {
        public static void RegisterRequiredServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region ServiceLayer
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(IUserService).Assembly));
            services.AddScoped<IUserService, UserService>();


            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICartService, CartService>();
            #endregion

            #region RepositoryLayer
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICartRepository, CartRepository>();
            #endregion

            #region ApiLayer




            services.AddHttpContextAccessor();






            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "APILayer", Version = "v1" });
                // To Enable authorization using Swagger (JWT)    
                c.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme()
                {
                    Name = "FooRestaurantApiAuth",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Cookie,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Cookie"

                                }
                            },
                            new string[] {}

                    }
                });
            });


            services.AddDbContext<PostgreDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection")));




            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });
            services.AddMemoryCache();






            services.AddScoped<IsOrderStatusUpdateValidForUserFilter>();
            services.AddScoped<IsOrderStatusUpdateValidForAdminFilter>();

            services.AddScoped<ICustomCookieAuthenticationEvent, CustomCookieAuthenticationEvent>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme

            )
            .AddCookie(options =>
            {
                options.ForwardForbid = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Cookie.MaxAge = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = false;

                options.Cookie.Name = "FooRestaurantApiAuth";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/api/user/Login";
                options.EventsType = typeof(ICustomCookieAuthenticationEvent);
            });
            services.AddScoped<IAuthorizationHandler, RoleIdAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, RoleIddPolicyProvider>();

            #endregion




        }


        public static IActionResult CreateCustomResponse<Terror, Tsource>(this ApiResponseDto<Terror, Tsource> apiresponseDto) where Terror : class
        {

            return new ObjectResult(apiresponseDto.Success ? apiresponseDto.SuccessData : apiresponseDto.ErrorData) { StatusCode = (int)apiresponseDto.StatusCode };

        }

    }
}