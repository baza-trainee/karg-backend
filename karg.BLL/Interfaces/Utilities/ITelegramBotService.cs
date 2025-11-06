using karg.BLL.DTO.Utilities;
using Telegram.Bot.Types;

namespace karg.BLL.Interfaces.Utilities
{
    public interface ITelegramBotService
    {
        Task HandleWebhookUpdateAsync(Update update);
        Task SendAnnouncementAsync(AdoptionRequestDTO request);
    }
}
