using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult Land(Map map)
        {
            return Ok(GetBestLanding(map));
        }

        private static List<Generation> GetBestLanding(Map map)
        {
            var environment = new MarsLanderEnvironment(map.SurfaceElements.ToList());
            var lander = new Lander
            {
                Situation = new Situation
                {
                    Fuel = map.InitialFuel,
                    Power = map.InitialPower,
                    Rotation = map.InitialRotation,
                    X = map.InitialX,
                    Y = map.InitialY,
                    HorizontalSpeed = map.InitialHorizontalSpeed,
                    VerticalSpeed = map.InitialVerticalSpeed
                }
            };
            var evolution = new MarsLanderEvolution(environment);

            evolution.Run(100, 50, 100, lander);

            return evolution.Generations;
        }
    }
}