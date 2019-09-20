namespace WellData.Core.Services.Models
{
    public class WellModel : SimpleModel
    {
        private string owner;
        private decimal longitude;
        private decimal latitude;
        private int property;
        private string leaseOrWellName;

        public WellModel() : base(typeof(WellModel))
        {
        }

        public string Owner
        {
            get => owner; set
            {
                owner = value;
                OnPropertyChanged();
            }
        }

        public double Id { get; set; }

        public decimal Longitude
        {
            get => longitude; set
            {
                longitude = value;
                OnPropertyChanged();
            }
        }

        public decimal Latitude
        {
            get => latitude; set
            {
                latitude = value;
                OnPropertyChanged();
            }
        }

        public int Property
        {
            get => property; set
            {
                property = value;
                OnPropertyChanged();
            }
        }

        public string LeaseOrWellName
        {
            get => leaseOrWellName; set
            {
                leaseOrWellName = value;
                OnPropertyChanged();
            }
        }

    }
}
