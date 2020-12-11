using System;
using CodinGame.Utilities.Extensions;
using CodinGame.Utilities.Maths.Enums;
using CodinGame.Utilities.Maths.Models;

namespace CodinGame.Utilities.Maths
{
    public static partial class Trigonometry
    {
        /// <summary>Test whether two line segments intersect. If so, calculate the intersection point.</summary>
        //  http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect/565282#565282
        //  http://stackoverflow.com/a/14143738/292237
        //  http://www.codeproject.com/Tips/862988/Find-the-Intersection-Point-of-Two-Line-Segments
        public static IntersectionPoint GetIntersection(Vector vector1, Vector vector2)
        {
            var p = vector1.Point1;
            var p2 = vector1.Point2;
            var q = vector2.Point1;
            var q2 = vector2.Point2;
            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (0 <= (q - p) * r && (q - p) * r <= r * r || 0 <= (p - q) * s && (p - q) * s <= s * s)
                    return new IntersectionPoint(IntersectionType.CollinearOverlapping);

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return new IntersectionPoint(IntersectionType.CollinearDisjoint);
            }

            // 3. If r x s = 0 and (q - p) x r != 0, then the two lines are parallel and non-intersecting.
            if (rxs.IsZero() && !qpxr.IsZero())
                return new IntersectionPoint(IntersectionType.Parallel);

            // t = (q - p) x s / (r x s)
            var t = (q - p).Cross(s) / rxs;

            // u = (q - p) x r / (r x s)
            var u = (q - p).Cross(r) / rxs;

            // 4. If r x s != 0 and 0 <= t <= 1 and 0 <= u <= 1
            // the two line segments meet at the point p + t r = q + u s.
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                var intersection = p + t * r;
                // An intersection was found.
                return new IntersectionPoint(intersection.X, intersection.Y, IntersectionType.Point);
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return new IntersectionPoint(IntersectionType.None);
        }

        /// <summary>Calculates the intersection of a line and a horizontal line at an arbitrary y-level.</summary>
        public static double GetXIntersect(Point point1, Point point2, double yLevel)
        {
            var adjustedY1 = point1.Y - yLevel;
            var adjustedY2 = point2.Y - yLevel;
            var aboveInterceptTravelPercentage = Math.Abs(adjustedY2) / (Math.Abs(adjustedY2) + Math.Abs(adjustedY1));
            var xIntercept = point1.X + (point2.X - point1.X) * aboveInterceptTravelPercentage;
            return xIntercept;
        }

        /// <summary>Calculates the intersection of a line and a vertical line at an arbitrary x-level.</summary>
        public static double GetYIntersect(Point point1, Point point2, double xLevel)
        {
            var adjustedX1 = point1.X - xLevel;
            var adjustedX2 = point2.X - xLevel;
            var aboveInterceptTravelPercentage = Math.Abs(adjustedX2) / (Math.Abs(adjustedX2) + Math.Abs(adjustedX1));
            var yIntercept = point1.Y + (point2.Y - point1.Y) * aboveInterceptTravelPercentage;
            return yIntercept;
        }
    }
}