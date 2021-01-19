﻿using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IAmendmentService
    {
        Amendment GetAmendment(CheckingWindow checkingWindow, string id);
        IEnumerable<IDictionary<string, object>> GetAmendments();
        AdjustmentOutcome AddAmendment(Amendment amendment);
        void RelateEvidence(string amendmentId, string evidenceFolderName, bool updateEvidenceOption);
        bool CancelAmendment(string amendmentId);
        IEnumerable<Amendment> GetAmendments(CheckingWindow checkingWindow, string urn);
    }
}