namespace Simulation.MontyHall.API.Models
{
    public class SimulationResponseModel
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Simulations { get; set; }
        public float WinPercentage { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is SimulationResponseModel model &&
                   Wins == model.Wins &&
                   Losses == model.Losses &&
                   Simulations == model.Simulations &&
                   WinPercentage == model.WinPercentage;
        }

    }
}
