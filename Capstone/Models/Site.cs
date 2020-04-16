using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        /// <summary>
        /// Site Id of the site
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Id of the campground associated with the camp site
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// The arbitrary campsite number of the site
        /// </summary>
        public int SiteNumber { get; set; }

        /// <summary>
        /// The maximum number of guests that can use the site
        /// </summary>
        public int MaxOccupancy { get; set; }

        /// <summary>
        /// Whether the site is handicap accessible
        /// </summary>
        public bool Accessible { get; set; }

        /// <summary>
        /// Maximum length of an RV that can be used at the site
        /// </summary>
        public int MaxRVLength { get; set; }

        /// <summary>
        /// Whether the site has utility hookups
        /// </summary>
        public bool Utilities { get; set; }
    }
}
