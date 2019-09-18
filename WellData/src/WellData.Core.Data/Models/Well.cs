using System.Collections.Generic;

namespace WellData.Core.Data.Models
{
    public class Well
    {
        public string Owner { get; set; }

        public int API { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Property { get; set; }

        public string LeaseOrWellName { get; set; }

        public IList<Tank> Tanks { get; set; }


    }
}
