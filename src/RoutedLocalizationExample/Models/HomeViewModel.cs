using System;

namespace RoutedLocalizationExample.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            DateCreated = DateTime.Now;
        }

        /// <summary>
        /// Localized string value
        /// </summary>
        public string LocalizedString { get; set; }

        /// <summary>
        /// Creation time
        /// </summary>
        public DateTime DateCreated { get; set; }
    }
}