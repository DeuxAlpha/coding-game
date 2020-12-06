using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.Models.Dtos;

namespace CodinGame.Application.Models
{
    public class LandRequest
    {
        public Map Map { get; set; }
        public AiWeight AiWeight { get; set; }
        public EvolutionParameters Parameters { get; set; }
    }
}