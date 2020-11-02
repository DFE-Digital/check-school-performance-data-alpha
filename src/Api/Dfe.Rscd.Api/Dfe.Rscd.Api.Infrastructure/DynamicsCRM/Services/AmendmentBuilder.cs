﻿using Dfe.CspdAlpha.Web.Infrastructure.Crm;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Config;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Services
{
    public class AmendmentBuilder : IAmendmentBuilder
    {
        private readonly rscd_Checkingwindow[] ks4Windows = new[]
            {rscd_Checkingwindow.KS4June, rscd_Checkingwindow.KS4Late, rscd_Checkingwindow.KS4Errata};
        private readonly string ALLOCATION_YEAR;
        private readonly Guid _firstLineTeamId;
        private IOrganizationService _organizationService;
        private IOutcomeService _outcomeService;

        public AmendmentBuilder(IOrganizationService organizationService, IOutcomeService outcomeService, IOptions<DynamicsOptions> dynamicsOptions, IConfiguration configuration)
        {
            _outcomeService = outcomeService;
            _organizationService = organizationService;
            _firstLineTeamId = dynamicsOptions.Value.Helpdesk1stLineTeamId;
            ALLOCATION_YEAR = configuration["AllocationYear"];
        }

        private Entity BuildAmendmentType(Amendment amendment)
        {
            // Create Remove
            if (amendment.AmendmentType == AmendmentType.RemovePupil)
            {
                var removeDto = new rscd_Removepupil
                {
                    rscd_Name = amendment.Pupil.FullName
                };
                var removeDetail = (RemovePupil)amendment.AmendmentDetail;
                removeDto.rscd_Reason = removeDetail.Reason;
                removeDto.rscd_Subreason = removeDetail.SubReason;
                removeDto.rscd_Details = removeDetail.Detail;
                removeDto.rscd_Allocationyear = removeDetail.AllocationYear;
                return removeDto;
            }

            // Create add
            if (amendment.AmendmentType == AmendmentType.AddPupil)
            {
                var addDto = new rscd_Addpupil
                {
                    rscd_Name = amendment.Pupil.FullName
                };
                var addDetail = (AddPupil)amendment.AmendmentDetail;
                addDto.rscd_Reason = addDetail.Reason.ToCRMAddReason();
                addDto.rscd_PreviousschoolURN = amendment.Pupil.URN;
                addDto.rscd_PreviousschoolLAESTAB = amendment.Pupil.LaEstab;
                var reading = addDetail.PriorAttainmentResults.FirstOrDefault(r => r.Subject == Ks2Subject.Reading);
                if (reading != null)
                {
                    addDto.rscd_Readingexamyear = reading.ExamYear;
                    addDto.rscd_Readingexammark = reading.TestMark;
                    addDto.rscd_Readingscaledscore = reading.ScaledScore;
                }

                var writing = addDetail.PriorAttainmentResults.FirstOrDefault(r => r.Subject == Ks2Subject.Writing);
                if (writing != null)
                {
                    addDto.rscd_Writingexamyear = writing.ExamYear;
                    addDto.rscd_Writingteacherassessment = writing.TestMark;
                    addDto.rscd_Writingscaledscore = writing.ScaledScore;
                }

                var maths = addDetail.PriorAttainmentResults.FirstOrDefault(r => r.Subject == Ks2Subject.Maths);
                if (maths != null)
                {
                    addDto.rscd_Mathsexamyear = maths.ExamYear;
                    addDto.rscd_Mathsexammark = maths.TestMark;
                    addDto.rscd_Mathsscaledscore = maths.ScaledScore;
                }

                return addDto;
            }

            return null;
        }

        public Guid BuildAmendments(Amendment amendment)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new rscd_Amendment
                {
                    rscd_Checkingwindow = amendment.CheckingWindow.ToCRMCheckingWindow(),
                    rscd_Amendmenttype = amendment.AmendmentType.ToCRMAmendmentType(),
                    rscd_Academicyear = ALLOCATION_YEAR,
                    rscd_URN = amendment.URN,
                    OwnerId = new EntityReference("team", _firstLineTeamId),
                    rscd_Recordedby = "RSCD Website" // TODO: what should the recorded by field hold
                };

                // pupil details
                amendmentDto.rscd_UPN = amendment.Pupil.UPN;
                amendmentDto.rscd_ULN = amendment.Pupil.ULN;
                amendmentDto.rscd_Name = amendment.Pupil.FullName;
                amendmentDto.rscd_Firstname = amendment.Pupil.ForeName;
                amendmentDto.rscd_Lastname = amendment.Pupil.LastName;
                amendmentDto.rscd_Gender = amendment.Pupil.Gender.ToCRMGenderType();
                amendmentDto.rscd_Dateofbirth = amendment.Pupil.DateOfBirth;
                amendmentDto.rscd_Age = amendment.Pupil.Age;
                if (ks4Windows.Any(w => w == amendmentDto.rscd_Checkingwindow))
                {
                    amendmentDto.rscd_Dateofadmission = amendment.Pupil.DateOfAdmission;
                    amendmentDto.rscd_Yeargroup = amendment.Pupil.YearGroup;
                } 
                amendmentDto.rscd_Evidencestatus = amendment.EvidenceStatus.ToCRMEvidenceStatus();

                var amendmentTypeEntity = BuildAmendmentType(amendment);
                context.AddObject(amendmentTypeEntity);

                _outcomeService.SetOutcome(amendmentDto, amendment);

                context.AddObject(amendmentDto);

                // Save
                var result = context.SaveChanges();
                if (result.HasError)
                {
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
                }

                if (amendmentDto.rscd_Outcome == rscd_Outcome.Autoapproved ||
                    amendmentDto.rscd_Outcome == rscd_Outcome.Autorejected)
                {
                    amendmentDto.StateCode = rscd_AmendmentState.Inactive;
                    _organizationService.Update(amendmentDto);
                }

                var relationship = new Relationship(amendmentDto.rscd_Amendmenttype == rscd_Amendmenttype.Removeapupil ? "rscd_Amendment_Removepupil" : "rscd_Amendment_Addpupil"); // TODO: need better way to derive this
                _organizationService.Associate(amendmentTypeEntity.LogicalName, amendmentTypeEntity.Id, relationship,
                    new EntityReferenceCollection
                    {
                        new EntityReference(amendmentDto.LogicalName, amendmentDto.Id)
                    });

                return amendmentDto.Id;
            }
        }
    }
}
