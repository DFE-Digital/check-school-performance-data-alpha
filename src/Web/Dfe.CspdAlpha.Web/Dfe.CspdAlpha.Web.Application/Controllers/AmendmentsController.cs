using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Helpers;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Amendments;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels.Pupil;
using Dfe.Rscd.Web.ApiClient;
using Microsoft.AspNetCore.Mvc;
using ProblemDetails = Dfe.Rscd.Web.ApiClient.ProblemDetails;

namespace Dfe.CspdAlpha.Web.Application.Controllers
{
    public class AmendmentsController : SessionController
    {
        private const string NotshowedYet = "NotShowed";
        private readonly IAmendmentService _amendmentService;

        public AmendmentsController(IAmendmentService amendmentService)
        {
            _amendmentService = amendmentService;
        }

        public IActionResult Index(string urn)
        {
            var checkingWindow = CheckingWindowHelper.GetCheckingWindow(RouteData);
            var amendments = _amendmentService.GetAmendmentsListViewModel(urn);

            return View(amendments);
        }

        public IActionResult Cancel(string id)
        {
            if (_amendmentService.CancelAmendment(id)) return RedirectToAction("Index");

            return RedirectToAction("Error", "Home");
        }

        public IActionResult Clear()
        {
            ClearAmendmentAndRelated();
            return RedirectToAction("Index", "TaskList");
        }

        [ActionName("View")]
        public IActionResult ViewAmendment(string id)
        {
            var amendment = _amendmentService.GetAmendment(id);
            if (amendment != null) return View(new AmendmentViewModel {Amendment = amendment});
            return RedirectToAction("Error", "Home");
        }

        public IActionResult Confirm()
        {
            var amendment = GetAmendment();
            return View(GetConfirmViewModel(amendment));
        }

        private ConfirmViewModel GetConfirmViewModel(Amendment amendment)
        {
            var viewModel = new ConfirmViewModel
            {
                AmendmentType = amendment.AmendmentType,
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            viewModel.BackAction = "Index";
            viewModel.BackController = "Reason";

            return viewModel;
        }

        [HttpPost]
        public IActionResult Confirm(ConfirmViewModel viewModel)
        {
            var amendment = GetAmendment();

            // Ensure steps haven't been manually skipped
            if (amendment == null) return RedirectToAction("Index", "TaskList");

            // Cancel amendment
            if (!viewModel.ConfirmAmendment)
            {
                // Cancel amendment
                ClearAmendmentAndRelated();
                return RedirectToAction("Index", "TaskList");
            }

            try
            {
                amendment.IsUserConfirmed = true;
                var amendmentOutcome = _amendmentService.CreateAmendment(amendment);
                // Create amendment and redirect to amendment received page

                if (amendmentOutcome.IsComplete && amendmentOutcome.IsAmendmentCreated)
                {
                    ClearAmendmentAndRelated();

                    var receivedViewModel = new ReceivedViewModel
                    {
                        NewAmendmentId = amendmentOutcome.NewAmendmentId,
                        NewAmendmentRef = amendmentOutcome.NewAmendmentReferenceNumber
                    };

                    return RedirectToAction("Received", receivedViewModel);
                }

                return RedirectToAction("Prompt");
            }
            catch (ApiException<ProblemDetails> apiException)
            {
                var properties = apiException.Result.AdditionalProperties;
                dynamic titleContent = properties.Values.Last();
                return View("CustomMessage", new CustomMessageViewModel
                {
                    Description = properties.Values.First().ToString(),
                    Title = titleContent.errorMessage.ToString(),
                    PupilDetails = new PupilViewModel(amendment.Pupil)
                });
            }
        }

        [HttpPost]
        public IActionResult CustomMessage()
        {
            return RedirectToAction("Index", "TaskList");
        }

        public IActionResult Received(ReceivedViewModel viewModel)
        {
            return View("Received", viewModel);
        }

        [HttpPost]
        public IActionResult Prompt(PromptAnswerViewModel promptAnswerViewModel)
        {
            var questions = GetQuestions();
            var promptAnswer = promptAnswerViewModel.GetAnswerAsString(Request.Form);

            SaveAnswer(new UserAnswer{ QuestionId = promptAnswerViewModel.QuestionId, Value = promptAnswer });
            var amendment = GetAmendment();

            var outcome = _amendmentService.CreateAmendment(amendment);

            if (outcome.ValidationErrors != null && outcome.ValidationErrors.Count > 0)
            {
                var errorsPromptViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex, outcome.ValidationErrors)
                {
                    PupilDetails = new PupilViewModel(amendment.Pupil)
                };

                return View("Prompt", errorsPromptViewModel);
            }
            
            var nextQuestionViewModel = new QuestionViewModel(questions, promptAnswerViewModel.CurrentIndex + 1)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            if (nextQuestionViewModel.HasMoreQuestions)
            {
                return View("Prompt", nextQuestionViewModel);
            }

            return RedirectToAction("Confirm");
        }

        public IActionResult Prompt()
        {
            var amendment = GetAmendment();

            if (amendment == null) return RedirectToAction("Index", "TaskList");

            var amendmentOutcome = _amendmentService.CreateAmendment(amendment);

            amendment.EvidenceStatus = amendmentOutcome.EvidenceStatus;

            SaveAmendment(amendment);

            if (amendmentOutcome.IsComplete || amendmentOutcome.FurtherQuestions == null)
            {
                SaveAmendment(amendment);

                return RedirectToAction("Confirm");
            }

            var questions = amendmentOutcome.FurtherQuestions.ToList();
            SaveQuestions(questions);

            var promptViewModel = new QuestionViewModel(questions)
            {
                PupilDetails = new PupilViewModel(amendment.Pupil)
            };

            return View("Prompt", promptViewModel);
        }
    }
}
