﻿using System;
using Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Entities;
using Dfe.Rscd.Api.Domain.Entities;

namespace Dfe.Rscd.Api.Services
{
    public static class GenderExtensions
    {
        public static Gender ToDomainGenderType(this cr3d5_Gender gender)
        {
            switch (gender)
            {
                case cr3d5_Gender.Male:
                    return new Gender {Code = "M", Description = "Male"};
                case cr3d5_Gender.Female:
                    return new Gender { Code = "F", Description = "Female" };
                default:
                    throw new ApplicationException();
            }
        }

        public static cr3d5_Gender ToCRMGenderType(this Gender gender)
        {
            switch (gender.Code)
            {
                case "M":
                    return cr3d5_Gender.Male;
                case "F":
                    return cr3d5_Gender.Female;
                default:
                    throw new ApplicationException();
            }
        }
    }
}