﻿using System;

namespace Dfe.Rscd.Api.Domain.Entities.Questions
{
    public class NumberQuestion : Question
    {
        public NumberQuestion(string title, string label, Validator validator)
        {
            Id = Guid.NewGuid();
            Title = title;
            QuestionType = QuestionType.Number;
            Validator = validator;
            Answer = new Answer
            {
                QuestionId = Id,
                Label = label
            };
        }
    }
}
