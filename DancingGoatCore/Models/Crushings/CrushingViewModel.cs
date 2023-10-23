using CMS.DocumentEngine.Types.DancingGoatCore;

namespace DancingGoat.Models.Crushings
{
    public class CrushingViewModel : ITypedProductViewModel
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

        public static CrushingViewModel GetViewModel(Crushing crushingViewModel)
        {
            return new CrushingViewModel
            {
                Altitude = crushingViewModel.Fields.Altitude,
                Country = crushingViewModel.Fields.FactoryCountry,
                Farm = crushingViewModel.Fields.FactoryName,
                IsDecaf = crushingViewModel.Fields.IsDecaf,
                Processing = crushingViewModel.Fields.Processing,
                Variety = crushingViewModel.Fields.Variety,
                BladeCapacity = crushingViewModel.BladeCapacity,
                Horsepower = crushingViewModel.Horsepower,
                OperatingWeight = crushingViewModel.OperatingWeight
            };
        }
    }
}