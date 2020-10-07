using Dfe.CspdAlpha.Web.Application.Application.Extensions;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.Amendments.AmendmentTypes;
using Dfe.CspdAlpha.Web.Application.Models.Common;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.RemovePupil;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class RemovePupilController : Controller
    {
        private IPupilService _pupilService;
        private IAmendmentService _amendmentService;
        private CheckingWindow CheckingWindow => CheckingWindowHelper.GetCheckingWindow(RouteData);

        public RemovePupilController(IPupilService pupilService, IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
            _pupilService = pupilService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SearchPupilsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Results", new { viewModel.SearchType, Query = viewModel.PupilID ?? viewModel.Name });
            }
            return View(viewModel);
        }

        public IActionResult Results(SearchQuery viewModel)
        {
            var pupilsFound = _pupilService.GetPupilDetailsList(CheckingWindow, viewModel);
            if (pupilsFound.Count == 0 || pupilsFound.Count > 1)
            {
                return View( new ResultsViewModel
                {
                    PupilList = pupilsFound
                });
            }
            return RedirectToAction("MatchedPupil", new { id = pupilsFound.First().ID });
        }

        [HttpPost]
        public IActionResult Results(ResultsViewModel viewModel, string urn)
        {
            if (ModelState.IsValid)
            {
                SavePupilToSession(viewModel.SelectedID, urn);
                return RedirectToAction("Reason");
            }
            viewModel.PupilList = _pupilService.GetPupilDetailsList(CheckingWindow, new SearchQuery { Query = viewModel.Query, URN = viewModel.URN, SearchType = viewModel.SearchType});

            return View(viewModel);
        }

        private MatchedPupilViewModel SavePupilToSession(string id, string urn)
        {
            var viewModel = _pupilService.GetPupil(CheckingWindow, id);
            var removePupilAmendment = new Amendment
            {
                URN = urn,
                CheckingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData),
                AmendmentDetail = new RemovePupil
                {
                    PupilDetails = new PupilDetails
                    {
                        ID = id,
                        UPN = viewModel.PupilViewModel.UPN,
                        FirstName = viewModel.PupilViewModel.FirstName,
                        LastName = viewModel.PupilViewModel.LastName,
                        DateOfBirth = viewModel.PupilViewModel.DateOfBirth,
                        Age = viewModel.PupilViewModel.Age,
                        Gender = viewModel.PupilViewModel.Gender,
                        DateOfAdmission = viewModel.PupilViewModel.DateOfAdmission,
                        YearGroup = viewModel.PupilViewModel.YearGroup
                    }
                }
            };
            HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, removePupilAmendment);
            return viewModel;
        }

        public IActionResult MatchedPupil(string id, string urn)
        {
            var viewModel = SavePupilToSession(id, urn);
            return View(viewModel);
        }

        public IActionResult Reason()
        {
            var removePupilAmendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            return View(new ReasonViewModel{PupilDetails = removePupilAmendment.AmendmentDetail.PupilDetails, Reasons = _amendmentService.GetRemoveReasons()});
        }

        [HttpPost]
        public IActionResult Reason(ReasonViewModel viewModel)
        {
            var removePupilAmendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (ModelState.IsValid)
            {
                var amendmentDetail = (RemovePupil)removePupilAmendment.AmendmentDetail;
                amendmentDetail.Reason = viewModel.SelectedReason;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, removePupilAmendment);
                if (new[] {"329", "330"}.Any(r => r == viewModel.SelectedReason))
                {
                    return RedirectToAction("SubReason");
                }

                if (viewModel.SelectedReason == "325")
                {
                    return RedirectToAction("Details");
                }
            }
            return View(new ReasonViewModel { PupilDetails = removePupilAmendment.AmendmentDetail.PupilDetails, Reasons = _amendmentService.GetRemoveReasons() });
        }

        public IActionResult SubReason()
        {
            var removePupilAmendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (RemovePupil)removePupilAmendment.AmendmentDetail;
            return View(new SubReasonViewModel { PupilDetails = removePupilAmendment.AmendmentDetail.PupilDetails, Reasons = _amendmentService.GetRemoveReasons(amendmentDetail.Reason) });
        }

        [HttpPost]
        public IActionResult SubReason(SubReasonViewModel viewModel)
        {
            var removePupilAmendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            var amendmentDetail = (RemovePupil)removePupilAmendment.AmendmentDetail;
            if (ModelState.IsValid)
            {
                amendmentDetail.SubReason = viewModel.SelectedReason;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, removePupilAmendment);
            }
            return View(new SubReasonViewModel { PupilDetails = removePupilAmendment.AmendmentDetail.PupilDetails, Reasons = _amendmentService.GetRemoveReasons(amendmentDetail.Reason) });
        }

        public IActionResult Details()
        {
            var removePupilAmendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            return View(new DetailsViewModel {PupilDetails = removePupilAmendment.AmendmentDetail.PupilDetails});
        }

        [HttpPost]
        public IActionResult Details(DetailsViewModel viewModel)
        {
            var removePupilAmendment = HttpContext.Session.Get<Amendment>(Constants.AMENDMENT_SESSION_KEY);
            if (ModelState.IsValid)
            {
                var amendmentDetail = (RemovePupil)removePupilAmendment.AmendmentDetail;
                amendmentDetail.Detail = viewModel.AmendmentDetails;
                HttpContext.Session.Set(Constants.AMENDMENT_SESSION_KEY, removePupilAmendment);
                return RedirectToAction("Confirm","Amendments");
            }

            viewModel.PupilDetails = removePupilAmendment.AmendmentDetail.PupilDetails;
            return View(viewModel);
        }
    }
}
