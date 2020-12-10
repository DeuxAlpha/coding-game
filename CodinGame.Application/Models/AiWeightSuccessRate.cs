using CodinGame.MarsLander.Models;

namespace CodinGame.Application.Models
{
    public class AiWeightSuccessRate
    {
        public AiWeight AiWeight { get; set; }
        public int SuccessfulLandings { get; set; }
        public double HighestNonLandingScore { get; set; }
    }
}