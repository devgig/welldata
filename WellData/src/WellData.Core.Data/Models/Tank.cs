using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellData.Core.Data.Models
{
    public class Tank
    {
        public int MID { get; set; }

        public string Name { get; set; }

        public int Number { get; set; }

        public decimal Size { get; set; }

        public decimal BbblsPerInch { get; set; }

        public int SEC { get; set; }

        public string TWP { get; set; }

        public string RNG { get; set; }

        public string County { get; set; }
    }
}
