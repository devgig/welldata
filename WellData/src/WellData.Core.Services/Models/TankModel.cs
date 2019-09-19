using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellData.Core.Services.Models
{
    public class TankModel 
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public decimal Size { get; set; }

        public decimal BbblsPerInch { get; set; }

        public int SEC { get; set; }

        public string TWP { get; set; }

        public string RNG { get; set; }

        public string County { get; set; }

        public double WellId { get; set; }
    }
}
