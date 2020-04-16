using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        /// <summary>
        /// The National Park id.
        /// </summary>
        public int ParkId { get; set; }

        /// <summary>
        /// The name of the National Park.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The location of the National Park.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The date that the National Park was founded.
        /// </summary>
        public DateTime EstablishDate { get; set; }

        /// <summary>
        /// The size of the park in square kilometers.
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// The annual number of visitors to the park
        /// </summary>
        public int Visitors { get; set; }

        /// <summary>
        /// A short description of the park.
        /// </summary>
        public string Description { get; set; }

    }
}
