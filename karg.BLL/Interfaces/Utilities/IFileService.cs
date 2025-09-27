namespace karg.BLL.Interfaces.Utilities
{
    public interface IFileService
    {
        Task<string> SaveImageFile(string folderPath, byte[] fileBytes, string fileName);
        void DeleteImageFile(string fileUri);
        string CreateDirectory(string path);
        void DeleteDirectory(string path);
        Task SaveChatId(long chatId);
        Task DeleteChatId(long chatId);
        Task<List<long>> LoadChatIds();
    }
}
