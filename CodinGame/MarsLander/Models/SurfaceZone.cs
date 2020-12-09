using System.Collections;
using System.Collections.Generic;
using CodinGame.Utilities.Maths;

namespace CodinGame.MarsLander.Models
{
    public class SurfaceZone
    {
        public int LeftX { get; set; }
        public int LeftY { get; set; }
        public int RightX { get; set; }
        public int RightY { get; set; }
        public double Angle { get; set; }
        public List<SurfaceElement> SurfaceElements { get; set; }

        public SurfaceZone()
        {

        }

        public SurfaceZone(int leftX, int leftY, int rightX, int rightY)
        {
            LeftX = leftX;
            LeftY = leftY;
            RightX = rightX;
            RightY = rightY;
            Angle = Trigonometry.GetAngle(LeftX, LeftY, RightX, RightY);
            SurfaceElements = new List<SurfaceElement>();
            for (var x = leftX; x < rightX; x++)
            {
                SurfaceElements.Add(new SurfaceElement {X = x, Y = LeftY + Trigonometry.GetNewYPosition(Angle, x - leftX)});
            }
        }
    }
}