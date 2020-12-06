using System;
using System.Collections.Generic;
using System.Linq;
using CodinGame.Application.Models;
using CodinGame.MarsLander;
using CodinGame.MarsLander.Actors;
using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.Models.Dtos;
using CodinGame.MarsLander.Storage;
using Microsoft.AspNetCore.Mvc;

namespace CodinGame.Application.Controllers
{
    [ApiController]
    [Route("mars-lander")]
    public class MarsLanderController : Controller
    {
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

        private static List<Generation> GetBestLanding(LandRequest landRequest)
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

            evolution.Run(landRequest.Parameters.Generations, landRequest.Parameters.Population, 150, lander);

            return evolution.Generations;
        }
    }
}