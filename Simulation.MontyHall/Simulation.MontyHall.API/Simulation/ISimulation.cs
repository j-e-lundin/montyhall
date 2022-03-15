
using System.ComponentModel.DataAnnotations;

namespace Simulation
{
    public interface ISimulation<T> where T : ISimulationParamaters
    {
        public int Simulate(T paramaters);
    }

    public class MontyHallSimulation : ISimulation<MontyHallParamaters>
    {
        private readonly int[] _doorNumbers = new[] { 1, 2, 3 };

        public int Simulate(MontyHallParamaters paramaters) { 
            
            var playerWins = IsWinningDoorPicked(paramaters);
            return Convert.ToInt32(playerWins);
        }

        
        /*
         * Simplified algorithm to determine which door is to be picked
        */
        private bool IsWinningDoorPicked(MontyHallParamaters paramaters)
        {

            if (paramaters.ChangeDoor)
            {
                //Opened door cannot be player door or door containing the prize!
                int doorToOpen = _doorNumbers.FirstOrDefault(door => door != paramaters.PlayerDoor && door != paramaters.PrizeDoor);

                paramaters.PlayerDoor = _doorNumbers.SingleOrDefault(index => index != doorToOpen && index != paramaters.PlayerDoor);
            }
            return paramaters.PlayerDoor==paramaters.PrizeDoor;
        }
    }


    //Dummy interface to set Generic constraints
    public interface ISimulationParamaters
    {

    }
    public class MontyHallParamaters : ISimulationParamaters
    {
        public int PlayerDoor { get; set; }
        public int PrizeDoor { get; set; }
        public bool ChangeDoor { get; set; }
    }
}