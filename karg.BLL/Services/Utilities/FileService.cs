using karg.BLL.Interfaces.Utilities;
using Microsoft.Extensions.Configuration;

namespace karg.BLL.Services.Utilities
{
    public class FileService : IFileService
    {
        private readonly string _baseFilePath;

        public FileService(IConfiguration configuration)
        {
            _baseFilePath = configuration["FileStoragePath"];
        }

        public async Task<string> SaveFileAsync(string folderPath, byte[] fileBytes, string fileName)
        {
            string filePath = Path.Combine(folderPath, fileName);
            await File.WriteAllBytesAsync(filePath, fileBytes);

            return filePath.Replace("\\", "/");
        }

        public void DeleteFile(string fileUri)
        {
            string filePath = Path.Combine(_baseFilePath, fileUri.TrimStart('/'));
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
    }
}
