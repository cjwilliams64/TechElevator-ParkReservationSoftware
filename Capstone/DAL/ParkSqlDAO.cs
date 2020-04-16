using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the Parks.
        /// </summary>
        /// <returns>A list of all Parks.</returns>
        public IList<Park> GetAllParks()
        {
            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM park";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        parks.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return parks;
        }

        /// <summary>
        /// Helper Method to convert SQL row data to a Park object
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        private static Park RowToObject(SqlDataReader rdr)
        {
            Park park = new Park
            {
                ParkId = Convert.ToInt32(rdr["park_id"]),
                Area = Convert.ToInt32(rdr["area"]),
                Visitors = Convert.ToInt32(rdr["visitors"]),

                Name = Convert.ToString(rdr["name"]),
                Location = Convert.ToString(rdr["location"]),
                Description = Convert.ToString(rdr["description"]),

                EstablishDate = Convert.ToDateTime(rdr["establish_date"])
            };

            return park;
        }
    }
}

