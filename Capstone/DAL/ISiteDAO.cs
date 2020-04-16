using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace Capstone.DAL
{
    public interface ISiteDAO
    {
        IList<Site> GetAvailableSites(int campgroundId, DateTime startDate, DateTime endDate, [Optional] int? maxOccupancyRequired, [Optional] bool? isAccessible, [Optional] int? rvSizeRequired, [Optional] bool? isHookupRequired);
    }
}
