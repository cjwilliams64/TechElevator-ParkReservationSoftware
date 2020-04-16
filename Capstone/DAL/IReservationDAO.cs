using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IReservationDAO
    {
        Reservation MakeReservation(int siteNumber, int campgroundId, string name, DateTime startDate, DateTime endDate);

        Reservation MakeReservationBySiteId(int siteId, string name, DateTime startDate, DateTime endDate);

        IList<Reservation> ViewAllUpcomingReservations(DateTime startDate, DateTime endDate);
    }
}
