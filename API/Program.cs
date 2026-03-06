using API.ServiceRegister;
using iFinancing360.API.Helper;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Service.Messaging.MessageBrokerConfiguration;
using DotNetEnv;
using Serilog;
using Serilog.Formatting.Compact;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

// Add services to the container.
builder.Services.AddService();
builder.Services.AddGlobalConfig();

builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter()));
    
builder.Services.AddKafkaServices();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
        .AddJsonOptions(jsonOptions =>
        {
            jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Menambahkan header
    c.OperationFilter<AddRequiredHeaderParameter>();

    // Menambahkan kolom token authorization di swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Masukkan token JWT Anda di kolom ini. Contoh: Bearer {token}"
    });

    // Menambahkan token authorization requirement
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
  {
      {
          new Microsoft.OpenApi.Models.OpenApiSecurityScheme
          {
              Reference = new Microsoft.OpenApi.Models.OpenApiReference
              {
                  Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                  Id = "Bearer"
              }
          },
          Array.Empty<string>()
      }
  });
});

builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
        });
// builder.WebHost.UseUrls("http://localhost:5201"); // temp

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 100 * 1024 * 1024; // 100 MB

});


// Add User Authorization
builder.Services.AddUserAuthorization();


var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpMetrics();
app.MapMetrics();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// publish
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAppSetting();
app.MapControllers();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        // Menambahkan header baru
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "User",
            In = ParameterLocation.Header,
            Required = false // set to false if this is optional
            ,
            Schema = new OpenApiSchema
            {
                Default = new OpenApiString("bmltZEE=")
            }

        });
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "SystemDate",
            In = ParameterLocation.Header,
            Required = false // set to false if this is optional
            ,
            Schema = new OpenApiSchema
            {
                Default = new OpenApiDateTime(DateTimeOffset.Now)
            }

        });
    }
}
