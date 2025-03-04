using karg.BLL.Interfaces.Utilities;
using Microsoft.Extensions.Configuration;

namespace karg.BLL.Services.Utilities
{
    public class FileService : IFileService
    {
        private readonly string _imageStoragePath;
        private readonly string _chatIdsFilePath;

        public FileService(IConfiguration configuration)
        {
            _imageStoragePath = configuration["ImageStoragePath"];
            _chatIdsFilePath = configuration["ChatIdsFilePath"];
        }

        public async Task<string> SaveImageFile(string folderPath, byte[] imageBytes, string imageName)
        {
            string filePath = Path.Combine(folderPath, imageName);
            await File.WriteAllBytesAsync(filePath, imageBytes);

            return filePath.Replace("\\", "/");
        }

        public void DeleteImageFile(string imageUri)
        {
            string filePath = Path.Combine(_imageStoragePath, imageUri.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public async Task SaveChatId(long chatId)
        {
            var chatIds = await LoadChatIds();
            if (!chatIds.Contains(chatId))
            {
                await File.AppendAllLinesAsync(_chatIdsFilePath, new[] { chatId.ToString() });
            }
        }

        public async Task<List<long>> LoadChatIds()
        {
            if (!File.Exists(_chatIdsFilePath))
            {
                return new List<long>();
            }

            var lines = await File.ReadAllLinesAsync(_chatIdsFilePath);
            return lines.Select(long.Parse).ToList();
        }
    }
}
