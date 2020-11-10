﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Dfe.Rscd.Api.Domain.Core;
using Dfe.Rscd.Api.Domain.Core.Enums;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Extensions;

namespace Dfe.Rscd.Api.Infrastructure.CosmosDb.DTOs
{
    public class PupilDTO
    {
        public string id { get; set; }
        public string URN { get; set; }
        public string UPN { get; set; }
        public string ULN { get; set; }
        public string DFESNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int DOB { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int ENTRYDAT { get; set; }
        public string ActualYearGroup { get; set; }
        public bool Attendance_year_0 { get; set; }
        public bool Attendance_year_1 { get; set; }
        public bool Attendance_year_2 { get; set; }
        public string Core_Provider_0 { get; set; }
        public string Core_Provider_1 { get; set; }
        public string Core_Provider_2 { get; set; }
        public string SRC_LAESTAB_0 { get; set; }
        public string SRC_LAESTAB_1 { get; set; }
        public string SRC_LAESTAB_2 { get; set; }
        public List<ResultDTO> performance { get; set; }

        public Pupil GetPupil(string allocationYear)
        {
            return new Pupil
            {
                Id = id,
                URN = URN,
                UPN = UPN,
                ULN = ULN,
                LaEstab = DFESNumber,
                ForeName = Forename,
                LastName = Surname,
                DateOfBirth = GetDateTime(DOB.ToString()),
                Age = Age,
                Gender = Gender == "M" ? Domain.Core.Enums.Gender.Male : Domain.Core.Enums.Gender.Female,
                DateOfAdmission = GetDateTime(ENTRYDAT.ToString()),
                YearGroup = ActualYearGroup,
                Results = performance.Select(p => new Result { SubjectCode = p.SubjectCode, ExamYear = p.ExamYear, TestMark = p.TestMark, ScaledScore = p.ScaledScore }).ToList(),
                Allocations = GetSourceOfAllocations(allocationYear)
            };
        }

        private List<SourceOfAllocation> GetSourceOfAllocations(string allocationYear)
        {
            var year = int.Parse(allocationYear);
            var allocations = new List<SourceOfAllocation>();
            if (string.IsNullOrEmpty(SRC_LAESTAB_0) && string.IsNullOrEmpty(SRC_LAESTAB_1) && string.IsNullOrEmpty(SRC_LAESTAB_2))
            {
                return allocations;
            }

            if (Attendance_year_0 && DFESNumber == Core_Provider_0)
            {
                allocations.Add(new SourceOfAllocation(year--, SRC_LAESTAB_0.ToAllocation())); 
            }
            else
            {
                allocations.Add(new SourceOfAllocation(year--, Allocation.NotAllocated));
            }
            if (Attendance_year_1 && DFESNumber == Core_Provider_1)
            {
                allocations.Add(new SourceOfAllocation(year--, SRC_LAESTAB_1.ToAllocation()));
            }
            else
            {
                allocations.Add(new SourceOfAllocation(year--, Allocation.NotAllocated));
            }
            if (Attendance_year_2 && DFESNumber == Core_Provider_2)
            {
                allocations.Add(new SourceOfAllocation(year--, SRC_LAESTAB_2.ToAllocation()));
            }
            else
            {
                allocations.Add(new SourceOfAllocation(year--, Allocation.NotAllocated));
            }

            return allocations;
        }

        private DateTime GetDateTime(string dateString)
        {
            return DateTime.TryParseExact(dateString, "yyyyMMdd", new CultureInfo("en-GB"), DateTimeStyles.None, out var date ) ? date : DateTime.MinValue;
        }
    }
}
