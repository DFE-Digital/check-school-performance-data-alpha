using Dfe.CspdAlpha.Web.Application.Models.Common;
using System.Collections.Generic;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil
{
    public class UploadEvidenceViewModel
    {
        public string Id { get; set; }
        public PupilViewModel PupilViewModel { get; set; }
        public List<EvidenceFile> EvidenceFiles { get; set; }
    }
}
