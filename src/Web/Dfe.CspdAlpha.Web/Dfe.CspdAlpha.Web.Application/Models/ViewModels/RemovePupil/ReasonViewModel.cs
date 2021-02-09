using System.Collections.Generic;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.Common;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.RemovePupil
{
    public class ReasonViewModel : ContextAwareViewModel
    {
        public QueryType SearchType { get; set; }

        public string Query { get; set; }

        public string MatchedId { get; set; }

        public PupilViewModel PupilDetails { get; set; }

        public int? SelectedReasonCode { get; set; }

        public List<AmendmentReason> Reasons = new List<AmendmentReason>();

        public Dictionary<int, string> ReasonsKs5  => new Dictionary<int, string>
        {
            { Constants.NOT_AT_END_OF_16_TO_18_STUDY, "Not at the end of 16 to 18 study" },
            { Constants.INTERNATIONAL_STUDENT, "International student" },
            { Constants.DECEASED, "Deceased" },
            { Constants.NOT_ON_ROLL, "Not on roll" },
            { Constants.OTHER_WITH_EVIDENCE, "Other with evidence" },
            { Constants.OTHER_EVIDENCE_NOT_REQUIRED, "Other without evidence" },
        };
    }
}
