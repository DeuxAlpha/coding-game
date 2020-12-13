using System.Collections;
using System.Collections.Generic;
using CodinGame.GreatEscape.v2.Models;

namespace CodinGame.GreatEscape.v2
{
    public class GreatEscapeActor
    {
        public IEnumerable<Dragon> Players { get; set; }
        public double Score { get; set; }

        private readonly GreatEscapeEnvironment _environment;

        public GreatEscapeActor(GreatEscapeEnvironment environment)
        {
            _environment = environment;
        }
    }
}