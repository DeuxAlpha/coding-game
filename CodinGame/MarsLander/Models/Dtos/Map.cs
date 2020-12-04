using System.Collections;
using System.Collections.Generic;

namespace CodinGame.MarsLander.Models.Dtos
{
    public class Map
    {
        public string Name { get; set; }
        public IEnumerable<SurfaceElement> SurfaceElements { get; set; }
        public int InitialX { get; set; }
        public int InitialY { get; set; }
        public int InitialHorizontalSpeed { get; set; }
        public int InitialVerticalSpeed { get; set; }
        public int InitialFuel { get; set; }
        public int InitialRotation { get; set; }
        public int InitialPower { get; set; }
    }
}