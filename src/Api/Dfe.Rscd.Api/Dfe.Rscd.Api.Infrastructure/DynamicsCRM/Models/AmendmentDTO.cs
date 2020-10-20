﻿using Dfe.Rscd.Api.Domain.Entities;
using Newtonsoft.Json;

namespace Dfe.Rscd.Api.Infrastructure.DynamicsCRM.Models
{
    public class AmendmentDTO
    {
        public Amendment Amendment { get; set; }
        public RemovePupil RemovePupil { get; set; }
        public AddPupil AddPupil { get; set; }
    }
}