using CMS.DocumentEngine.Types.DancingGoatCore;
using DancingGoat.Models.Excavators;

namespace DancingGoat.Models.Excavators
{
    public class ExcavatorViewModel : ITypedProductViewModel
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

        public static ExcavatorViewModel GetViewModel(Excavator excavatorViewModel)
        {
            return new ExcavatorViewModel
            {
                Altitude = excavatorViewModel.Fields.Altitude,
                Country = excavatorViewModel.Fields.FactoryCountry,
                Farm = excavatorViewModel.Fields.FactoryName,
                IsDecaf = excavatorViewModel.Fields.IsDecaf,
                Processing = excavatorViewModel.Fields.Processing,
                Variety = excavatorViewModel.Fields.Variety,
                BladeCapacity = excavatorViewModel.BladeCapacity,
                Horsepower = excavatorViewModel.Horsepower,
                OperatingWeight = excavatorViewModel.OperatingWeight
            };
        }
    }
}