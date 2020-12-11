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

        [HttpPost("calculate-weights")]
        public IActionResult CalculateWeights(CalculateWeightRequest calculateWeightRequest)
        {
            return Ok(GetBestWeights(calculateWeightRequest));
        }

        private List<Generation> GetBestLanding(LandRequest landRequest)
        {
            var environment = new MarsLanderEnvironment(landRequest.Map.SurfaceZones.ToList());
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

            if (landRequest.Parameters.Actions != null)
                evolution.MaxActions = landRequest.Parameters.Actions <= 0 ? null : landRequest.Parameters.Actions;

            var sw = Stopwatch.StartNew();
            evolution.Run(landRequest.Parameters.Generations, landRequest.Parameters.Population, lander);
            sw.Stop();
            var highestScore = evolution.Generations
                .SelectMany(generation => generation.Actors)
                .Select(actor => actor.Score)
                .OrderBy(score => score)
                .First();
            _logger.LogInformation("{elapsed}", sw.Elapsed);
            _logger.LogInformation("Highest Score: {@score}", highestScore);

            return evolution.Generations;
        }

        private IEnumerable<AiWeightSuccessRate> GetBestWeights(CalculateWeightRequest request)
        {
            // Upward tries
            // Horizontal Speed Weight
            var environment = new MarsLanderEnvironment(request.Map.SurfaceZones.ToList());
            var lander = new Lander
            {
                Situation = new Situation
                {
                    Fuel = request.Map.InitialFuel,
                    Power = request.Map.InitialPower,
                    Rotation = request.Map.InitialRotation,
                    X = request.Map.InitialX,
                    Y = request.Map.InitialY,
                    HorizontalSpeed = request.Map.InitialHorizontalSpeed,
                    VerticalSpeed = request.Map.InitialVerticalSpeed
                }
            };
            var successRates = new List<AiWeightSuccessRate>();

            // TODO: To method
            for (var weightIndex = 0; weightIndex < 4; weightIndex++)
            {
                for (var attempt = 0; attempt < request.UpwardTries; attempt++)
                {
                    for (var repeatingAttempts = 0; repeatingAttempts < 10; repeatingAttempts++)
                    {
                        double weight;
                        var aiWeight = request.OriginalWeights.Clone();
                        switch (weightIndex)
                        {
                            case 0:
                                weight = request.OriginalWeights.HorizontalSpeedWeight + request.ChangePerTry * attempt;
                                aiWeight.HorizontalSpeedWeight = weight;
                                break;
                            case 1:
                                weight = request.OriginalWeights.VerticalSpeedWeight + request.ChangePerTry * attempt;
                                aiWeight.VerticalSpeedWeight = weight;
                                break;
                            case 2:
                                weight = request.OriginalWeights.RotationWeight + request.ChangePerTry * attempt;
                                aiWeight.RotationWeight = weight;
                                break;
                            case 3:
                                weight = request.OriginalWeights.HorizontalDistanceWeight +
                                         request.ChangePerTry * attempt;
                                aiWeight.HorizontalDistanceWeight = weight;
                                break;
                        }

                        var evolution = new MarsLanderEvolution(environment, aiWeight);

                        var sw = Stopwatch.StartNew();
                        evolution.Run(request.Parameters.Generations, request.Parameters.Population, lander);
                        sw.Stop();
                        _logger.LogInformation("{elapsed}", sw.Elapsed);

                        var successRate = new AiWeightSuccessRate
                        {
                            AiWeight = aiWeight,
                            SuccessfulLandings = evolution.Generations.SelectMany(generation => generation.Actors)
                                .Count(actor => actor.Lander.Status == LanderStatus.Landed),
                            HighestNonLandingScore = evolution.Generations
                                .SelectMany(generation => generation.Actors)
                                .Select(actor => actor.Score)
                                .OrderBy(score => score)
                                .First(score => score > 0)
                        };

                        if (successRate.SuccessfulLandings > 0)
                        {
                            _logger.LogInformation("We got a winner!");
                            _logger.LogInformation("Weight: {@weight}", successRate.AiWeight);
                            _logger.LogInformation("Successes: {@successes}", successRate.SuccessfulLandings);
                            _logger.LogInformation("Generations: {@generations}", evolution.Generations.Count);
                        }
                        _logger.LogInformation("Highest Scoring non-success: {@score}",
                            successRate.HighestNonLandingScore);

                        successRates.Add(successRate);
                    }
                }

                for (var attempt = 0; attempt < request.DownwardTries; attempt++)
                {
                    for (var repeatingAttempts = 0; repeatingAttempts < 10; repeatingAttempts++)
                    {
                        double weight;
                        var aiWeight = request.OriginalWeights.Clone();
                        switch (weightIndex)
                        {
                            case 0:
                                weight = request.OriginalWeights.HorizontalSpeedWeight + request.ChangePerTry * attempt;
                                aiWeight.HorizontalSpeedWeight = weight;
                                break;
                            case 1:
                                weight = request.OriginalWeights.VerticalSpeedWeight + request.ChangePerTry * attempt;
                                aiWeight.VerticalSpeedWeight = weight;
                                break;
                            case 2:
                                weight = request.OriginalWeights.RotationWeight + request.ChangePerTry * attempt;
                                aiWeight.RotationWeight = weight;
                                break;
                            case 3:
                                weight = request.OriginalWeights.HorizontalDistanceWeight +
                                         request.ChangePerTry * attempt;
                                aiWeight.HorizontalDistanceWeight = weight;
                                break;
                        }

                        var evolution = new MarsLanderEvolution(environment, aiWeight);

                        var sw = Stopwatch.StartNew();
                        evolution.Run(request.Parameters.Generations, request.Parameters.Population, lander);
                        sw.Stop();
                        _logger.LogInformation("{elapsed}", sw.Elapsed);

                        var successRate = new AiWeightSuccessRate
                        {
                            AiWeight = aiWeight,
                            SuccessfulLandings = evolution.Generations.SelectMany(generation => generation.Actors)
                                .Count(actor => actor.Lander.Status == LanderStatus.Landed),
                            HighestNonLandingScore = evolution.Generations.SelectMany(generation => generation.Actors)
                                .Select(actor => actor.Score).OrderBy(score => score).First()
                        };

                        _logger.LogInformation("Weight: {weight}", successRate.AiWeight);
                        _logger.LogInformation("Successes: {successes}", successRate.SuccessfulLandings);
                        _logger.LogInformation("Highest Scoring non-success: {score}",
                            successRate.HighestNonLandingScore);

                        successRates.Add(successRate);
                    }
                }
            }

            return successRates;
        }
    }
}