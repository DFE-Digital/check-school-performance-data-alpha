using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using System;
using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Results;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentViewModel
    {
        public PupilViewModel PupilViewModel { get; set; }
        public List<PriorAttainmentResultViewModel> Results { get; set; }
    }
}
