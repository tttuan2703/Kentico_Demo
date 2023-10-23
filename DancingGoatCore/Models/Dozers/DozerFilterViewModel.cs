using CMS.DataEngine;
using CMS.Helpers;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DancingGoat.Models.Dozers
{
    public class DozerFilterViewModel : IRepositoryFilter
    {
        [Display(Name = "Decaf")]
        public bool OnlyDecaf { get; set; }


        [UIHint("DozerProductFilter")]
        public DozerProductFilterCheckboxViewModel[] ProcessingTypes { get; set; }


        public DozerFilterViewModel()
        {
        }


        public void Load(IStringLocalizer localizer)
        {
            ProcessingTypes = GetDozerProcessingTypes()
               .Select(c => GetProductFilterCheckboxViewModel(c, localizer))
               .ToArray();
        }


        public WhereCondition GetWhereCondition()
        {
            var decafWhereCondition = GetDecafWhereCondition();
            var processingTypesWhere = GetProcessingTypesWhere();

            return decafWhereCondition.And(processingTypesWhere);
        }


        public string GetCacheKey()
        {
            var serializedProcessingTypes = ProcessingTypes
               .Select(type => string.Format($"{type.Value}:{type.IsChecked}"))
               .Join(TextHelper.NewLine);

            return string.Format($"OnlyDecaf:{OnlyDecaf}{TextHelper.NewLine}{serializedProcessingTypes}");
        }


        private WhereCondition GetDecafWhereCondition()
        {
            var whereCondition = new WhereCondition();

            if (OnlyDecaf)
            {
                whereCondition.WhereTrue("DozerIsDecaf");
            }
            return whereCondition;
        }


        private WhereCondition GetProcessingTypesWhere()
        {
            var whereCondition = new WhereCondition();
            var selectedTypes = GetSelectedTypes();

            if (selectedTypes.Any())
            {
                whereCondition.WhereIn("DozerProcessing", selectedTypes);
            }

            return whereCondition;

        }


        private List<string> GetSelectedTypes()
        {
            return ProcessingTypes
                .Where(x => x.IsChecked)
                .Select(x => x.Value)
                .ToList();
        }


        private IEnumerable<string> GetDozerProcessingTypes()
        {
            return new List<string> { "Washed", "Semiwashed", "Natural" };
        }


        private DozerProductFilterCheckboxViewModel GetProductFilterCheckboxViewModel(string processingType, IStringLocalizer localizer)
        {
            return new DozerProductFilterCheckboxViewModel
            {
                DisplayName = localizer[processingType],
                Value = processingType
            };
        }
    }
}