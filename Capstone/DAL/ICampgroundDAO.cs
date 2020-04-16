using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{

    public interface ICampgroundDAO
    {
        IList<Campground> GetAllCampgrounds();

        IList<Campground> GetCampgroundsByParkId(int parkId);
    }
}
