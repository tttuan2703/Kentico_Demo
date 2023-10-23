using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models.Dozers
{
    public class DozerViewModel : ITypedProductViewModel
    {
        public int Altitude { get; set; }

        public string Country { get; set; }

        public string Farm { get; set; }

        public bool IsDecaf { get; set; }

        public string Processing { get; set; }

        public string Variety { get; set; }

        public string Horsepower { get; set; }

        public string OperatingWeight { get; set; }

        public string BladeCapacity { get; set; }

        public static DozerViewModel GetViewModel(Dozer dozerViewModel)
        {
            return new DozerViewModel
            {
                Altitude = dozerViewModel.Fields.Altitude,
                Country = dozerViewModel.Fields.FactoryCountry,
                Farm = dozerViewModel.Fields.FactoryName,
                IsDecaf = dozerViewModel.Fields.IsDecaf,
                Processing = dozerViewModel.Fields.Processing,
                Variety = dozerViewModel.Fields.Variety,
                BladeCapacity = dozerViewModel.BladeCapacity,
                Horsepower = dozerViewModel.Horsepower,
                OperatingWeight = dozerViewModel.OperatingWeight
            };
        }
    }
}