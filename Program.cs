using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;

using book_app_api.Services;
using book_app_api.Infrastructure.Swagger;
using book_app_api.Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);
DefaultInit(builder);
AddApiVersioning(builder);
AddAwsDependencies(builder);
AddProjectDependencies(builder);

var app = builder.Build();
app.UseHttpLogging();
app.UseCors();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});

app.MapControllers();
app.Run();


static void DefaultInit(WebApplicationBuilder builder)
{
    builder.Services.AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    });
    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

    builder.Services.ConfigureLogging();
    builder.Services.ConfigureCors();
}

static void AddApiVersioning(WebApplicationBuilder builder)
{
    builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    });
    
    builder.Services.AddVersionedApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
}

static void AddAwsDependencies(WebApplicationBuilder builder)
{
    // Get the AWS profile information from configuration providers
    AWSOptions awsOptions = builder.Configuration.GetAWSOptions();
    // Configure AWS service clients to use these credentials
    builder.Services.AddDefaultAWSOptions(awsOptions);

    builder.Services.AddAWSService<IAmazonDynamoDB>();
    builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();
}

static void AddProjectDependencies(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IBooksService, BooksService>();
}