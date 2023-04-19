using CleanArchTemplate.Shared.Constants.Localization;
using CleanArchTemplate.Shared.Settings;

namespace CleanArchTemplate.Abstraction.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}