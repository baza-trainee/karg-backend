using karg.BLL.Interfaces.Advices;
using karg.BLL.Interfaces.Animals;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.FAQs;
using karg.BLL.Interfaces.Partners;
using karg.BLL.Interfaces.Rescuers;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Profiles;
using karg.BLL.Services.Advices;
using karg.BLL.Services.Animals;
using karg.BLL.Services.Authentication;
using karg.BLL.Services.FAQs;
using karg.BLL.Services.Partners;
using karg.BLL.Services.Rescuers;
using karg.BLL.Services.Utilities;
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

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers().AddNewtonsoftJson();
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

            builder.Services.AddCors(policy => policy.AddPolicy("corspolicy", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
            builder.Services.AddScoped<IPasswordValidationService, PasswordValidationService>();
            builder.Services.AddScoped<IRescuerService, RescuerService>();
            builder.Services.AddScoped<IAdviceService, AdviceService>();
            builder.Services.AddScoped<IAnimalService, AnimalService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IPartnerService, PartnerService>();
            builder.Services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
            builder.Services.AddScoped<IFAQService, FAQService>();
            builder.Services.AddScoped<ILocalizationService, LocalizationService>();
            builder.Services.AddScoped<ILocalizationSetService, LocalizationSetService>();
            builder.Services.AddScoped<ICultureService, CultureService>();

            builder.Services.AddScoped<IAdviceRepository, AdviceRepository>();
            builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
            builder.Services.AddScoped<IRescuerRepository, RescuerRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
            builder.Services.AddScoped<IFAQRepository, FAQRepository>();
            builder.Services.AddScoped<ICultureRepository, CultureRepository>();
            builder.Services.AddScoped<ILocalizationRepository, LocalizationRepository>();
            builder.Services.AddScoped<ILocalizationSetRepository, LocalizationSetRepository>();

            builder.Services.AddAutoMapper(typeof(AdviceProfile));
            builder.Services.AddAutoMapper(typeof(AnimalProfile));
            builder.Services.AddAutoMapper(typeof(RescuerProfile));
            builder.Services.AddAutoMapper(typeof(PartnerProfile));
            builder.Services.AddAutoMapper(typeof(ImageProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("corspolicy");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
