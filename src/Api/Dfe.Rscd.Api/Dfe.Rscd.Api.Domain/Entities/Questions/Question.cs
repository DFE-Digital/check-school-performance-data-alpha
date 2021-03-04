using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
	public class Question : IValidatable
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string HelpTextHtml { get; set; }
        public QuestionType QuestionType { get; set; }
        public bool IsValid()
        {
            if (Validator == null)
                throw new ArgumentNullException($"Validator not specified");

            return Validator.IsValid();
        }

        public List<string> Validate(string answer)
        {
            if (Validator == null)
                throw new ArgumentNullException($"Validator not specified");

            return Validator.Validate(answer);
        }

        public Validator Validator { get; set; }

        public Answer Answer { get; set; }

        
        public ValidatedAnswer GetAnswer(List<UserAnswer> userAnswers)
        {
            var userAnswer = userAnswers.Single(x => x.QuestionId == Id);
            var answer = GetAnswerPotential(userAnswer.Value);

            if (Answer.HasConditional && (Answer.ConditionalValue == answer.Value || Answer.ConditionalValue == answer.Description))
            {
                userAnswer = userAnswers.Single(x => x.QuestionId == Answer.ConditionalQuestion.Id);

                return new ValidatedAnswer
                {
                    Value = answer.Description + " - " + userAnswer.Value,
                    IsRejected = answer.Reject,
                    QuestionId = Id
                };
            }

            return new ValidatedAnswer
            {
                Value = answer.Description,
                IsRejected = answer.Reject,
                QuestionId = Id
            };
        }

        private AnswerPotential GetAnswerPotential(string value)
        {
            if (Answer.AnswerPotentials != null && Answer.AnswerPotentials.Count > 0)
            {
                var answer = Answer.AnswerPotentials.SingleOrDefault(x => x.Value == value);
                if (answer != null)
                {
                    return answer;
                }
            }
            
            return new AnswerPotential { Description = value, Value = value, Reject = false};
        }
    }
}

