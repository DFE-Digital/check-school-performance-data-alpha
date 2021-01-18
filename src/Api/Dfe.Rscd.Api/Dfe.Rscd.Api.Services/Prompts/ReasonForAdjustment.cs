﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dfe.Rscd.Api.Services
{
    internal enum ReasonsForAdjustment
    {
        CancelAddBack = 3,
        CancelAdjustmentForAdmissionFollowingPermanentExclusion = 4,
        RemovePupilCompletelyFromAllData = 5,
        PublishPupilAtThisSchool = 6,
        ReinstateThePupilKS4 = 7,
        AdmittedFromAbroad = 8,
        ContingencyKS4 = 9,
        AdmittedFollowingPermanentExclusionFromMaintainedSchool = 10,
        PermanentlyLeftEngland = 11,
        Deceased = 12,
        PupilNotAtEndOfKeyStage4 = 13,
        PupilAtEndOfKeyStage4 = 17,
        LeftSchoolRollBeforeExamsKS4 = 18,
        KS4Other = 19,
        AddPupilToAchievementAndAttainmentTablesKS4 = 21,
        ResultsBelongToAnotherPupilKS4 = 23,
        NotAtEndOfAdvancedStudy = 54,
        LeftBeforeExamsKS5 = 55,
        KS5Other = 56,
        AddPupilToAchievementAndAttainmentTablesKS5 = 57,
        ResultsBelongToAnotherPupilKS5 = 58,
        ReinstatePupilKS5 = 59,
        AddUnlistedPupilToAATKS2 = 92,
        AddUnlistedPupilToAATKS4 = 94,
        AddUnlistedStudentToAATKS5 = 95,
        ContingencyKS2 = 212,
        PupilNotAtEndOfKeyStage2InAllSubjects = 213,
        LeftSchoolRollBeforeTestsKS2 = 218,
        KS2Other = 219,
        AddPupilToAchievementAndAttainmentTablesKS2 = 221,
        ResultsBelongToAnotherPupilKS2 = 223,
        PupilNotAtEndOfKeyStage3InAllSubjects = 313,
        LeftSchoolRollBeforeTestsKS3 = 318,
        KS3Other = 319, 
        AddPupilToAchievementAndAttainmentTablesKS3 = 321,
        ResultsBelongToAnotherPupilKS3 = 323,
        CancelAddBackKS5 = 324
    }
}