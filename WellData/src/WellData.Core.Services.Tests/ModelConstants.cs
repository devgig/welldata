using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellData.Core.Services.Models;

namespace WellData.Core.Services.Tests
{
    public static class ModelConstants
    {
        public static TankModel GetTank()
        {
            return new TankModel
            {
                Id = 1065,
                Name = "Sorenson - 247983",
                Number = 1,
                Size = 405.56m,
                BbblsPerInch = 1.67586776859504m,
                SEC = 6,
                TWP = "021N",
                RNG = "058E",
                County = "RICHLAND",
                WellId = "2508321270"
            };
        }

        public static WellModel GetWell()
        {
            return new WellModel
            {
                Id = "2508321270",
                Owner = "Continental Resources, Inc.",
                Longitude = -104.32836m,
                Latitude = 47.60448m,
                Property = 200210,
                LeaseOrWellName = "Sorenson 14-6H"
            };
        }


    }
}
