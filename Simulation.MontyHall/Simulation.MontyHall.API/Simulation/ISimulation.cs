
namespace Simulation
{
    public interface ISimulation
    {
        public int Simulate(int playerDoor, int prizeDoor, bool changeDoor);
    }

    public class MontyHallSimulation : ISimulation
    {
        private readonly int[] _doorNumbers = new[] { 1, 2, 3 };

        public int Simulate(int playerDoor, int prizeDoor, bool changeDoor)
        {

            var playerWins = IsWinningDoorPicked(playerDoor, prizeDoor, changeDoor);
            return Convert.ToInt32(playerWins);
        }

        /*
         * Simplified algorithm to determine which door is to be picked
        */
        private bool IsWinningDoorPicked(int playerDoor, int prizeDoor, bool changeDoor)
        {

            if (changeDoor)
            {
                //Opened door cannot be player door or door containing the prize!
                int doorToOpen = _doorNumbers.FirstOrDefault(door => door != playerDoor && door != prizeDoor);

                playerDoor = _doorNumbers.SingleOrDefault(index => index != doorToOpen && index != playerDoor);
            }
            return playerDoor == prizeDoor;
        }
    }
}