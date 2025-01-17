namespace karg.BLL.Interfaces.Localization
{
    public interface ILocalizationSetService
    {
        Task<int> CreateAndSaveLocalizationSet(string valueEn, string valueUa);
        Task DeleteLocalizationSets(List<int> localizationSetIds);
        Task<int> UpdateLocalizationSet(int localizationSetId, string value_en, string value_ua);
    }
}
