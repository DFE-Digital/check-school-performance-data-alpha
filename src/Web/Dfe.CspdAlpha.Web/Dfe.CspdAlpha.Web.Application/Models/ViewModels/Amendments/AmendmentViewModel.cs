using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.Rscd.Web.ApiClient;

namespace Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments
{
    public class AmendmentViewModel : ContextAwareViewModel
    {
        public Amendment Amendment { get; set; }
        public Keystage Keystage => CheckingWindowHelper.ToKeyStage(CheckingWindow);
    }
}
