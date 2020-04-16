using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    // A group of static methods that allow the program to validate user input against a list of objects
    public static class Validators
    {
        /// <summary>
        ///  Method to build a list of valid selections for a given list of parks in order to validate user input
        /// </summary>
        /// <param name="parks"></param>
        /// <returns>A list of integers representing valid selctions for parkId</returns>
        public static List<int> GetValidParkIds(IList<Park> parks)
        {
            List<int> validSelections = new List<int>();

            // Iterate over the list and add all valid park ids to the valid selections list
            foreach (Park park in parks)
            {
                validSelections.Add(park.ParkId);
            }

            return validSelections;
        }

        /// <summary>
        /// Method to build a list of valid selections for a given list of campgrounds in order to validate user input
        /// </summary>
        /// <param name="campgrounds"></param>
        /// <returns>A list of integers representing valid selections for campgroundId</returns>
        public static List<int> GetValidCampgroundIds(IList<Campground> campgrounds)
        {
            List<int> validSelections = new List<int>();

            // Explicitly add 0 to the list to allow the user to cancel a transaction
            validSelections.Add(0);

            // Iterate over the list and add all valid campground Ids to the valid selections list
            foreach (Campground campground in campgrounds)
            {
                validSelections.Add(campground.CampgroundId);
            }

            return validSelections;
        }

        /// <summary>
        /// Method to build a list of valid selections for a given list of sites in order to validate user input
        /// </summary>
        /// <param name="sites"></param>
        /// <returns>A list of integers representing valid selections for site number</returns>
        public static List<int> GetValidSiteNumber(IList<Site> sites)
        {
            List<int> validSelections = new List<int>();

            // Explicitly add 0 to the list to allow the user to cancel a transaction
            validSelections.Add(0);

            // Iterate over the list and all all valid site numbers to the valid selections list
            foreach (Site site in sites)
            {
                validSelections.Add(site.SiteNumber);
            }

            return validSelections;
        }

        /// <summary>
        /// Method to build a list of valid selections for a given list of sites in order to validate user input
        /// </summary>
        /// <param name="sites"></param>
        /// <returns>A list of integers representing valid selections for site Id</returns>
        public static List<int> GetValidSiteId(IList<Site> sites)
        {
            List<int> validSelections = new List<int>();

            // Explicitly add 0 to the list to allow the user to cancel a transaction
            validSelections.Add(0);

            // Iterate over the list and all all valid site numbers to the valid selections list
            foreach (Site site in sites)
            {
                validSelections.Add(site.SiteId);
            }

            return validSelections;
        }
    }
}
