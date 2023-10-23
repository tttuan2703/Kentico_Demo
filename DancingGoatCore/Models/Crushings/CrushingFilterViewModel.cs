using CMS.DataEngine;
using CMS.Helpers;
using DancingGoat.Models.Crushings;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DancingGoat.Models.Crushings
{
    public class CrushingFilterViewModel : IRepositoryFilter
    {
        [Display(Name = "Decaf")]
        public bool OnlyDecaf { get; set; }


        [UIHint("CrushingProductFilter")]
        public CrushingProductFilterCheckboxViewModel[] ProcessingTypes { get; set; }


        public CrushingFilterViewModel()
        {
        }


        public void Load(IStringLocalizer localizer)
        {
            ProcessingTypes = GetCrushingProcessingTypes()
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
                whereCondition.WhereTrue("CrushingIsDecaf");
            }
            return whereCondition;
        }


        private WhereCondition GetProcessingTypesWhere()
        {
            var whereCondition = new WhereCondition();
            var selectedTypes = GetSelectedTypes();

            if (selectedTypes.Any())
            {
                whereCondition.WhereIn("CrushingProcessing", selectedTypes);
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


        private IEnumerable<string> GetCrushingProcessingTypes()
        {
            return new List<string> { "Washed", "Semiwashed", "Natural" };
        }


        private CrushingProductFilterCheckboxViewModel GetProductFilterCheckboxViewModel(string processingType, IStringLocalizer localizer)
        {
            return new CrushingProductFilterCheckboxViewModel
            {
                DisplayName = localizer[processingType],
                Value = processingType
            };
        }
    }
}
