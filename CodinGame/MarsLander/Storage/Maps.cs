using System.Collections;
using System.Collections.Generic;
using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.Models.Dtos;

namespace CodinGame.MarsLander.Storage
{
    public static class Maps
    {
        public static IEnumerable<Map> Get()
        {
            return new List<Map>()
            {
                new Map
                {
                    Name = "Easy on the right",
                    SurfaceZones = new[]
                    {
                        new SurfaceZone (0, 100, 1000, 500),
                        new SurfaceZone (1000, 500, 1500, 1000),
                        new SurfaceZone (1500, 1000, 4000, 150),
                        new SurfaceZone (4000, 150, 5500, 150),
                        new SurfaceZone (5500, 150, 6999, 800),
                    },
                    InitialX = 2500,
                    InitialY = 2700,
                    InitialHorizontalSpeed = 0,
                    InitialVerticalSpeed = 0,
                    InitialFuel = 550,
                    InitialRotation = 0,
                    InitialPower = 0
                },
                new Map
                {
                    Name = "Initial speed, correct Side",
                    SurfaceZones = new[]
                    {
                        new SurfaceZone(0, 100, 1000, 500),
                        new SurfaceZone(1000, 500, 1500, 100),
                        new SurfaceZone(1500, 100, 3000, 100),
                        new SurfaceZone(3000, 100, 3500, 500),
                        new SurfaceZone(3500, 500, 3700, 200),
                        new SurfaceZone(3700, 200, 5000, 1500),
                        new SurfaceZone(5000, 1500, 5800, 300),
                        new SurfaceZone(5800, 300, 6000, 1000),
                        new SurfaceZone(6000, 1000, 6999, 2000),
                    },
                    InitialX = 6500,
                    InitialY = 2800,
                    InitialHorizontalSpeed = -100,
                    InitialVerticalSpeed = 0,
                    InitialFuel = 600,
                    InitialRotation = 90,
                    InitialPower = 0
                },
                new Map
                {
                    Name = "Initial speed, wrong side",
                    SurfaceZones = new[]
                    {
                        new SurfaceZone(0, 100, 1000, 500),
                        new SurfaceZone(1000, 500, 1500, 1500),
                        new SurfaceZone(1500, 1500, 3000, 1000),
                        new SurfaceZone(3000, 1000, 4000, 150),
                        new SurfaceZone(4000, 150, 5500, 150),
                        new SurfaceZone(5500, 150, 6999, 800),
                    },
                    InitialX = 6500,
                    InitialY = 2800,
                    InitialHorizontalSpeed = -90,
                    InitialVerticalSpeed = 0,
                    InitialFuel = 750,
                    InitialRotation = 90,
                    InitialPower = 0
                },
                new Map
                {
                    Name = "Deep Canyon",
                    SurfaceZones = new[]
                    {
                        new SurfaceZone(0, 1000, 300, 1500),
                        new SurfaceZone(300, 1500, 350, 1400),
                        new SurfaceZone(350, 1400, 500, 2000),
                        new SurfaceZone(500, 2000, 800, 1800),
                        new SurfaceZone(800, 1800, 1000, 2500),
                        new SurfaceZone(1000, 2500, 1200, 2100),
                        new SurfaceZone(1200, 2100, 1500, 2400),
                        new SurfaceZone(1500, 2400, 2000, 1000),
                        new SurfaceZone(2000, 1000, 2200, 500),
                        new SurfaceZone(2200, 500, 2500, 100),
                        new SurfaceZone(2500, 100, 2900, 800),
                        new SurfaceZone(2900, 800, 3000, 500),
                        new SurfaceZone(3000, 500, 3200, 1000),
                        new SurfaceZone(3200, 1000, 3500, 2000),
                        new SurfaceZone(3500, 2000, 3800, 800),
                        new SurfaceZone(3800, 800, 4000, 200),
                        new SurfaceZone(4000, 200, 5000, 200),
                        new SurfaceZone(5000, 200, 5500, 1500),
                        new SurfaceZone(5500, 1500, 6999, 2800),
                    },
                    InitialX = 500,
                    InitialY = 2700,
                    InitialHorizontalSpeed = 100,
                    InitialVerticalSpeed = 0,
                    InitialFuel = 800,
                    InitialRotation = -90,
                    InitialPower = 0
                },
                new Map
                {
                    Name = "High Ground",
                    SurfaceZones = new[]
                    {
                        new SurfaceZone(0, 1000, 300, 1500),
                        new SurfaceZone(300, 1500, 350, 1400),
                        new SurfaceZone(350, 1400, 500, 2100),
                        new SurfaceZone(500, 2100, 1500, 2100),
                        new SurfaceZone(1500, 2100, 2000, 200),
                        new SurfaceZone(2000, 200, 2500, 500),
                        new SurfaceZone(2500, 500, 2900, 300),
                        new SurfaceZone(2900, 300, 3000, 200),
                        new SurfaceZone(3000, 200, 3200, 1000),
                        new SurfaceZone(3200, 1000, 3500, 500),
                        new SurfaceZone(3500, 500, 3800, 500),
                        new SurfaceZone(3800, 800, 4000, 200),
                        new SurfaceZone(4000, 200, 4200, 800),
                        new SurfaceZone(4200, 800, 4800, 800),
                        new SurfaceZone(4800, 600, 5000, 1200),
                        new SurfaceZone(5000, 1200, 5500, 900),
                        new SurfaceZone(5500, 900, 6000, 500),
                        new SurfaceZone(6000, 500, 6500, 300),
                        new SurfaceZone(6500, 300, 6999, 500),
                    },
                    InitialX = 6500,
                    InitialY = 2700,
                    InitialHorizontalSpeed = -50,
                    InitialVerticalSpeed = 0,
                    InitialFuel = 1000,
                    InitialRotation = 90,
                    InitialPower = 0
                }
            };
        }
    }
}