using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {

        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Method to create a new reservation in the database using site number
        /// </summary>
        /// <param name="siteNumber"></param>
        /// <param name="campgroundId"></param>
        /// <param name="name"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>A reservation object holding information about the created reservation</returns>
        public Reservation MakeReservation(int siteNumber, int campgroundId, string name, DateTime startDate, DateTime endDate)
        {
            Reservation reservation = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    string sql =
@"INSERT INTO reservation (site_id, name, from_date, to_date, create_date)
Values ((SELECT site_id FROM site WHERE site_number = @siteNumber AND campground_id = @campgroundId), @name, @fromDate, @toDate, @createDate)
Select @@identity;";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@siteNumber", siteNumber);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);

                    int newReservationId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Run a second query to return a row matching the reservation id in order to create a reservation object to return
                    sql = "SELECT * FROM reservation WHERE reservation_id = @newReservationId";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@newReservationId", newReservationId);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        reservation = RowToObject(rdr);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reservation;
        }

        /// <summary>
        /// Method to create a new reservation in the database using site Id
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="name"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>A reservation object holding information about the created reservation</returns>
        public Reservation MakeReservationBySiteId(int siteId, string name, DateTime startDate, DateTime endDate)
        {
            Reservation reservation = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    string sql =
@"INSERT INTO reservation (site_id, name, from_date, to_date, create_date)
Values (@siteId, @name, @fromDate, @toDate, @createDate)
Select @@identity;";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@siteId", siteId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);

                    int newReservationId = Convert.ToInt32(cmd.ExecuteScalar());

                    // Run a second query to return a row matching the reservation id in order to create a reservation object to return
                    sql = "SELECT * FROM reservation WHERE reservation_id = @newReservationId";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@newReservationId", newReservationId);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        reservation = RowToObject(rdr);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reservation;
        }

        /// <summary>
        /// Method to get all reservations for a specified date range
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>A list of reservation objects matching the parameters</returns>
        public IList<Reservation> ViewAllUpcomingReservations(DateTime startDate, DateTime endDate)
        {
            IList<Reservation> reservations = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    string sql =
@"SELECT * FROM reservation WHERE from_date BETWEEN @startDate AND @endDate";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        reservations.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return reservations;
        }

        /// <summary>
        /// Helper Method to convert SQL row data to a Reservation object
        /// </summary>
        /// <param name="rdr"></param>
        /// <returns></returns>
        private static Reservation RowToObject(SqlDataReader rdr)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(rdr["reservation_id"]);
            reservation.SiteId = Convert.ToInt32(rdr["site_id"]);

            reservation.Name = Convert.ToString(rdr["name"]);

            reservation.StartDate = Convert.ToDateTime(rdr["from_date"]);
            reservation.EndDate = Convert.ToDateTime(rdr["to_date"]);
            reservation.BookingDate = Convert.ToDateTime(rdr["create_date"]);

            return reservation;
        }
    }
}
