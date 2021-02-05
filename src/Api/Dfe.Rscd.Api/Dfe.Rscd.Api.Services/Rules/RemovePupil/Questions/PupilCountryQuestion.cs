﻿using System.Collections.Generic;
using Dfe.Rscd.Api.Domain.Entities.Questions;

namespace Dfe.Rscd.Api.Services.Rules
{
    public class PupilCountryQuestion : ConditionalFurtherQuestion
    {
        public PupilCountryQuestion(List<AnswerPotential> countries)
        {
            Setup(countries);
        }

        private void Setup(List<AnswerPotential> countries)
        {
            var nonConditionalQuestion =
                new SelectQuestion(
                    nameof(PupilCountryQuestion),
                    "Pupil's originating country",
                    "Select pupil's originating country",
                    countries, new Validator
                    {
                        AllowNull = false,
                        InValidErrorMessage = string.Empty,
                        NullErrorMessage = "Select pupil's originating country",
                        ValidatorType = ValidatorType.None
                    });

            SetupNonConditionalQuestion(nonConditionalQuestion);

            var conditionalQuestion = new StringQuestion(
                $"{nameof(PupilCountryQuestion)}.Other",
                "Select pupil's originating country",
                "You have selected 'Other'. Please enter the pupil's originating country.", new Validator
                {
                    InValidErrorMessage = "Enter pupil's native language",
                    NullErrorMessage = "Enter a valid pupil's native language",
                    ValidatorType = ValidatorType.AlphabeticalIncludingSpecialChars,
                    AllowNull = false,
                });

            SetupConditionalQuestion(conditionalQuestion, "Other");
        }
    }
}