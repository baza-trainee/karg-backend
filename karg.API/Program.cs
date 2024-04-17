using karg.BLL.Interfaces;
using karg.BLL.Services;
using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<KargDbContext>((serviceProvider, options) =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("KargDbConnection"), new MySqlServerVersion(new Version(8, 0, 30)));
            });

            builder.Services.AddScoped<IAnimalService, AnimalService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();

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
