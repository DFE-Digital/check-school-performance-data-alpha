using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class MatchedPupilViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
    }
}