using System.Collections;
using System.Collections.Generic;

namespace CodinGame.MarsLander.Models
{
    public class Generation
    {
        public List<GenerationActor> Actors { get; set; }
        public int GenerationNumber { get; set; }
    }
}