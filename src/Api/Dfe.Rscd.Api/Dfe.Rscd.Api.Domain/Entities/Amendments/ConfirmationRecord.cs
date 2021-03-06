﻿using System;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public class ConfirmationRecord
    {
        public string UserId { get; set; }
        public string EstablishmentId { get; set; }
        public bool ReviewCompleted { get; set; }
        public bool DataConfirmed { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
}