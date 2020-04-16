using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlDAOTests
    {
        private TransactionScope transaction = null;

        private string connectionString = "Server=.\\SqlExpress;Database=npcampground;Trusted_Connection=True;";
        private int newReservationId;
        private int newSiteId;
        private int newCampgroundId;


        [TestInitialize]
        public void SetupDatabase()
        {
            //Start a transaction so we can rollback when we are finished with this test
            transaction = new TransactionScope();

            //Open Setup.sql and read in the script to be executed 
            string setupSQL;
            using (StreamReader rdr = new StreamReader("Setup.sql"))
            {
                setupSQL = rdr.ReadToEnd();
            }

            //Connect to the DB and execute the script
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(setupSQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    newReservationId = Convert.ToInt32(rdr["newReservationId"]);
                    newSiteId = Convert.ToInt32(rdr["newSiteId"]);
                    newCampgroundId = Convert.ToInt32(rdr["newCampgroundId"]);
                }
            }
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            //Rollback the transaction to get our good data back
            transaction.Dispose();
        }

        [TestMethod]
        public void TestMakeReservation()
        {
            //Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(connectionString);

            //Act
            Reservation reservation = dao.MakeReservation(10, newCampgroundId, "Fries Island", Convert.ToDateTime("01/05/2020"), Convert.ToDateTime("03/05/2020"));
         

            //Assert 
           
            Assert.AreEqual("Fries Island", reservation.Name);
            Assert.AreEqual(Convert.ToDateTime("01/05/2020"), reservation.StartDate);
        }


        [TestMethod]
        public void TestMakeReservationBySiteId()
        {
            //Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(connectionString);

            //Act
            Reservation reservation = dao.MakeReservationBySiteId(newSiteId, "Williams View", Convert.ToDateTime("01/24/1995"), Convert.ToDateTime("02/01/1995"));


            //Assert 
            Assert.AreEqual(Convert.ToDateTime("02/01/1995"), reservation.EndDate);
            Assert.AreEqual("Williams View", reservation.Name);
        }


        [TestMethod]
        public void TestViewAllUpcomingReservations()
        {
            //Arrange
            ReservationSqlDAO dao = new ReservationSqlDAO(connectionString);
            Reservation reservation = dao.MakeReservation(10, newCampgroundId, "Fries Island", Convert.ToDateTime("02/29/2020"), Convert.ToDateTime("03/01/2020"));

            //Act
            IList<Reservation> reservations = dao.ViewAllUpcomingReservations(DateTime.Now, Convert.ToDateTime("03/01/2020"));
            //int i = 0;
            //for (; i < reservations.Count; i++)
            //{
            //    if (reservations[i].ReservationId == newReservationId)
            //    {
            //        break;
            //    }
            //}

            //Assert 
         
            Assert.AreEqual(Convert.ToDateTime("03/01/2020"), reservations[reservations.Count -1].EndDate);
        }



    }
}
