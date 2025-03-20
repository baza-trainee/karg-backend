using karg.API.Middlewares;
using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Email;
using karg.BLL.Interfaces.Entities;
using karg.BLL.Interfaces.Localization;
using karg.BLL.Interfaces.Utilities;
using karg.BLL.Profiles;
using karg.BLL.Services.Authentication;
using karg.BLL.Services.Email;
using karg.BLL.Services.Entities;
using karg.BLL.Services.Localization;
using karg.BLL.Services.Utilities;
using karg.DAL.Context;
using karg.DAL.Interfaces;
using karg.DAL.Models.Enums;
using karg.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Telegram.Bot;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KargDbContext>((serviceProvider, options) =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("KargDbConnection"),
        new MySqlServerVersion(new Version(8, 0, 30)), mySqlConfig =>
        {
            mySqlConfig.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null
            );
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Issuer"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Director", policy => policy.RequireClaim("Role", new[] { RescuerRole.Director.ToString() }));
    options.AddPolicy("Employee", policy => policy.RequireClaim("Role", new[] { RescuerRole.Employee.ToString() }));
});

builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers().AddNewtonsoftJson();

var client = new TelegramBotClient(builder.Configuration["TelegramBot:Token"]);
await client.SetWebhookAsync(builder.Configuration["TelegramBot:WebhookUrl"]);

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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(policy => policy.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddSingleton<ITelegramBotClient>(client);

builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IPasswordValidationService, PasswordValidationService>();
builder.Services.AddScoped<IRescuerService, RescuerService>();
builder.Services.AddScoped<IAdviceService, AdviceService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
builder.Services.AddScoped<IFAQService, FAQService>();
builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<ILocalizationSetService, LocalizationSetService>();
builder.Services.AddScoped<IYearResultService, YearResultService>();
builder.Services.AddScoped<ICultureService, CultureService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ITelegramBotService, TelegramBotService>();

builder.Services.AddScoped<IAdviceRepository, AdviceRepository>();
builder.Services.AddScoped<IYearResultRepository, YearResultRepository>();
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IRescuerRepository, RescuerRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
builder.Services.AddScoped<IFAQRepository, FAQRepository>();
builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddScoped<ICultureRepository, CultureRepository>();
builder.Services.AddScoped<ILocalizationRepository, LocalizationRepository>();
builder.Services.AddScoped<ILocalizationSetRepository, LocalizationSetRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddAutoMapper(typeof(AdviceProfile));
builder.Services.AddAutoMapper(typeof(AnimalProfile));
builder.Services.AddAutoMapper(typeof(RescuerProfile));
builder.Services.AddAutoMapper(typeof(PartnerProfile));
builder.Services.AddAutoMapper(typeof(FAQProfile));
builder.Services.AddAutoMapper(typeof(YearResultProfile));
builder.Services.AddAutoMapper(typeof(JwtTokenProfile));
builder.Services.AddAutoMapper(typeof(ContactProfile));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<KargDbContext>();
    context.Database.Migrate();

    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    string fileStoragePath = configuration["ImageStoragePath"];
    string rootPath = Path.Combine(fileStoragePath, "uploads");
    string[] categories = { "animal", "advice", "rescuer", "partner", "yearresult" };

    foreach (var category in categories)
    {
        string categoryPath = Path.Combine(rootPath, category);
        if (!Directory.Exists(categoryPath))
        {
            Directory.CreateDirectory(categoryPath);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();