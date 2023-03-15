using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using RoutedLocalizationExample.Helpers;

namespace RoutedLocalizationExample.ActionFilters
{
    /// <summary>
    /// Set language that is defined in route parameter "lang"
    /// </summary>
    public class LocalizationAttribute : ActionFilterAttribute
    {
        private readonly IList<string> _supportedCultures;
        private readonly string _defaultLanguage;

        public LocalizationAttribute()
        {
            // Get supported cultures
            _supportedCultures = LocalizationHelper.GetSupportedCultures();

            // Set default language
            _defaultLanguage = _supportedCultures[0];
        }

        /// <summary>
        /// Apply culture to current thread
        /// </summary>
        /// <param name="lang">locale name</param>
        private void SetCurrentCulture(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Get lang from route values
            string lang = (string)filterContext.RouteData.Values["lang"] ?? _defaultLanguage;

            // If we haven't found appropriate culture - set default language then
            if (!_supportedCultures.Contains(lang))
                lang = _defaultLanguage;

            SetCurrentCulture(lang);
        }
    }
}