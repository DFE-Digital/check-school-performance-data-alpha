﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class PupilDTO
    {
        public string id { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string DFESNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int ENTRYDAT { get; set; }
        public string ActualYearGroup { get; set; }

        public List<ResultDTO> performance { get; set; }

        public Pupil Pupil => new Pupil
        {
            Id = new PupilId(id),
            Urn = new URN(URN),
            UPN = UPN,
            LaEstab = DFESNumber,
            ForeName = Forename,
            LastName = Surname,
            DateOfBirth = DateTime.ParseExact(DOB.ToString(), "yyyyMMdd", new CultureInfo("en-GB")),
            Age = Age,
            Gender = Gender == "M" ? Domain.Core.Enums.Gender.Male : Domain.Core.Enums.Gender.Female,
            DateOfAdmission = DateTime.ParseExact(ENTRYDAT.ToString(), "yyyyMMdd", new CultureInfo("en-GB")),
            YearGroup = ActualYearGroup,
            Results = performance.Select(p => new Result { SubjectCode = p.SubjectCode, ExamYear = p.ExamYear, TestMark = p.TestMark, ScaledScore = p.ScaledScore }).ToList()
        };
    }
}
