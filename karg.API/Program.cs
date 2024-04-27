using karg.BLL.Interfaces;
using karg.BLL.Services;
using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace karg.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "karg API",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddDbContext<KargDbContext>((serviceProvider, options) =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("KargDbConnection"), new MySqlServerVersion(new Version(8, 0, 30)));
            });

            builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
            builder.Services.AddScoped<IPasswordValidationService, PasswordValidationService>();
            builder.Services.AddScoped<IRescuerService, RescuerService>();
            builder.Services.AddScoped<IAnimalService, AnimalService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IPartnerService, PartnerService>();
            builder.Services.AddScoped<IAnimalMappingService, AnimalMappingService>();
            builder.Services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));

            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped<IRescuerRepository, RescuerRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
