using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using System.Text;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/telegrambot")]
    public class TelegramBotController : Controller
    {
        private readonly ITelegramBotService _telegramBotService;
        private readonly IMemoryCache _cache;

        public TelegramBotController(ITelegramBotService telegramBotService, IMemoryCache cache)
        {
            _telegramBotService = telegramBotService;
            _cache = cache;
        }

        /// <summary>
        /// Handles webhook updates sent by Telegram.
        /// </summary>
        /// <param name="update">Update received from Telegram Webhook.</param>
        /// <response code="200">Successfully processed the webhook update.</response>
        /// <response code="500">An internal server error occurred while processing the webhook update.</response>
        [HttpPost("webhook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> HandleWebhookUpdate([FromBody] Update update)
        {
            try
            {
                await _telegramBotService.HandleWebhookUpdateAsync(update);

                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        /// <summary>
        /// Sends an announcement through the Telegram bot.
        /// </summary>
        /// <param name="request">The adoption request DTO containing details for the announcement.</param>
        /// <response code="200">Successfully sent the announcement.</response>
        /// <response code="429">Too many requests. The user has already sent an announcement and must wait 4 hours before sending another.</response>
        /// <response code="500">An internal server error occurred while sending the announcement.</response>
        [HttpPost("sendannouncement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendAnnouncement([FromBody] AdoptionRequestDTO request)
        {
            try
            {
                var cacheKey = $"Announcement_{request.PhoneNumber}";

                if (_cache.TryGetValue(cacheKey, out _))
                {
                    return StatusCode(StatusCodes.Status429TooManyRequests, "Ви вже надсилали оголошення. Спробуйте через 4 години.");
                }

                await _telegramBotService.SendAnnouncementAsync(request);

                _cache.Set(cacheKey, true, TimeSpan.FromHours(4));

                return Ok("Оголошення надіслано.");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}