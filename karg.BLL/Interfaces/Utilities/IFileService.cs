namespace karg.BLL.Interfaces.Utilities
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(string folderPath, byte[] fileBytes, string fileName);
        void DeleteFile(string fileUri);
        string CreateDirectory(string path);
        void DeleteDirectory(string path);
    }
}
