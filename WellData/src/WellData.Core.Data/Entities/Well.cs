using System;
using System.Collections.Generic;

namespace WellData.Core.Data.Entities
{
    public class Well
    {
        public string Owner { get; set; }

        public double Id { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Property { get; set; }

        public string LeaseOrWellName { get; set; }

        public virtual List<Tank> Tanks { get; set; }

    }
}
