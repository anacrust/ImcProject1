﻿using System;
using System.Threading.Tasks;
using ApiClient.Models;
using ApiClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public TaxCalculatorController(IConfiguration config, ILogger<TaxCalculatorController> logger)
        {
            _config = config;
            _logger = logger;
        }

        // For dev only
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return Ok(new { Message = "This works" });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest();
            }

        }


        // TODO: add taxjar service that adds up all the tax and returns a tax ActionResult<TaxSumViewModel> instead, incorporate into stratgy pattern
        [HttpGet("location/{zip}")]
        public async Task<ActionResult<TaxRateLocation>> GetTaxRateByZip(string zip)
        {
            try
            {
                var taxCalculator = new TaxCalculator(_config["TaxCalculators:TaxJar:BaseUrl"], _config["TaxCalculators:TaxJar:Authorization"]);
                //var userId = Request.Headers["UserId"];
                var taxService = new TaxCalculatorService(taxCalculator, _logger);
                var taxRateLocation = await taxService.GetRateByLocation(zip);
                return Ok(taxRateLocation);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest();
            }

        }

        // TODO: add taxjar service that adds up all the tax and returns a tax ActionResult<TaxSumViewModel> instead, incorporate into stratgy pattern
        [HttpGet("order/{id}")]
        public async Task<ActionResult<TaxRateOrder>> GetTaxRateByOrderId(string id)
        {
            try
            {
                var taxCalculator = new TaxCalculator(_config["TaxCalculators:TaxJar:BaseUrl"], _config["TaxCalculators:TaxJar:Authorization"]);
                //var userId = Request.Headers["UserId"];
                var taxService = new TaxCalculatorService(taxCalculator, _logger);
                var taxRateOrder = await taxService.GetRateByOrder(id);
                return Ok(taxRateOrder);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest();
            }

        }

    }
}