namespace karg.BLL.Interfaces.Localization
{
    public interface ICultureService
    {
        Task<bool> IsCultureCodeInDatabase(string cultureCode);
    }
}
