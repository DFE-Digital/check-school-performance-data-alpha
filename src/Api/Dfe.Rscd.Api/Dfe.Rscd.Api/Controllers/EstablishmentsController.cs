﻿using System;
using System.Threading.Tasks;
using Dfe.Rscd.Api.Domain.Entities;
using Dfe.Rscd.Api.Models;
using Dfe.Rscd.Api.Models.SearchRequests;
using Dfe.Rscd.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Rscd.Api.Controllers
{
    [Route("api/{checkingWindow}/[controller]")]
    [ApiController]
    public class EstablishmentsController : ControllerBase
    {
        private readonly IEstablishmentService _establishmentService;

        public EstablishmentsController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        [HttpGet]
        [Route("{urn}")]
        [SwaggerOperation(
            Summary = "Searches for a school",
            Description = "Searches for a school identified by the supplied URN and returns an Establishment object.",
            OperationId = "GetEstablishmentByURN",
            Tags = new[] {"Establishments"}
        )]
        [ProducesResponseType(typeof(GetResponse<School>), 200)]
        public IActionResult Get(
            [FromRoute] [SwaggerParameter("The URN of the school requesting amendments", Required = true)]
            string urn,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            CheckingWindow checkingWindow)
        {
            var urnValue = new URN(urn);
            var establishmentData = _establishmentService.GetByURN(checkingWindow, urnValue);
            var response = new GetResponse<School>
            {
                Result = establishmentData,
                Error = new Error()
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("search")]
        [SwaggerOperation(
            Summary = "Searches for schools or colleges.",
            Description = @"Searches for schools or colleges based on the supplied query parameters.",
            OperationId = "SearchTEstablishments",
            Tags = new[] {"Establishments"}
        )]
        [ProducesResponseType(typeof(GetResponse<School>), 200)]
        [ProducesResponseType(400)]
        public IActionResult Search(
            [FromQuery] [SwaggerParameter("Event search criteria.", Required = true)]
            EstablishmentsSearchRequest request,
            [FromRoute] [SwaggerParameter("The checking window to request amendments from", Required = true)]
            CheckingWindow checkingWindow)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var establishmentData = _establishmentService.GetByDFESNumber(checkingWindow, request.DFESNumber);
            var response = new GetResponse<School>
            {
                Result = establishmentData,
                Error = new Error()
            };
            return Ok(response);
        }
    }
}