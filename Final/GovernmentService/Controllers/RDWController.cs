using GovernmentService.Models;
using GovernmentService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dapr.Client;
using System.Collections.Generic;

namespace GovernmentService.Controllers
{
    [ApiController]
    public class RDWController : ControllerBase
    {
        private string _expectedAPIKey;
        private readonly ILogger<RDWController> _logger;
        private readonly IVehicleInfoRepository _vehicleInfoRepository;

        public RDWController(ILogger<RDWController> logger, IVehicleInfoRepository vehicleInfoRepository,
            DaprClient daprClient)
        {
            _logger = logger;
            _vehicleInfoRepository = vehicleInfoRepository;

            // get API key
            var apiKeySecret = daprClient.GetSecretAsync("local-secret-store", "rdw-api-key",
                  new Dictionary<string, string> { { "namespace", "dapr-trafficcontrol" } }).Result;
            _expectedAPIKey = apiKeySecret["rdw-api-key"];
        }

        [HttpGet("rdw/{apikey}/vehicle/{licenseNumber}")]
        public ActionResult<VehicleInfo> GetVehicleDetails(string apiKey, string licenseNumber)
        {
            if (apiKey != _expectedAPIKey)
            {
                return Unauthorized();
            }

            _logger.LogInformation($"RDW: Retrieving vehicle-info for licensenumber {licenseNumber}");
            VehicleInfo info = _vehicleInfoRepository.GetVehicleInfo(licenseNumber);
            return info;
        }
    }
}
