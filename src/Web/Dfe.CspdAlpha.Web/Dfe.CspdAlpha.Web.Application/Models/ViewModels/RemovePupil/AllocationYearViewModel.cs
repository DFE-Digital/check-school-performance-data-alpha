using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class AllocationYearViewModel
    {
        public readonly Dictionary<string, string> AllocationYears = new Dictionary<string, string>();

        public AllocationYearViewModel()
        {
            // Required to allow constructor-less creation as controller action parameter
        }

        public AllocationYearViewModel(string allocationYear)
        {

            BuildAllocationYears(allocationYear);
        }

        private void BuildAllocationYears(string allocationYear)
        {
            var year = int.Parse(allocationYear);
            var previous = year - 1;
            for (var i = 0; i < 3; i++)
            {
                AllocationYears.Add($"{previous.ToString()}-{year.ToString().Substring(2)}", $"{previous.ToString()}/{year.ToString().Substring(2)}");
                year = previous--;
            }
        }

        public PupilDetails PupilDetails { get; set; }
        public string AllocationYear { get; set; }
    }
}