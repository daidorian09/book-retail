﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace WebAPI.Infrastructure;

public static class Bootstrap
{
    public static IServiceCollection AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        //  builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddFluentValidationAutoValidation(conf =>
        {
            conf.DisableDataAnnotationsValidation = true;
        })
        .AddValidatorsFromAssemblyContaining<Locator>()
        .AddValidatorsFromAssemblyContaining<Application.Locator>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
                   options.SuppressModelStateInvalidFilter = true);

        return builder.Services;
    }
}
