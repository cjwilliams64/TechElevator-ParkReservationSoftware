using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {

        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Get a list of all available campsites for a user selected start and end date
        /// includes optional parameters required to perform an advanced search
        /// </summary>
        /// <param name="campgroundId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>A list of Site objects matching the search parameters</returns>          
        public IList<Site> GetAvailableSites(int campgroundId, DateTime startDate, DateTime endDate, [Optional] int? maxOccupancyRequired, [Optional] bool? isAccessible, [Optional] int? rvSizeRequired, [Optional] bool? isHookupRequired)
        {
            // Create a list of sites to build from a sql query
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Open a connection to the database
                    conn.Open();

                    // Create a sql string to perform the search
                    string sql =
@"SELECT TOP 5 * FROM site s
JOIN campground c ON s.campground_id = c.campground_id
WHERE s.campground_id = @campgroundId 
AND s.site_id NOT IN (SELECT site_id FROM reservation WHERE (from_date BETWEEN @startDate AND @endDate OR to_date BETWEEN @startDate AND @endDate))
AND (@startMonth BETWEEN c.open_from_mm AND c.open_to_mm)
AND (@endMonth BETWEEN c.open_from_mm AND c.open_to_mm) ";

                    //Append advanced search optional parameters to the sql string
                    if (maxOccupancyRequired.HasValue)
                    {
                        sql += " AND @maxOccupancyRequired <= s.max_occupancy ";
                    }
                    if (rvSizeRequired.HasValue)
                    {
                        sql += " AND @rvSizeRequired <= s.max_rv_length";
                    }
                    if (isAccessible.HasValue && (bool)isAccessible)
                    {
                        sql += " AND s.accessible = 1 ";
                    }
                    if (isHookupRequired.HasValue && (bool)isHookupRequired)
                    {
                        sql += " AND s.utilities = 1";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@startMonth", startDate.Month);
                    cmd.Parameters.AddWithValue("@endMonth", endDate.Month);

                    // Parameterizing the data for advanced search functions, check if data exists before adding parameters
                    if (maxOccupancyRequired.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@maxOccupancyRequired", maxOccupancyRequired);
                    }
                    if (rvSizeRequired.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@rvSizeRequired", rvSizeRequired);
                    }

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        sites.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sites;
        }

        /// <summary>
        /// Helper Method to convert SQL row data to a Site object
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        private static Site RowToObject(SqlDataReader rdr)
        {
            Site site = new Site();

            site.SiteId = Convert.ToInt32(rdr["site_id"]);
            site.CampgroundId = Convert.ToInt32(rdr["campground_id"]);
            site.SiteNumber = Convert.ToInt32(rdr["site_number"]);
            site.MaxOccupancy = Convert.ToInt32(rdr["max_occupancy"]);
            site.MaxRVLength = Convert.ToInt32(rdr["max_rv_length"]);

            site.Accessible = Convert.ToBoolean(rdr["accessible"]);
            site.Utilities = Convert.ToBoolean(rdr["utilities"]);

            return site;
        }
    }
}

