using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Capstone.Views
{
    public static class ObjectListViews
    {
        /// <summary>
        /// Method to display a detailed view of all national parks
        /// </summary>
        /// <param name="parks"></param>
        public static void DisplayParksDetailedView(IList<Park> parks)
        {
            string[] labels = { "Location:", "Established:", "Area:", "Annual Visitors:" };
            foreach (Park park in parks)
            {
                Console.WriteLine();
                Console.WriteLine(park.Name);
                Console.WriteLine($"{labels[0],-24}{park.Location}");
                Console.WriteLine($"{labels[1],-24}{park.EstablishDate:d}");
                Console.WriteLine($"{labels[2],-24}{park.Area:n} sq km");
                Console.WriteLine($"{labels[2],-24}{park.Visitors}");
                Console.WriteLine();
                DisplayParagraphWithWordWrap(park.Description);

            }
        }

        /// <summary>
        /// Method to display information about a national park on a single line
        /// </summary>
        /// <param name="parks"></param>
        public static void DisplayParksSingleLine(IList<Park> parks)
        {
            Console.WriteLine();
            foreach (Park park in parks)
            {
                Console.WriteLine($"{park.ParkId}) {park.Name}");
            }
        }

        /// <summary>
        /// Method to display campgrounds from a provided list of campgrounds
        /// </summary>
        /// <param name="campgrounds"></param>
        public static void DisplayCampgrounds(IList<Campground> campgrounds)
        {
            string[] labels = { "Name", "Open", "Close", "Daily Fee" };
            Console.WriteLine();
            Console.WriteLine($"       {labels[0],-36}{labels[1],-12}{labels[2],-12}{labels[3],-12}");
            for (int i = 0; i < campgrounds.Count(); i++)
            {

                Console.WriteLine($"#{campgrounds[i].CampgroundId,-6}{campgrounds[i].Name,-36}{intToMonth(campgrounds[i].OpenFromMonth),-12}{intToMonth(campgrounds[i].OpenToMonth),-12}{campgrounds[i].DailyFee,-12:C}");

            }
        }

        /// <summary>
        /// Method to display a list of sites from a given list of camp sites also calculate total price
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="campgrounds"></param>
        /// <param name="campgroundSelection"></param>
        /// <param name="numDays"></param>
        public static void DisplayCampSites(IList<Site> sites, IList<Campground> campgrounds, int campgroundSelection, int numDays)
        {
            string[] labels = { "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost" };
            Console.WriteLine();
            Console.WriteLine($"{labels[0],-10}{labels[1],-16}{labels[2],-16}{labels[3],-16} {labels[4],-16} {labels[5],-16}");
            decimal price = 0m;

            // Iterate over the list of all campgrounds and find a campground that matches the selected campground
            // Use the daily fee property to calculate the total fee for the stay
            foreach (Campground campground in campgrounds)
            {
                if (campground.CampgroundId == campgroundSelection)
                {
                    price = campground.DailyFee;
                    break;
                }
            }

            if (numDays != 0)
            {
                price *= numDays;
            }

            for (int i = 0; i < sites.Count(); i++)
            {
                Console.WriteLine();
                Console.WriteLine($"#{sites[i].SiteNumber,-10}{sites[i].MaxOccupancy,-16}{FormatAccesibility(sites[i].Accessible),-16}{FormatRVLength(sites[i].MaxRVLength),-16}{FormatUtilities(sites[i].Utilities),-16}{price,-16:C}");

            }
        }

        /// <summary>
        /// Method to display campsites at all campgrounds for a chosen park
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="campgrounds"></param>
        /// <param name="campgroundSelection"></param>
        /// <param name="numDays"></param>
        public static void DisplayCampSitesParkwide(IList<Site> sites, IList<Campground> campgrounds, int numDays)
        {
            string[] labels = { "Campground", "Site ID", "Site No.", "Max Occup.", "Accessible?", "Max RV Length", "Utility", "Cost" };
            Console.WriteLine();
            Console.WriteLine($"{labels[0],-32}{labels[1],-10}{labels[2],-10}{labels[3],-10} {labels[4],-16} {labels[5],-16}{labels[6],-16}{labels[7],-16}");
            decimal price = 0m;

            // Iterate over the list of sites and determain campground name and price for proper display
            foreach (Site site in sites)
            {
                string campgroundName = "";
                decimal campgroundPrice = 0.0m;
                foreach (Campground campground in campgrounds)
                {
                    if (site.CampgroundId == campground.CampgroundId)
                    {
                        campgroundName = campground.Name;
                        campgroundPrice = campground.DailyFee;
                    }
                }

                // Calculate the price for the stay
                price = campgroundPrice * numDays;

                // Display results
                Console.WriteLine();
                Console.WriteLine($"{campgroundName, -32}{site.SiteId,-10}#{site.SiteNumber,-10}{site.MaxOccupancy,-10}{FormatAccesibility(site.Accessible),-16}{FormatRVLength(site.MaxRVLength),-16}{FormatUtilities(site.Utilities),-16}{price,-16:C}");
            }
        }

        /// <summary>
        /// Display a list of reservations from the provided reservation list
        /// </summary>
        /// <param name="reservations"></param>
        public static void DisplayReservationList(IList<Reservation> reservations)
        {
            string[] labels = { "Reservation ID", "Site ID", "Name", "Start Date", "End Date", "Date Created"};
            Console.WriteLine();
            Console.WriteLine($"{labels[0],-18}{labels[1],-12}{labels[2],-32}{labels[3],-14}{labels[4],-14}{labels[5],-14}");

            foreach (Reservation reservation in reservations)
            {
                Console.WriteLine($"{reservation.ReservationId,-18}{reservation.SiteId,-12}{reservation.Name,-32}{reservation.StartDate, -14:d}{reservation.EndDate, -14:d}{reservation.BookingDate, -14:d}");
            }
        }

        /// <summary>
        /// Display a single reservation confirmation 
        /// </summary>
        /// <param name="reservation"></param>
        public static void DisplaySingleReservation(Reservation reservation)
        {
            Console.WriteLine($"Reservation #{reservation.ReservationId} was booked on {reservation.BookingDate:d} for {reservation.StartDate:d} to {reservation.EndDate:d}.");
        }

        // Method to wrap the words of a paragraph according to console screen width, code found at:
        // https://social.msdn.microsoft.com/Forums/en-US/1ec953bc-f776-466c-a2f7-f29a2a3440c2/make-consolewriteline-wrap-words-instead-of-letters-with-methods
        private static void DisplayParagraphWithWordWrap(string text)
        {
            int width = Console.WindowWidth;
            string pattern = @"(?<line>.{1," + width + @"})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            var lines = Regex.Matches(text, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Helper method to convert an integer month into a string value
        /// </summary>
        /// <param name="month"></param>
        /// <returns>A string for the corresponding month</returns>
        private static string intToMonth(int month)
        {
            Dictionary<int, string> numberToMonth = new Dictionary<int, string>()
            {
               { 1, "January" },
               { 2, "February"},
               { 3, "March"},
               {4, "April"},
               {5, "May"},
               {6, "June"},
               {7, "July"},
               {8, "August"},
               {9, "September"},
               {10, "October"},
               {11, "November"},
               {12, "December"}

            };

            return numberToMonth[month];
        }

        /// <summary>
        /// Helper method to format the accessibilty property according to design specifications
        /// </summary>
        /// <param name="accessibility"></param>
        /// <returns>A string value corresponding to choice</returns>
        private static string FormatAccesibility(bool accessibility)
        {
            Dictionary<bool, string> boolToWord = new Dictionary<bool, string>()
            {
               { false, "No" },
               { true, "Yes"},
            };

            return boolToWord[accessibility];
        }

        /// <summary>
        /// Helper method to format the rv length property according to design specifications
        /// </summary>
        /// <param name="rvLength"></param>
        /// <returns>A string value corresponding to choice</returns>
        private static string FormatRVLength(int rvLength)
        {
            if (rvLength == 0)
            {
                return "N/A";
            }
            return $"{rvLength}";
        }

        /// <summary>
        /// Helper method to format the utilities property according to design specifications
        /// </summary>
        /// <param name="utility"></param>
        /// <returns>A string value corresponding to choice</returns>
        private static string FormatUtilities(bool utility)
        {
            Dictionary<bool, string> boolToWord = new Dictionary<bool, string>()
            {
               { false, "N/A" },
               { true, "Yes"},
            };

            return boolToWord[utility];
        }
    }
}