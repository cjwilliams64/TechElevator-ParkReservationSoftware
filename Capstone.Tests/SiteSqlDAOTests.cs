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
    public class SiteSqlDAOTests
    {
        private TransactionScope transaction = null;

        private string connectionString = "Server=.\\SqlExpress;Database=npcampground;Trusted_Connection=True;";
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
                while (rdr.Read())
                {
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
        public void TestGetAvailableSites()
        {
            //Arrange
            SiteSqlDAO dao = new SiteSqlDAO(connectionString);

            //Act
            IList<Site> sites = dao.GetAvailableSites(newCampgroundId, Convert.ToDateTime("05/05/2020"), Convert.ToDateTime("05/19/2020"));
            int i = 0;
            for (; i < sites.Count; i++)
            {
                if (sites[i].SiteId == newSiteId)
                {
                    break;
                }
            }

            //Assert 
            Assert.AreEqual(1, sites.Count);
            Assert.AreEqual(4, sites[i].MaxOccupancy);

        }

        [TestMethod]
        public void TestGetAvailableSitesAdvancedSearchNoMatches()
        {
            //Arrange
            SiteSqlDAO dao = new SiteSqlDAO(connectionString);

            //Act
            IList<Site> sites = dao.GetAvailableSites(newCampgroundId, Convert.ToDateTime("01/01/2020"), Convert.ToDateTime("05/19/2020"), 4, false, 22, true);

            //Assert 
            Assert.AreEqual(0, sites.Count);
          
        }

        [TestMethod]
        public void TestGetAvailableSitesAdvancedSearchWithMatches()
        {
            //Arrange
            SiteSqlDAO dao = new SiteSqlDAO(connectionString);

            //Act
            IList<Site> sites = dao.GetAvailableSites(newCampgroundId, Convert.ToDateTime("06/13/2020"), Convert.ToDateTime("06/19/2020"), 3, false, 0, true);
            int i = 0;
            for (; i < sites.Count; i++)
            {
                if (sites[i].SiteId == newSiteId)
                {
                    break;
                }
            }

            //Assert 
            Assert.AreEqual(1, sites.Count);
            Assert.AreEqual(10, sites[i].SiteNumber);

        }
    }
}
