using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using RoutedLocalizationExample.ActionFilters;
using RoutedLocalizationExample.Models;
using RoutedLocalizationExample.Resources;

namespace RoutedLocalizationExample.Controllers
{
    public class HomeController : Controller
    {
        // Localize string without any external impact
        public ActionResult Index()
        {
            // Get string from strongly typed localization resources
            var model = new HomeViewModel { LocalizedString = Strings.SomeLocalizedString };
            return View(model);
        }

        // Localize string without any external impact with caching
        [OutputCache(Duration = 3600)]
        public ActionResult CachedIndex()
        {
            var model = new HomeViewModel { LocalizedString = Strings.SomeLocalizedString };
            return View("Index", model);
        }

        // Get language from quuery string (by binder)
        public ActionResult LangFromQueryString(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

            var model = new HomeViewModel { LocalizedString = Strings.SomeLocalizedString };
            return View("Index", model);
        }

        // Get language as a parameter from route data
        public ActionResult LangFromRouteValues(string lang)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(lang);

            var model = new HomeViewModel { LocalizedString = Strings.SomeLocalizedString };
            return View("Index", model);
        }

        // Get language in action filter (from route parameter)
        [Localization]
        public ActionResult LangFromRouteInActionFilter()
        {
            var model = new HomeViewModel { LocalizedString = Strings.SomeLocalizedString };
            return View("Index", model);
        }

        // Get language in action filter (from route parameter) with caching result
        [Localization]
        [OutputCache(Duration = 3600)]
        public ActionResult CachedLangFromRouteInActionFilter()
        {
            var model = new HomeViewModel { LocalizedString = Strings.SomeLocalizedString };
            return View("Index", model);
        }
    }
}