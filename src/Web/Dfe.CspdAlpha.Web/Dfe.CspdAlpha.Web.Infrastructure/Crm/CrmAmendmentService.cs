﻿using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Core.Enums;
using Dfe.CspdAlpha.Web.Domain.Entities;
using Dfe.CspdAlpha.Web.Domain.Interfaces;
using Dfe.CspdAlpha.Web.Shared.Config;
using Microsoft.Extensions.Options;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dfe.CspdAlpha.Web.Infrastructure.Crm
{
    /// <summary>
    /// Dynamics 365-backed amendment store.
    /// </summary>
    /// <remarks>Currently uses Dynamics SDK (OrganizationServiceContext), which curiously 
    /// doesn't seem to support async calls. Will probably want to switch to use Web API
    /// at some point.</remarks>
    public class CrmAmendmentService : IAmendmentService
    {
        private readonly IOrganizationService _organizationService;
        private readonly Guid _firstLineTeamId;
        private IEstablishmentService _establishmentService;

        private const string fileUploadRelationshipName = "cr3d5_new_Amendment_Evidence_cr3d5_Fileupload";

        public CrmAmendmentService(
            IOrganizationService organizationService, 
            IEstablishmentService establishmentService,
            IOptions<DynamicsOptions> config)
        {
            _establishmentService = establishmentService;
            _organizationService = organizationService;
            _firstLineTeamId = config.Value.Helpdesk1stLineTeamId;
        }

        private cr3d5_establishment GetOrCreateEstablishment(string id, CrmServiceContext context)
        {
            var establishmentDto =
                context.cr3d5_establishmentSet.SingleOrDefault(
                    e => e.cr3d5_Urn == id);
            if (establishmentDto == null)
            {
                establishmentDto =
                    context.cr3d5_establishmentSet.SingleOrDefault(
                        e => e.cr3d5_LAEstab == id);
            }
            if (establishmentDto != null)
            {
                return establishmentDto;
            }

            Establishment establishment = null;
            try
            {
                establishment = _establishmentService.GetByURN(new URN(id));
            }
            catch { }

            if (establishment == null)
            {
                establishment = _establishmentService.GetByLAId(id);
            }

            if (establishment == null)
            {
                return null;
            }


            establishmentDto = new cr3d5_establishment
            {
                cr3d5_name = establishment.Name,
                cr3d5_Urn = establishment.Urn.Value,
                cr3d5_LAEstab = establishment.LaEstab,
                cr3d5_Schooltype = establishment.SchoolType,
                cr3d5_Numberofamendments = 0
            };
            context.AddObject(establishmentDto);
            var result = context.SaveChanges();
            if (result.HasError)
            {
                throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
            }

            return establishmentDto;
        }

        public bool CreateAddPupilAmendment(AddPupilAmendment amendment, out string id)
        {
            Guid amendmentId;
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendmentDto = new new_Amendment
                {
                    rscd_Amendmenttype = new_Amendment_rscd_Amendmenttype.Addpupil
                };

                // Reason for adding
               amendmentDto.cr3d5_addreasontype = amendment.AddReason == AddReason.New
                   ? cr3d5_Pupiltype.Newpupil
                   : cr3d5_Pupiltype.Existingpupil;

               // Pupil data
                amendmentDto.cr3d5_pupilid = amendment.AddReason == AddReason.Existing ? amendment.Pupil.UPN : string.Empty;
                amendmentDto.cr3d5_laestab = amendment.AddReason == AddReason.Existing ? amendment.Pupil.LaEstab : string.Empty;
                amendmentDto.cr3d5_urn = amendment.Pupil.Urn.Value;
                amendmentDto.new_Name = amendment.Pupil.FullName;
                amendmentDto.cr3d5_forename = amendment.Pupil.ForeName;
                amendmentDto.cr3d5_surname = amendment.Pupil.LastName;
                amendmentDto.cr3d5_gender =
                    amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male : cr3d5_Gender.Female;
                amendmentDto.cr3d5_dob = amendment.Pupil.DateOfBirth;
                amendmentDto.cr3d5_admissiondate = amendment.Pupil.DateOfAdmission;
                amendmentDto.cr3d5_yeargroup = amendment.Pupil.YearGroup;

                // prior attainment result
                if (amendment.PriorAttainmentResults.Any(r => r.Subject == Ks2Subject.Reading))
                {
                    var readingResult = amendment.PriorAttainmentResults.First(r => r.Subject == Ks2Subject.Reading);
                    amendmentDto.rscd_ReadingExamYear = readingResult.ExamYear;
                    amendmentDto.rscd_ReadingTestMark = readingResult.TestMark;
                    amendmentDto.rscd_ReadingScaledScore = readingResult.ScaledScore;
                }
                if (amendment.PriorAttainmentResults.Any(r => r.Subject == Ks2Subject.Writing))
                {
                    var writingResult = amendment.PriorAttainmentResults.First(r => r.Subject == Ks2Subject.Writing);
                    amendmentDto.rscd_WritingExamYear = writingResult.ExamYear;
                    amendmentDto.rscd_WritingTestMark = writingResult.TestMark;
                    amendmentDto.rscd_WritingScaledScore = writingResult.ScaledScore;
                }
                if (amendment.PriorAttainmentResults.Any(r => r.Subject == Ks2Subject.Maths))
                {
                    var mathsResult = amendment.PriorAttainmentResults.First(r => r.Subject == Ks2Subject.Maths);
                    amendmentDto.rscd_MathsExamYear = mathsResult.ExamYear;
                    amendmentDto.rscd_MathsTestMark = mathsResult.TestMark;
                    amendmentDto.rscd_MathsScaledScore = mathsResult.ScaledScore;
                }

                // Inclusion details
                amendmentDto.cr3d5_includeinperformanceresults = amendment.InclusionConfirmed;

                // Evidence status
                switch (amendment.EvidenceStatus)
                {
                    case EvidenceStatus.Now:
                        amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceNow;
                        break;
                    case EvidenceStatus.Later:
                        amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceLater;
                        break;
                    case EvidenceStatus.NotRequired:
                    default:
                        amendmentDto.cr3d5_evidenceoption = cr3d5_EvidenceOption.DontUploadEvidence;
                        break;
                }

                // assign to helpdesk 1st line
                amendmentDto.OwnerId = new EntityReference("team", _firstLineTeamId);

                // Save
                context.AddObject(amendmentDto);
                var result = context.SaveChanges();
                if (result.HasError)
                {
                    throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
                }
                // Relate to establishment
                var amendmentEstablishment = GetOrCreateEstablishment(amendment.Pupil.Urn.Value, context);
                RelateEstablishment(amendmentEstablishment, amendmentDto.Id, context);

                // If add existing pupil then create matching remove amendment if valid establishment
                if (amendmentDto.cr3d5_addreasontype == cr3d5_Pupiltype.Existingpupil)
                {
                    var removeAmendmentEstablishment = GetOrCreateEstablishment(amendment.Pupil.LaEstab, context);
                    if (removeAmendmentEstablishment != null)
                    {
                        // Create remove amendment
                        var removeDto = new new_Amendment
                        {
                            rscd_Amendmenttype = new_Amendment_rscd_Amendmenttype.Removepupil,
                            new_Name = amendment.Pupil.FullName,
                            cr3d5_laestab = amendment.Pupil.LaEstab,
                            cr3d5_pupilid = amendment.Pupil.UPN,
                            cr3d5_forename = amendment.Pupil.ForeName,
                            cr3d5_surname = amendment.Pupil.LastName,
                            cr3d5_gender = amendment.Pupil.Gender == Gender.Male ? cr3d5_Gender.Male : cr3d5_Gender.Female,
                            cr3d5_dob = amendment.Pupil.DateOfBirth,
                            OwnerId = new EntityReference("team", _firstLineTeamId)
                        };
                        context.AddObject(removeDto);
                        result = context.SaveChanges();
                        if (result.HasError)
                        {
                            throw result.FirstOrDefault(e => e.Error != null)?.Error ?? new ApplicationException();
                        }
                        // Relate to establishment
                        RelateEstablishment(removeAmendmentEstablishment, removeDto.Id, context);
                        // Create amendment relationship
                        var addExistingPupilRelationship = new Relationship("rscd_new_amendment_new_amendment");
                        addExistingPupilRelationship.PrimaryEntityRole = EntityRole.Referencing;
                        _organizationService.Associate(new_Amendment.EntityLogicalName, amendmentDto.Id, addExistingPupilRelationship, new EntityReferenceCollection
                        {
                            new EntityReference(new_Amendment.EntityLogicalName, removeDto.Id)
                        });
                        _organizationService.Associate(new_Amendment.EntityLogicalName, removeDto.Id, addExistingPupilRelationship, new EntityReferenceCollection
                        {
                            new EntityReference(new_Amendment.EntityLogicalName, amendmentDto.Id)
                        });
                    }
                }

                var addPUp = context.CreateQuery<new_Amendment>().Single(e => e.Id == amendmentDto.Id);
                id = addPUp?.cr3d5_addpupilref;
                amendmentId = amendmentDto.Id;
            }

            // Upload Evidence
            if (amendment.HasUploadedEvidence)
            {
                RelateEvidence(amendmentId, amendment.EvidenceFolderName, false);
            }

            return true;
        }

        public void RelateEstablishment(cr3d5_establishment establishment, Guid amendmentId, CrmServiceContext context)
        {
            var relationship = new Relationship("cr3d5_cr3d5_establishment_new_amendment");
            _organizationService.Associate(cr3d5_establishment.EntityLogicalName, establishment.Id, relationship, new EntityReferenceCollection
            {
                new EntityReference(new_Amendment.EntityLogicalName, amendmentId)
            });
        }

        public void RelateEvidence(Guid amendmentId, string evidenceFolderName, bool updateEvidenceOption)
        {
            // https://community.dynamics.com/crm/f/microsoft-dynamics-crm-forum/203503/adding-a-sharepointdocumentlocation-programmatically/528485
            Entity sharepointdocumentlocation = new Entity("sharepointdocumentlocation");
             sharepointdocumentlocation["name"] = evidenceFolderName;

            // TODO: Currently hard-coded to a document location record that points to the "Amendment" sub-folder. Will need to change
            // this to support different entities.
            sharepointdocumentlocation["parentsiteorlocation"] = new EntityReference(
                "sharepointdocumentlocation", Guid.Parse("34b250ed-aaf2-ea11-a815-000d3abb8438"));
            sharepointdocumentlocation["relativeurl"] = evidenceFolderName;
            sharepointdocumentlocation["regardingobjectid"] = new EntityReference(new_Amendment.EntityLogicalName, amendmentId);

            Guid sharepointdocumentlocationid = _organizationService.Create(sharepointdocumentlocation);

            if (updateEvidenceOption)
            {
                UpdateEvidenceStatus(amendmentId);
            }
        }

        public void RelateEvidence(Guid amendmentId, List<Evidence> evidenceList, bool updateEvidenceOption)
        {
            var relatedFileUploads = new EntityReferenceCollection();
            foreach (var evidence in evidenceList)
            {
                relatedFileUploads.Add(new EntityReference(cr3d5_Fileupload.EntityLogicalName, new Guid(evidence.Id)));
            }

            var relationship = new Relationship(fileUploadRelationshipName);
            _organizationService.Associate(new_Amendment.EntityLogicalName, amendmentId, relationship,
                relatedFileUploads);
            
            if (updateEvidenceOption)
            {
                UpdateEvidenceStatus(amendmentId);
            }
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(int laestab)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendments = context.new_AmendmentSet.Where(
                    x => x.cr3d5_laestab == laestab.ToString()).ToList();

                return amendments.Select(Convert);
            }
        }

        public IEnumerable<AddPupilAmendment> GetAddPupilAmendments(string urn)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendments = context.new_AmendmentSet.Where(
                    x => x.cr3d5_urn == urn).ToList();

                return amendments.Select(Convert);
            }
        }

        private AddPupilAmendment Convert(new_Amendment amendment)
        {
            return new AddPupilAmendment
            {
                Id = amendment.Id.ToString(),
                Reference = amendment.cr3d5_addpupilref,
                Status = GetStatus(amendment),
                CreatedDate = amendment.CreatedOn ?? DateTime.MinValue,
                AddReason = amendment.cr3d5_addreasontype == cr3d5_Pupiltype.Newpupil ? AddReason.New : AddReason.Existing,
                EvidenceStatus = amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceNow ? EvidenceStatus.Now : amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceLater ? EvidenceStatus.Later : EvidenceStatus.NotRequired,
                Pupil = new Pupil
                {
                    Id = string.IsNullOrWhiteSpace(amendment.cr3d5_pupilid) ? null : new PupilId(amendment.cr3d5_pupilid),
                    Urn = new URN(amendment.cr3d5_urn),
                    LaEstab = amendment.cr3d5_laestab,
                    ForeName = amendment.cr3d5_forename,
                    LastName = amendment.cr3d5_surname,
                    DateOfBirth = amendment.cr3d5_dob ?? DateTime.MinValue,
                    Gender = amendment.cr3d5_gender == cr3d5_Gender.Male ? Gender.Male : Gender.Female,
                    DateOfAdmission = amendment.cr3d5_admissiondate ?? DateTime.MinValue,
                    YearGroup = amendment.cr3d5_yeargroup
                },
                PriorAttainmentResults = GetPriorAttainmentResult(amendment),
                InclusionConfirmed = amendment.cr3d5_includeinperformanceresults ?? false
            };
        }

        private List<PriorAttainment> GetPriorAttainmentResult(new_Amendment amendment)
        {
            var results = new List<PriorAttainment>();
            if (!string.IsNullOrEmpty(amendment.rscd_ReadingExamYear))
            {
                results.Add(new PriorAttainment
                {
                    Subject = Ks2Subject.Reading,
                    ExamYear = amendment.rscd_ReadingExamYear,
                    TestMark = amendment.rscd_ReadingTestMark,
                    ScaledScore = amendment.rscd_ReadingScaledScore
                });
            }
            if (!string.IsNullOrEmpty(amendment.rscd_WritingExamYear))
            {
                results.Add(new PriorAttainment
                {
                    Subject = Ks2Subject.Writing,
                    ExamYear = amendment.rscd_WritingExamYear,
                    TestMark = amendment.rscd_WritingTestMark,
                    ScaledScore = amendment.rscd_WritingScaledScore
                });
            }
            if (!string.IsNullOrEmpty(amendment.rscd_MathsExamYear))
            {
                results.Add(new PriorAttainment
                {
                    Subject = Ks2Subject.Maths,
                    ExamYear = amendment.rscd_MathsExamYear,
                    TestMark = amendment.rscd_MathsTestMark,
                    ScaledScore = amendment.rscd_MathsScaledScore
                });
            }

            return results;
        }

        private string GetStatus(new_Amendment amendment)
        {
            if (amendment.cr3d5_firstlinedecision == cr3d5_Decision.Approved ||
                amendment.cr3d5_firstlinedecision == cr3d5_Decision.Rejected)
            {
                return amendment.cr3d5_firstlinedecision.ToString();
            }

            if (amendment.cr3d5_secondlinedecision == cr3d5_Decision.Approved ||
                amendment.cr3d5_secondlinedecision == cr3d5_Decision.Rejected)
            {
                return amendment.cr3d5_secondlinedecision.ToString();
            }

            if (amendment.cr3d5_finaldecision != null)
            {
                return amendment.cr3d5_finaldecision.ToString();
            }

            return amendment.cr3d5_amendmentstatus.ToString();
        }


        public AddPupilAmendment GetAddPupilAmendmentDetail(Guid amendmentId)
        {
            using (var context = new CrmServiceContext(_organizationService))
            {
                var amendment = context.new_AmendmentSet.Where(
                    x => x.Id == amendmentId).FirstOrDefault();

                // TODO: Get relationship name from attribute
                var relationship = new Relationship(fileUploadRelationshipName);
                context.LoadProperty(amendment, relationship);

                return Convert(amendment);
            }
        }

        /// <remarks>In a lot of cases, performing queries using IOrganizationService rather than
        /// CrmServiceContext is likely to give better performance by explicitly only retrieving
        /// the fields you actually need.</remarks>
        public bool CancelAddPupilAmendment(Guid amendmentId)
        {
            // TODO: Get field name from attribute
            var cols = new ColumnSet(
                        new String[] { "cr3d5_amendmentstatus" });
            var amendment = (new_Amendment) _organizationService.Retrieve(
                new_Amendment.EntityLogicalName, amendmentId, cols);

            if (amendment == null
                || amendment.cr3d5_amendmentstatus == new_amendmentStatus.Cancelled)
            {
                return false;
            }

            amendment.cr3d5_amendmentstatus = new_amendmentStatus.Cancelled;

            _organizationService.Update(amendment);

            return true;
        }

        private bool UpdateEvidenceStatus(Guid amendmentId)
        {
            var cols = new ColumnSet( new [] { "cr3d5_evidenceoption" });
            var amendment = (new_Amendment)_organizationService.Retrieve(new_Amendment.EntityLogicalName, amendmentId, cols);

            if (amendment == null || amendment.cr3d5_evidenceoption == cr3d5_EvidenceOption.UploadEvidenceNow)
            {
                return false;
            }

            amendment.cr3d5_evidenceoption = cr3d5_EvidenceOption.UploadEvidenceNow;

            _organizationService.Update(amendment);

            return true;
        }
    }
}
