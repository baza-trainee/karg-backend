using karg.BLL.DTO.Utilities;
using karg.BLL.Interfaces.Utilities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace karg.BLL.Services.Utilities
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IFileService _fileService;

        public TelegramBotService(ITelegramBotClient botClient, IFileService fileService)
        {
            _botClient = botClient;
            _fileService = fileService;
        }

        public async Task HandleWebhookUpdateAsync(Update update)
        {
            try
            {
                if (update.Message?.Text == "/start")
                {
                    var chatId = update.Message.Chat.Id;

                    await _fileService.SaveChatId(chatId);
                    await _botClient.SendTextMessageAsync(chatId, "Вас додано до розсилки!");
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error processing webhook update: {exception.Message}");
            }
        }

        public async Task SendAnnouncementAsync(AdoptionRequestDTO request)
        {
            try
            {
                var adoptionInfo = $"📢 Заява на всиновлення\n" +
                                     $"👤 Ім'я: {request.FullName}\n" +
                                     $"📞 Номер телефону: {request.PhoneNumber}\n" +
                                     $"🐾 Тварина, яку всиновлюють: {request.AnimalName}";
                var chatIds = await _fileService.LoadChatIds();

                foreach (var chatId in chatIds)
                {
                    if (!string.IsNullOrEmpty(request.AnimalImageUri))
                    {
                        var inputFile = InputFile.FromUri(request.AnimalImageUri);
                        await _botClient.SendPhotoAsync(chatId, inputFile, caption: adoptionInfo);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(chatId, adoptionInfo);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException($"Error sending announcement: {exception.Message}");
            }
        }
    }
}
