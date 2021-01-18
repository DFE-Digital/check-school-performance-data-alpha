﻿using System.Collections.Generic;
using Dfe.Rscd.Api.BusinessLogic.Entities;

namespace Dfe.Rscd.Api.Services
{
    public interface IDataService
    {
        IList<AmendCode> GetAmendCodes();
        IList<AwardingBody> GetAwardingBodies();
        IList<Ethnicity> GetEthnicities();
        IList<InclusionAdjustmentReason> GetInclusionAdjustmentReasons();
        IList<FirstLanguage> GetLanguages();
        IList<PINCLs> GetPINCLs();
        IList<SENStatus> GetSENStatus();
        IList<YearGroup> GetYearGroups();
    }
}