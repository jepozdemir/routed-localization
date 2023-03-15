using System;
using System.Linq;
using System.Collections.Generic;

namespace RoutedLocalizationExample.Helpers
{
    public static class LocalizationHelper
    {
        private readonly static IList<string> _supportedCultures = new List<string> { "en", "tr" };

        public static IList<string> GetSupportedCultures()
        {
            return _supportedCultures;
        }

        /// <summary>
        /// Get localized url
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="controllerNames"></param>
        /// <param name="languages"></param>
        public static string GetLocalizedUrl(Uri uri, IList<string> controllerNames, IList<string> languages)
        {
            var localizedUrl = string.Empty;

            var supportedCultures = GetSupportedCultures();

            // Divide requested url to parts
            var cleanedSegments = uri.Segments.Select(X => X.Replace("/", "")).ToList();

            // Check is already supported locale defined in route
            // cleanedSegments[0] is empty string, so lang parameter will be in [1] url segment
            var isCultureDefined = cleanedSegments.Count > 1 && supportedCultures.Contains(cleanedSegments[1]);

            // does request need to be changed
            var isRequestPathToHandle =
                // Url has controller's name part
                (cleanedSegments.Count > 1 && cleanedSegments.Intersect(controllerNames).Count() > 0) ||
                // This condition is for default (initial) route
                (cleanedSegments.Count == 1) ||
                // initial route with lang parameter that is not supported -> need to change it
                (cleanedSegments.Count == 2 && !supportedCultures.Contains(cleanedSegments[1]));     

            if (!isCultureDefined && isRequestPathToHandle)
            {
                var lang = "";
                // Get user preffered language from Accept-Language header
                if (languages != null && languages.Count > 0)
                {
                    // For our locale name approach we'll take only first part of lang-locale definition
                    var splitted = languages[0].Split(new char[] { '-' });
                    lang = splitted[0];
                }

                // If we don't support requested language - then redirect to requested page with default language
                if (!supportedCultures.Contains(lang))
                    lang = supportedCultures[0];

                var normalizedPathAndQuery = uri.PathAndQuery;
                if ((cleanedSegments.Count > 2 &&
                    !controllerNames.Contains(cleanedSegments[1]) &&
                    controllerNames.Contains(cleanedSegments[2])) ||
                    (cleanedSegments.Count == 2) && (!controllerNames.Contains(cleanedSegments[1])))
                {
                    // Second segment contains lang parameter, third segment contains controller name
                    cleanedSegments.RemoveAt(1);

                    // Remove wrong locale name from initial Uri
                    normalizedPathAndQuery = string.Join("/", cleanedSegments) + uri.Query;
                }

                // Finally, create new uri with language locale
                localizedUrl = string.Format("{0}://{1}:{2}/{3}{4}", uri.Scheme, uri.Host, uri.Port, lang.ToLower(), normalizedPathAndQuery);
            }

            return localizedUrl;
        }
    }
}