using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        /// <summary>
        /// The Id of the campground.
        /// </summary>
        public int CampgroundId { get; set; }

        /// <summary>
        /// The Id of the National Park the campground is a part of.
        /// </summary>
        public int ParkId { get; set; }

        /// <summary>
        /// The name of the campground.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The month the campground is open from expressed as an integer 1 = January, 2 = February etc.
        /// </summary>
        public int OpenFromMonth { get; set; }

        /// <summary>
        /// The month that the campground closes expressed as an integer 1 = January, 2 = February etc.
        /// </summary>
        public int OpenToMonth { get; set; }

        /// <summary>
        /// The fee required to stay at the campground for one day.
        /// </summary>
        public decimal DailyFee { get; set; }
    }
}
