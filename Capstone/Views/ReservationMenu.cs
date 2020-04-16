using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{

    public class ReservationMenu : CLIMenu
    {
        // DAOs required for functionality
        protected IParkDAO parkDAO;
        protected ICampgroundDAO campgroundDAO;
        protected ISiteDAO siteDAO;
        protected IReservationDAO reservationDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public ReservationMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) :
            base("Sub-Menu 1")
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "Search for and book a campsite");
            this.menuOptions.Add("2", "Search for campsites parkwide");
            this.menuOptions.Add("3", "View all upcoming reservations");
            this.menuOptions.Add("B", "Back to Main Menu");
            this.quitKey = "B";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            switch (choice)
            {
                case "1": // Allow the user to search for and book a campsite including advanced search options by calling the MakeReservation method
                    ReservationSearch();
                    Pause("");
                    return true;
                case "2": // Allow the user to search for a reservation across the entire park
                    ReservationSearchByPark();
                    Pause("");
                    return true;
                case "3": // Allow the user to view all upcoming reservations from the current date to a chosen end date
                    DateTime endDate = GetDateTime("Enter a date to see all reservations from now until that date: ");
                    IList<Reservation> reservations = reservationDAO.ViewAllUpcomingReservations(DateTime.Now, endDate);
                    ObjectListViews.DisplayReservationList(reservations);
                    Pause("");
                    return true;
            }
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.DarkRed);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Reservations"));
            ResetColor();
        }

        /// <summary>
        /// Helper method to perform the reservation search and prompt user for additional input
        /// </summary>
        private void ReservationSearch()
        {
            // Display a list of all parks for the user to choose from
            IList<Park> parks = parkDAO.GetAllParks();
            ObjectListViews.DisplayParksSingleLine(parks);

            // Prompt the user to select a valid park 
            int parkId = GetValidInteger("Please choose a park to view available campgrounds: ", Validators.GetValidParkIds(parks));

            // Get and display a list of campgrounds at the selected park
            IList<Campground> campgrounds = campgroundDAO.GetCampgroundsByParkId(parkId);
            ObjectListViews.DisplayCampgrounds(campgrounds);

            // Prompt user to choose a valid campground if the user selects 0 cancel the transaction and return to the reservation menu
            int campgroundId = GetValidInteger("Choose a campground (Select 0 to cancel): ", Validators.GetValidCampgroundIds(campgrounds));
            if (campgroundId == 0) return;

            // Prompt the user to enter an arrival and departure date and store them and then calculate the total number of days of the stay
            DateTime startDate = GetDateTime("Please enter an arrival date: ");
            DateTime endDate = GetDateTimeAfterDate("Please enter a departure date: ", startDate);
            int numDays = (int)((endDate - startDate).TotalDays);

            // Ask the user if they would like to perform an advanced search
            bool isAdvancedSearch = GetBool("Would you like to perform an advanced search (y/n): ");

            // Create a list to hold the results of the search
            IList<Site> sites = new List<Site>();

            // If the user selected to perform an advanced search get additional parameters, then build a list of sites matching search criteria
            if (isAdvancedSearch)
            {
                int maxOccupancyRequired = GetInteger("What is the max occupancy required: ");
                bool isAccessible = GetBool("Do you need a weelchair accessible site (y/n): ");
                int rvSizeRequired = GetInteger("What size RV parking is required (Enter 0 for no RV): ");
                bool isHookupRequired = GetBool("Do you need utility hookups (y/n): ");
                sites = siteDAO.GetAvailableSites(campgroundId, startDate, endDate, maxOccupancyRequired, isAccessible, rvSizeRequired, isHookupRequired);
            }
            else
            {
                sites = siteDAO.GetAvailableSites(campgroundId, startDate, endDate);
            }

            // Check if the search returned any results and print a message indicating no results or a list showing matching results
            if (sites.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry but no matching results were found for your search, returning to reservation menu.");
                return;
            }
            else
            {
                ObjectListViews.DisplayCampSites(sites, campgroundDAO.GetAllCampgrounds(), campgroundId, numDays);
            }

            // Prompt the user to choose a valid site to reserve allow the user to select 0 to cancel the reservation
            int siteNumber = GetValidInteger("Select a site that you want to reserve (enter 0 to cancel):", Validators.GetValidSiteNumber(sites));
            if (siteNumber == 0) return;

            // Prompt the user for a name to book the reservation under
            string name = GetString("Enter the name for the reservation: ");

            // Make the reservation and return to the user a confirmation number or error message if the booking was unsuccesful
            Reservation newReservation = reservationDAO.MakeReservation(siteNumber, campgroundId, name, startDate, endDate);
            if (newReservation == null)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry but there was an error with your reservation, returning to the reservation menu.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"The reservation has been made and the confirmation id is {newReservation.ReservationId}.");
                ObjectListViews.DisplaySingleReservation(newReservation);
                Console.WriteLine();
            }

            return;
        }

        /// <summary>
        /// Helper method to perform a search parkwide
        /// </summary>
        private void ReservationSearchByPark()
        {
            // Display a list of all parks for the user to choose from
            IList<Park> parks = parkDAO.GetAllParks();
            ObjectListViews.DisplayParksSingleLine(parks);

            // Prompt the user to select a valid park 
            int parkId = GetValidInteger("Please choose a park to see all available campsites at that park: ", Validators.GetValidParkIds(parks));

            // Get and display a list of campgrounds at the selected park
            IList<Campground> campgrounds = campgroundDAO.GetCampgroundsByParkId(parkId);

            // Prompt the user to enter an arrival and departure date and store them and then calculate the total number of days of the stay
            DateTime startDate = GetDateTime("Please enter an arrival date: ");
            DateTime endDate = GetDateTimeAfterDate("Please enter a departure date: ", startDate);
            int numDays = (int)((endDate - startDate).TotalDays);

            // Ask the user if they would like to perform an advanced search
            bool isAdvancedSearch = GetBool("Would you like to perform an advanced search (y/n): ");

            // Create a list to hold the results of the search
            IList<Site> sites = new List<Site>();

            // If the user selected to perform an advanced search get additional parameters, then build a list of sites matching search criteria
            if (isAdvancedSearch)
            {
                int maxOccupancyRequired = GetInteger("What is the max occupancy required: ");
                bool isAccessible = GetBool("Do you need a weelchair accessible site (y/n): ");
                int rvSizeRequired = GetInteger("What size RV parking is required (Enter 0 for no RV): ");
                bool isHookupRequired = GetBool("Do you need utility hookups (y/n): ");

                // Build a list of campsites by iterating over all campgrounds at the park and then all sites at that campground using advanced search
                foreach (Campground campground in campgrounds)
                {
                    IList<Site> foundSites = siteDAO.GetAvailableSites(campground.CampgroundId, startDate, endDate, maxOccupancyRequired, isAccessible, rvSizeRequired, isHookupRequired);
                    foreach (Site site in foundSites)
                    {
                        sites.Add(site);
                    }
                }
            }
            else
            {
                // Build a list of campsites by iterating over all campgrounds at the park and then all sites at that campground
                foreach (Campground campground in campgrounds)
                {
                    IList<Site> foundSites = siteDAO.GetAvailableSites(campground.CampgroundId, startDate, endDate);
                    foreach (Site site in foundSites)
                    {
                        sites.Add(site);
                    }
                }
            }

            // Check if the search returned any results and print a message indicating no results or a list showing matching results
            if (sites.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry but no matching results were found for your search, returning to reservation menu.");
                return;
            }
            else
            {
                ObjectListViews.DisplayCampSitesParkwide(sites, campgroundDAO.GetAllCampgrounds(), numDays);
            }

            // Prompt the user to choose a valid site to reserve allow the user to select 0 to cancel the reservation
            int siteId = GetValidInteger("Select a site that you want to reserve BY SITE ID(enter 0 to cancel):", Validators.GetValidSiteId(sites));
            if (siteId == 0) return;

            // Prompt the user for a name to book the reservation under
            string name = GetString("Enter the name for the reservation: ");

            // Make the reservation and return to the user a confirmation number or error message if the booking was unsuccesful
            Reservation newReservation = reservationDAO.MakeReservationBySiteId(siteId, name, startDate, endDate);
            if (newReservation == null)
            {
                Console.WriteLine();
                Console.WriteLine("Sorry but there was an error with your reservation, returning to the reservation menu.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine($"The reservation has been made and the confirmation id is {newReservation.ReservationId}.");
                ObjectListViews.DisplaySingleReservation(newReservation);
                Console.WriteLine();
            }
        }
    }
}

