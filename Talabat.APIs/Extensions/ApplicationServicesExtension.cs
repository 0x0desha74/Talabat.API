﻿using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IGenericRepository<>,GenericRepository<>>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //allow dependency injection of genericRepo
            //services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles)); //easy syntax

            //Validation Error Response Handling

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                                    .SelectMany(P => P.Value.Errors)
                                                                    .Select(E => E.ErrorMessage)
                                                                    .ToArray();
                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
            return services;
        }
    }
}
