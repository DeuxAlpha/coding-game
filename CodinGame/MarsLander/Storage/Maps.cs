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
                    SurfaceElements = new[]
                    {
                        new SurfaceElement {X = 0, Y = 100},
                        new SurfaceElement {X = 1000, Y = 500},
                        new SurfaceElement {X = 1500, Y = 1000},
                        new SurfaceElement {X = 4000, Y = 150},
                        new SurfaceElement {X = 5500, Y = 150},
                        new SurfaceElement {X = 6999, Y = 800}
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
                    SurfaceElements = new[]
                    {
                        new SurfaceElement {X = 0, Y = 100},
                        new SurfaceElement {X = 1000, Y = 500},
                        new SurfaceElement {X = 1500, Y = 100},
                        new SurfaceElement {X = 3000, Y = 100},
                        new SurfaceElement {X = 3500, Y = 500},
                        new SurfaceElement {X = 3700, Y = 200},
                        new SurfaceElement {X = 5000, Y = 1500},
                        new SurfaceElement {X = 5800, Y = 300},
                        new SurfaceElement {X = 6000, Y = 1000},
                        new SurfaceElement {X = 6999, Y = 2000},
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
                    SurfaceElements = new[]
                    {
                        new SurfaceElement {X = 0, Y = 100},
                        new SurfaceElement {X = 1000, Y = 500},
                        new SurfaceElement {X = 1500, Y = 1500},
                        new SurfaceElement {X = 3000, Y = 1000},
                        new SurfaceElement {X = 4000, Y = 150},
                        new SurfaceElement {X = 5500, Y = 150},
                        new SurfaceElement {X = 6999, Y = 800},
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
                    SurfaceElements = new[]
                    {
                        new SurfaceElement {X = 0, Y = 1000},
                        new SurfaceElement {X = 300, Y = 1500},
                        new SurfaceElement {X = 350, Y = 1400},
                        new SurfaceElement {X = 500, Y = 2000},
                        new SurfaceElement {X = 800, Y = 1800},
                        new SurfaceElement {X = 1000, Y = 2500},
                        new SurfaceElement {X = 1200, Y = 2100},
                        new SurfaceElement {X = 1500, Y = 2400},
                        new SurfaceElement {X = 2000, Y = 1000},
                        new SurfaceElement {X = 2200, Y = 500},
                        new SurfaceElement {X = 2500, Y = 100},
                        new SurfaceElement {X = 2900, Y = 800},
                        new SurfaceElement {X = 3000, Y = 500},
                        new SurfaceElement {X = 3200, Y = 1000},
                        new SurfaceElement {X = 3500, Y = 2000},
                        new SurfaceElement {X = 3800, Y = 800},
                        new SurfaceElement {X = 4000, Y = 200},
                        new SurfaceElement {X = 5000, Y = 200},
                        new SurfaceElement {X = 5500, Y = 1500},
                        new SurfaceElement {X = 6999, Y = 2800},
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
                    SurfaceElements = new[]
                    {
                        new SurfaceElement {X = 0, Y = 1000},
                        new SurfaceElement {X = 300, Y = 1500},
                        new SurfaceElement {X = 350, Y = 1400},
                        new SurfaceElement {X = 500, Y = 2100},
                        new SurfaceElement {X = 1500, Y = 2100},
                        new SurfaceElement {X = 2000, Y = 200},
                        new SurfaceElement {X = 2500, Y = 500},
                        new SurfaceElement {X = 2900, Y = 300},
                        new SurfaceElement {X = 3000, Y = 200},
                        new SurfaceElement {X = 3200, Y = 1000},
                        new SurfaceElement {X = 3500, Y = 500},
                        new SurfaceElement {X = 3800, Y = 800},
                        new SurfaceElement {X = 4000, Y = 200},
                        new SurfaceElement {X = 4200, Y = 800},
                        new SurfaceElement {X = 4800, Y = 600},
                        new SurfaceElement {X = 5000, Y = 1200},
                        new SurfaceElement {X = 5500, Y = 900},
                        new SurfaceElement {X = 6000, Y = 500},
                        new SurfaceElement {X = 6500, Y = 300},
                        new SurfaceElement {X = 6999, Y = 500},
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