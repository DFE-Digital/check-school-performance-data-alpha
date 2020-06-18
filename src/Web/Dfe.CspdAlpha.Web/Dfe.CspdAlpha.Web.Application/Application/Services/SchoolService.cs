using System.Collections.Generic;
using System.Linq;
using Dfe.CspdAlpha.Web.Application.Application.Interfaces;
using Dfe.CspdAlpha.Web.Application.Models.School;
using Dfe.CspdAlpha.Web.Application.Models.ViewModels;
using Dfe.CspdAlpha.Web.Domain.Core;
using Dfe.CspdAlpha.Web.Domain.Interfaces;

namespace Dfe.CspdAlpha.Web.Application.Application.Services
{
    public class SchoolService : ISchoolService
    {
        private IEstablishmentService _establishmentService;
        private readonly List<string> HEADLINE_MEASURES = new List<string> { "P8_BANDING", "PTEBACC_E_PTQ_EE", "PTL2BASICS_95" };
        private readonly List<string> ADDITIONAL_MEASURES = new List<string> { "PTL2BASICS_94", "ATT8SCR", "EBACCAPS" };
        private IPupilService _pupilService;

        public SchoolService(IEstablishmentService establishmentService, IPupilService pupilService)
        {
            _pupilService = pupilService;
            _establishmentService = establishmentService;
        }

        public SchoolViewModel GetSchoolViewModel(string urn)
        {
            var urnValue = new URN(urn);
            var establishmentData = _establishmentService.GetByURN(urnValue);
            return new SchoolViewModel
            {
                SchoolDetails = new SchoolDetails
                {
                    SchoolName = establishmentData.Name,
                    URN = urn
                },
                HeadlineMeasures = establishmentData.PerformanceMeasures.Where(p => HEADLINE_MEASURES.Any(h => h == p.Name)).Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList(),
                AdditionalMeasures = establishmentData.PerformanceMeasures.Where(p => ADDITIONAL_MEASURES.Any(h => h == p.Name)).Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList(),
                CohortMeasures = establishmentData.CohortMeasures.Select(m => new Measure{Name = m.Name, Data = m.Value}).ToList()
            };
        }

        public PupilListViewModel GetPupilListViewModel(string urn)
        {
            var urnValue = new URN(urn);
            return new PupilListViewModel
            {
                Urn = urn,
                Pupils = _pupilService
                    .GetByUrn(urnValue)
                    .Select(p => new Pupil {FirstName = p.ForeName, LastName = p.LastName, PupilId = p.Id.Value})
                    .OrderBy(p => p.FirstName)
                    .ToList()
            };
        }
    }
}
