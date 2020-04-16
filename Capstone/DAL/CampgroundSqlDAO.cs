using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        // Single Parameter Constructor requires a connection string
        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all campgrounds.
        /// </summary>
        /// <returns>A list of all campgrounds.</returns>
        public IList<Campground> GetAllCampgrounds()
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM campground";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        campgrounds.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return campgrounds;
        }

        /// <summary>
        /// Returns a list of all campgrounds for a supplied parkId
        /// </summary>
        /// <param name="parkId"></param>
        /// <returns>A list of matching campgrounds.</returns>
        public IList<Campground> GetCampgroundsByParkId(int parkId)
        {
            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM campground WHERE park_id = @parkId";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        campgrounds.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return campgrounds;
        }

        /// <summary>
        /// Helper Method to convert SQL row data to a Campground object
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        private static Campground RowToObject(SqlDataReader rdr)
        {
            return new Campground
            {
                CampgroundId = Convert.ToInt32(rdr["campground_id"]),
                ParkId = Convert.ToInt32(rdr["park_id"]),
                OpenFromMonth = Convert.ToInt32(rdr["open_from_mm"]),
                OpenToMonth = Convert.ToInt32(rdr["open_to_mm"]),

                Name = Convert.ToString(rdr["name"]),

                DailyFee = Convert.ToDecimal(rdr["daily_fee"])
            };
        }
    }
}
