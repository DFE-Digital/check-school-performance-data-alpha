using System.Collections.Generic;
using Dfe.CspdAlpha.Web.Application.Models.Common;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil
{
    public class ReasonViewModel
    {
        public PupilDetails PupilDetails { get; set; }
        public string SelectedReason { get; set; }
        public Dictionary<string, string> Reasons  { get; set; }
    }
}