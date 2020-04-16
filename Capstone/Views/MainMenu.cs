using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class MainMenu : CLIMenu
    {
        // DAOs - Interfaces to our data objects can be stored here...
        protected IParkDAO parkDAO;
        protected ICampgroundDAO campgroundDAO;
        protected ISiteDAO siteDAO;
        protected IReservationDAO reservationDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu.
        /// </summary>
        public MainMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO) : base("Main Menu")
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "List National Parks");
            this.menuOptions.Add("2", "List Campgrounds at a National Park");
            this.menuOptions.Add("3", "Make a reservation");
            this.menuOptions.Add("Q", "Quit program");
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
                case "1": // Display all parks with summary information
                    ObjectListViews.DisplayParksDetailedView(parkDAO.GetAllParks());
                    Pause("");
                    return true;
                case "2": // Display all the campgrounds at a selected national park by calling DisplayCampgroundByPark method
                    DisplayCampgroundsByPark();
                    Pause("");
                    return true;
                case "3": // Create and show the reservation sub-menu
                    ReservationMenu rm = new ReservationMenu(parkDAO, campgroundDAO, siteDAO, reservationDAO);
                    rm.Run();
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
            SetColor(ConsoleColor.DarkGreen);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("National Parks"));
            ResetColor();
        }

        /// <summary>
        /// Helper Method to execute the option to display campgrounds at a specific national park
        /// </summary>
        private void DisplayCampgroundsByPark()
        {
            // Get a List of all national parks and display this to the user
            IList<Park> parks = parkDAO.GetAllParks();
            ObjectListViews.DisplayParksSingleLine(parks);

            // Get a list of valid selections given the results of the previous query
            List<int> validSelections = Validators.GetValidParkIds(parks);

            // Prompt the user to enter a selection and validate that this selection exists in the list
            int parkId = GetValidInteger("Please select a national park by Id to display the campgrounds at that park:", validSelections);

            // Display the campgrounds at the selected national park
            ObjectListViews.DisplayCampgrounds(campgroundDAO.GetCampgroundsByParkId(parkId));
        }
    }
}
