using CodinGame.MarsLander.Models;
using CodinGame.MarsLander.Models.Dtos;

namespace CodinGame.Application.Models
{
    public class CalculateWeightRequest
    {
        public EvolutionParameters Parameters { get; set; }
        public Map Map { get; set; }
        public AiWeight OriginalWeights { get; set; }
        // Change to be tried up and down.
        public double ChangePerTry { get; set; }
        // Overall amount of tries into each direction.
        public int UpwardTries { get; set; }
        public int DownwardTries { get; set; }
    }
}