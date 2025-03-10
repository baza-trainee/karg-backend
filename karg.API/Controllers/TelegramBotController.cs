using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using System.Text;
using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Utilities;

namespace karg.API.Controllers
{
    [ApiController]
    [Route("api/telegrambot")]
    public class TelegramBotController : Controller
    {
        private readonly ITelegramBotService _telegramBotService;

        public TelegramBotController(ITelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
        }

        /// <summary>
        /// Handles webhook updates sent by Telegram.
        /// </summary>
        /// <param name="update">Update received from Telegram Webhook.</param>
        /// <response code="200">Successfully processed the webhook update.</response>
        /// <response code="500">An internal server error occurred while processing the webhook update.</response>
        [HttpPost]
        [Route("webhook")]
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
        /// <response code="500">An internal server error occurred while sending the announcement.</response>
        [HttpPost]
        [Route("sendannouncement")]
        public async Task<IActionResult> SendAnnouncement([FromBody] AdoptionRequestDTO request)
        {
            try
            {
                await _telegramBotService.SendAnnouncementAsync(request);

                return Ok("Оголошення надіслано.");
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}