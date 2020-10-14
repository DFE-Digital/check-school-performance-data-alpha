using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.Amendments
{
    public class Amendment : IAmendment
    {
        public string URN { get; set; }
        public CheckingWindow CheckingWindow { get; set; }
        public IAmendmentType AmendmentDetail { get; set; }
        public PupilDetails PupilDetails { get; set; }
        public EvidenceOption EvidenceOption { get; set; }
        public string EvidenceFolderName { get; set; }
    }
}
