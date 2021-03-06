using System.Collections.Generic;
using System.Linq;
using Dfe.Rscd.Web.ApiClient;
using Dfe.Rscd.Web.Application.Models.ViewModels.Pupil;
using Microsoft.AspNetCore.Http;

namespace Dfe.Rscd.Web.Application.Models.ViewModels.Amendments
{
    public class QuestionViewModel : ContextAwareViewModel
    {
        public string GetTitle()
        {
            return CurrentQuestion.Title;
        }

        public PupilViewModel PupilDetails { get; set; }
        public string PupilLabel => GetPupilLabel();

        public ICollection<Question> Questions { get;set; }

        public int CurrentQuestionIndex { get; set; }

        public bool HasMoreQuestions => Questions.Count > CurrentQuestionIndex;

        private string GetPupilLabel()
        {
            return PersonLowercase;
        }

        public Question CurrentQuestion
        {
            get
            {
                var currentQuestion = Questions.ToList()[CurrentQuestionIndex];
                if (ShowConditional)
                    return currentQuestion.Answer.ConditionalQuestion;

                return currentQuestion;
            }
        }

        public IDictionary<string, ICollection<string>> ValidationErrors { get; set; }

        public string BackController { get; set; }
        public string BackAction { get; set; }

        public bool ShowConditional { get; set; }

        public QuestionViewModel(ICollection<Question> questions, int currentIndex=0, IDictionary<string, ICollection<string>> validationErrors=null)
        {
            Questions = questions;
            CurrentQuestionIndex = currentIndex;
            ValidationErrors = validationErrors;
        }
    }
}
