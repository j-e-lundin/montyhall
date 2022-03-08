namespace Simulation.MontyHall.API.Models
{
    public class SimulationQueryModel
    {
        public int? Simulations { get; set; }
        
        public bool? ChangeDoors { get; set; }

        
        public bool IsValid(out string propertyName)
        {
            propertyName = string.Empty;
            if (!Simulations.HasValue || Simulations <= 0 || Simulations > Int32.MaxValue)
            {
                propertyName = nameof(Simulations).ToLower();
                return false;
            }
            else if (!ChangeDoors.HasValue)
            {
                propertyName = nameof(ChangeDoors).ToLower();
                return false;
            }
            return true;
        }
    }
}
