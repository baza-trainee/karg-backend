using karg.BLL.Interfaces.Authentication;
using karg.BLL.Interfaces.Entities;
using karg.DAL.Interfaces;
using karg.DAL.Models;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace karg.API.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (!HasAuthorization(endpoint))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Forbidden access attempt.");
                await WriteForbiddenResponse(context, "У вас недостатньо прав для доступу або ваш токен недійсний.");
                return;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
                var rescuerRepository = scope.ServiceProvider.GetRequiredService<IRescuerRepository>();

                try
                {
                    var principal = jwtTokenService.DecodeJwtToken(token);
                    var rescuerId = principal.FindFirst("Id")?.Value;
                    var rescuer = await rescuerRepository.GetById(int.Parse(rescuerId));
                    var rescuerRole = principal.FindFirst("Role")?.Value;

                    if (await jwtTokenService.GetJwtTokenById(rescuer.TokenId) != token)
                    {
                        _logger.LogWarning("Forbidden access attempt.");
                        await WriteForbiddenResponse(context, "У вас недостатньо прав для доступу або ваш токен недійсний.");
                        return;
                    }

                    context.Items["Id"] = rescuerId;
                    context.Items["Role"] = rescuerRole;

                    if (IsRescuerController(endpoint) && !HasAccess(rescuerId, rescuerRole, context.Request.Query["id"].FirstOrDefault()))
                    {
                        await WriteForbiddenResponse(context, "У вас недостатньо прав для оновлення цього користувача.");
                        return;
                    }

                    _logger.LogInformation($"Rescuer {rescuerId} is valid.");
                    await _next(context);
                }
                catch
                {
                    _logger.LogWarning("Invalid token.");
                    await WriteForbiddenResponse(context, "У вас недостатньо прав для доступу або ваш токен недійсний.");
                }
            }
        }

        private async Task WriteForbiddenResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.WriteAsync(message);
        }

        private bool HasAuthorization(Endpoint endpoint)
        {
            return endpoint?.Metadata
                .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
                .Any() ?? false;
        }

        private bool IsRescuerController(Endpoint endpoint)
        {
            var controllerDescriptor = endpoint?.Metadata
               .OfType<ControllerActionDescriptor>()
               .FirstOrDefault();

            return string.Equals(controllerDescriptor?.ControllerName, "Rescuer");
        }

        private bool HasAccess(string rescuerId, string rescuerRole, string idFromQuery)
        {
            return rescuerId == idFromQuery || rescuerRole.Equals("Director", StringComparison.OrdinalIgnoreCase);
        }
    }
}