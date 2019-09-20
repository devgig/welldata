namespace WellData.Core.Services.Models
{
    public class TankModel : SimpleModel
    {
        private string name;
        private int number;
        private decimal size;
        private decimal bbblsPerInch;
        private int sEC;
        private string tWP;
        private string rNG;
        private string county;

        public int Id { get; set; }

        public string Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public int Number
        {
            get => number; set
            {
                number = value;
                OnPropertyChanged();
            }
        }

        public decimal Size
        {
            get => size; set
            {
                size = value;
                OnPropertyChanged();
            }
        }

        public decimal BbblsPerInch
        {
            get => bbblsPerInch; set
            {
                bbblsPerInch = value;
                OnPropertyChanged();
            }
        }

        public int SEC
        {
            get => sEC; set
            {
                sEC = value;
                OnPropertyChanged();
            }
        }

        public string TWP
        {
            get => tWP; set
            {
                tWP = value;
                OnPropertyChanged();
            }
        }

        public string RNG
        {
            get => rNG; set
            {
                rNG = value;
                OnPropertyChanged();
            }
        }

        public string County
        {
            get => county; set
            {
                county = value;
                OnPropertyChanged();
            }
        }

        public double WellId { get; set; }
    }
}
