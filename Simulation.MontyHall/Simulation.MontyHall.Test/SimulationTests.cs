using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Simulation.MontyHall.Test
{
    public class SimulationTests
    {
        [Theory]
        [InlineData(2, 1, true, 1)]
        [InlineData(3, 1, true, 1)]
        [InlineData(1, 2, true, 1)]
        [InlineData(2, 2, false, 1)]
        [InlineData(1, 1, false, 1)]
        [InlineData(1, 1, true, 0)]
        [InlineData(2, 1, false, 0)]
        [InlineData(3, 2, false, 0)]
        public void Simulation_Returns_Expected_Result(int playerDoor, int prizeDoor, bool changeDoors, int expectedResult)
        {
            ISimulation sut = new MontyHallSimulation();
            var resuilt = sut.Simulate(playerDoor, prizeDoor, changeDoors);

            resuilt.Should().Be(expectedResult);
        }


        [Theory]
        [InlineData(100000)]
        [InlineData(1000000)]
        [InlineData(10000000)]
        public void Given_Large_Number_Of_Simulations_WinPercentage_Is_In_Expected_Range_When_ChangingDoors(int simulations)
        {
            ISimulation sut = new MontyHallSimulation();

            int wins = Enumerable.Range(0, simulations)
                .Sum(i => sut.Simulate(Random.Shared.Next(1, 4), Random.Shared.Next(1, 4), changeDoor: true));

            var winPercentage = ((float)wins / simulations) * 100f;
            // Roughly 2/3, give or take.Stabilzes at ~ 66,7 as the number of simulations grow
            winPercentage.Should().BeInRange(66f, 68f);

        }

        [Theory]
        [InlineData(100000)]
        [InlineData(1000000)]
        [InlineData(10000000)]
        public void Given_Large_Number_Of_Simulations_WinPercentage_Is_In_Expected_Range_When_Not_ChangingDoors(int simulations)
        {
            ISimulation sut = new MontyHallSimulation();

            int wins = Enumerable.Range(0, simulations)
                .Sum(i => sut.Simulate(Random.Shared.Next(1, 4), Random.Shared.Next(1, 4), changeDoor: false));

            var winPercentage = ((float)wins / simulations) * 100f;
            // Roughly 1/3, give or take. Stabilzes at ~ 33,3 as the number of simulations grow
            winPercentage.Should().BeInRange(33f, 34f);

        }

    }
}