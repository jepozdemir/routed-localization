using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using RoutedLocalizationExample.Helpers;

namespace RoutedLocalizationExample.HttpModules
{
    /// <summary>
    /// Module to append lang parameter to the requested url if it's missing or unsupported
    /// </summary>
    public class LocalizationHttpModule : IHttpModule
    {
        /// <summary>
        /// List of supported locales
        /// </summary>
        private readonly IList<string> _supportedCultures;

        /// <summary>
        /// We need to have controllers list to fix missing urls
        /// </summary>
        private readonly IList<string> _controllerNames;

        public LocalizationHttpModule()
        {
            // Get list of supported cultures 
            _supportedCultures = LocalizationHelper.GetSupportedCultures();

            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type));

            _controllerNames = new List<string>();

            foreach (var controllerType in controllerTypes)
            {
                var fullName = controllerType.Name;

                // We need only name part of Controller class that is used in route
                _controllerNames.Add(fullName.Replace("Controller", ""));
            }
        }

        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        public void Init(HttpApplication application)
        {
            application.BeginRequest += new EventHandler(Application_BeginRequest);
        }

        private void Application_BeginRequest(object source, EventArgs e)
        {
            try
            {
                var application = (HttpApplication)source;
                var httpContext = application.Context;

                // We will redirect to url with defined locale only in case for HTTP GET verb
                // cause we assume that all requests with other verbs will be called from site directly
                // where all the urls created with URLHelper, so it complies with routing rules and will contain "lang" parameter
                if (string.Equals(httpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    var localizedUri = LocalizationHelper.GetLocalizedUrl(httpContext.Request.Url, _controllerNames, httpContext.Request.UserLanguages);
                    if (!string.IsNullOrEmpty(localizedUri))
                        // Perform redirect action to changed url if it exists
                        httpContext.Response.Redirect(localizedUri);
                }
            }
            catch (Exception)
            {
                // you may log error here
            }
        }

        public void Dispose() { }
    }
}