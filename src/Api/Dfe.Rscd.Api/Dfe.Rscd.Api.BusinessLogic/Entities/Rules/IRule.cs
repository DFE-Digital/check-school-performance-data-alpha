﻿using System.Collections.Generic;

namespace Dfe.Rscd.Api.Domain.Entities
{
    public interface IRule
    {
        AmendmentType AmendmentType { get; }
        AmendmentOutcome Apply(Amendment amendment);

        int AmendmentReason { get; }
    }
}