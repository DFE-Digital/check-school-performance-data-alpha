﻿namespace Dfe.Rscd.Api.Domain.Entities
{
    public class ValidatedAnswer
    {
        public string QuestionId { get; set; }
        public string Value { get; set; }
        public bool IsRejected { get;set; }
    }
}