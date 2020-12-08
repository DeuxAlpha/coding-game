using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CodinGame.Application.Models;
using CodinGame.MarsLander;
using CodinGame.MarsLander.Actors;
using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.Models.Dtos;
using CodinGame.MarsLander.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace CodinGame.Application.Controllers
{
    [ApiController]
    [Route("mars-lander")]
    public class MarsLanderController : Controller
    {
        private readonly ILogger<MarsLanderController> _logger;

        public MarsLanderController(ILogger<MarsLanderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("maps")]
        public IActionResult GetMaps()
        {
            return Ok(Maps.Get());
        }

        [HttpPost("land")]
        public IActionResult Land(LandRequest landRequest)
        {
            return Ok(GetBestLanding(landRequest));
        }

        private List<Generation> GetBestLanding(LandRequest landRequest)
        {
            var environment = new MarsLanderEnvironment(landRequest.Map.SurfaceElements.ToList());
            var lander = new Lander
            {
                Situation = new Situation
                {
                    Fuel = landRequest.Map.InitialFuel,
                    Power = landRequest.Map.InitialPower,
                    Rotation = landRequest.Map.InitialRotation,
                    X = landRequest.Map.InitialX,
                    Y = landRequest.Map.InitialY,
                    HorizontalSpeed = landRequest.Map.InitialHorizontalSpeed,
                    VerticalSpeed = landRequest.Map.InitialVerticalSpeed
                }
            };
            var evolution = new MarsLanderEvolution(environment, landRequest.AiWeight);

            if (landRequest.Parameters.Actions != null) evolution.MaxActions = landRequest.Parameters.Actions <= 0 ? null : landRequest.Parameters.Actions;

            var sw = Stopwatch.StartNew();
            evolution.Run(landRequest.Parameters.Generations, landRequest.Parameters.Population, lander);
            sw.Stop();
            _logger.LogInformation("{elapsed}", sw.Elapsed);

            return evolution.Generations;
        }
    }
}