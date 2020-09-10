using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentsListViewModel
    {
        public CheckDataNavigation CheckDataNavigationModel => new CheckDataNavigation(NavigationItem.Amendments , LateCheckingPhase);
        public string Urn { get; set; }
        public List<Amendment> AmendmentList { get; set; }
        public bool LateCheckingPhase { get; set; }
        public string AmendmentListJson => JsonConvert.SerializeObject(AmendmentList);
    }
}
