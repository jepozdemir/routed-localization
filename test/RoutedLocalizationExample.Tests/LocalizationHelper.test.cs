using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoutedLocalizationExample.Helpers;

namespace RoutedLocalizationExample.Tests
{
    [TestClass]
    public class LocalizationHelperTests
    {
        private IList<string> GetControllerNames()
        {
            return new List<string> { "Home", "Person", "Department" };
        }

        private IList<string> GetSupportedLanguages()
        {
            return new List<string> { "en-EN", "tr-TR" };
        }

        [TestMethod]
        public void LocalizeFreeRoute_Success()
        {
            var url = new Uri("http://sample.domain.com");
            var controllers = GetControllerNames();
            var languages = GetSupportedLanguages();

            var localizedUrl = LocalizationHelper.GetLocalizedUrl(url, controllers, languages);
            Assert.AreEqual("http://sample.domain.com:80/en/", localizedUrl);
        }

        [TestMethod]
        public void LocalizeDefaultRoute_WithBadLocale_Success()
        {
            var url = new Uri("http://sample.domain.com/wronglocale");
            var controllers = GetControllerNames();
            var languages = GetSupportedLanguages();

            var localizedUrl = LocalizationHelper.GetLocalizedUrl(url, controllers, languages);
            Assert.AreEqual("http://sample.domain.com:80/en", localizedUrl);
        }

        [TestMethod]
        public void LocalizeHomeRoute_Success()
        {
            var url = new Uri("http://sample.domain.com/Home/Index");
            var controllers = GetControllerNames();
            var languages = GetSupportedLanguages();

            var localizedUrl = LocalizationHelper.GetLocalizedUrl(url, controllers, languages);
            Assert.AreEqual("http://sample.domain.com:80/en/Home/Index", localizedUrl);
        }

        [TestMethod]
        public void LocalizeHomeRoute_WithBadLocale_Success()
        {
            var url = new Uri("http://sample.domain.com/wrongLocale/Home/Index");
            var controllers = GetControllerNames();
            var languages = GetSupportedLanguages();

            var localizedUrl = LocalizationHelper.GetLocalizedUrl(url, controllers, languages);
            Assert.AreEqual("http://sample.domain.com:80/en/Home/Index", localizedUrl);
        }

        [TestMethod]
        public void LocalizeHomeRoute_WithoutAnySupportedLanguages_Success()
        {
            var url = new Uri("http://sample.domain.com/Home/Index");
            var controllers = GetControllerNames();
            IList<string> languages = null;

            var localizedUrl = LocalizationHelper.GetLocalizedUrl(url, controllers, languages);
            Assert.AreEqual("http://sample.domain.com:80/en/Home/Index", localizedUrl);
        }

        [TestMethod]
        public void Localize_NotSupportedLocale_Success()
        {
            var url = new Uri("http://sample.domain.com/");
            var controllers = GetControllerNames();
            var languages = new List<string> { "es-ES" };

            var localizedUrl = LocalizationHelper.GetLocalizedUrl(url, controllers, languages);
            Assert.AreEqual("http://sample.domain.com:80/en/", localizedUrl);
        }
    }
}
