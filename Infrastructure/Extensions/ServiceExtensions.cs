using Microsoft.AspNetCore.HttpLogging;


namespace book_app_api.Infrastructure.Extensions;


public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        string[]? allowedCorsHosts =
            Environment.GetEnvironmentVariable("FRONTEND_ALLOWED_HOSTS")
                ?.Split(";", StringSplitOptions.RemoveEmptyEntries);

        if (allowedCorsHosts is not null)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(allowedCorsHosts)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }

    public static void ConfigureLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestMethod
                                    | HttpLoggingFields.RequestPath
                                    | HttpLoggingFields.RequestQuery
                                    | HttpLoggingFields.RequestScheme
                                    | HttpLoggingFields.RequestProtocol;
        });
    }
}