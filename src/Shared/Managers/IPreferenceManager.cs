using System.Threading.Tasks;

using CleanArchTemplate.Shared.Settings;
using CleanArchTemplate.Shared.Wrapper;

namespace CleanArchTemplate.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}