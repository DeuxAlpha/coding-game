namespace CodinGame.Utilities.Maths.Enums
{
    public enum IntersectionType
    {
        /// <summary>The two lines intersect at exactly one point.</summary>
        Point,
        /// <summary>The two lines are collinear and overlapping.</summary>
        CollinearOverlapping,
        /// <summary>The two lines are collinear but disjoint, meaning that they don't actually connect (e.g. ---  ---).
        /// </summary>
        CollinearDisjoint,
        /// <summary>The two lines do not intersect and are parallel to each other.</summary>
        Parallel,
        /// <summary>The two lines neither intersect not are they parallel.</summary>
        None
    }
}