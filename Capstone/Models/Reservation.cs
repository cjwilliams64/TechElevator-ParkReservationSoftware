using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        /// <summary>
        /// The reservation Id
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// Site Id that corresponds to the chosen site
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Name the reservation is held under
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start Date of the reservation
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End Date of the reservation
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Date the reservation was booked
        /// </summary>
        public DateTime BookingDate { get; set; }

    }
}
