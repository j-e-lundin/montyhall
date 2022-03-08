
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Simulation.MontyHall.API.Controllers;
using Simulation.MontyHall.API.Models;
using Xunit;

namespace Simulation.MontyHall.Test
{
    public class SimulationControllerTests
    {
        private const string InvalidQueryParamaterMessage = "Invalid query paramater 'changedoors'";
        private const string ExpectedLogMEssage = "Simulation result: Wins 0, Losses 10, Winning percentage 0, Simulations 10";

        private static SimulationController? _controller;
        private static ISimulation _simulation = Substitute.For<ISimulation>();

        private static ILogger<SimulationController> _mockLogger = Substitute.For<ILogger<SimulationController>>();
        private readonly SimulationQueryModel _queryInvalidChangeDoors = new () { ChangeDoors = null, Simulations = 10 };

        public SimulationControllerTests()
        {

            _simulation.Simulate(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(0);
        }

        private static SimulationController MockController()
        {
            if (_controller is not { })
            {
                _controller = new SimulationController(_mockLogger, _simulation);
            }
            return _controller;

        }

        [Fact]
        public void Given_Valid_Query_Endpoint_Returns_OK_With_Correct_Return_Type()
        {
            SimulationResponseModel expectedResult = new () { Simulations = 10, Losses = 10, WinPercentage = 0, Wins = 0 };
            var sut = MockController();

            var result = sut.Get(new SimulationQueryModel() { ChangeDoors = true, Simulations = 10 }) ;
            
            //When multiple assertions are used, AssertionScope ensures that all assertions are run
            using (new AssertionScope())
            {
                result.Should().BeAssignableTo<OkObjectResult>();
                var okResult = result as OkObjectResult;
                okResult.Value.Should().NotBeNull().And.BeOfType<SimulationResponseModel>().And.BeXmlSerializable();
                okResult.Value.Equals(expectedResult).Should().BeTrue();
                okResult.StatusCode.Should().Be(200);
            }

        }

        [Fact]
        public void Given_InValid_Query_Endpoint_Returns_BadRequest_With_Correct_Return_Type()
        {
            var sut = MockController();

            var result = sut.Get(_queryInvalidChangeDoors);

            //When multiple assertions are used, AssertionScope ensures that all assertions are run
            using (new AssertionScope())
            {
                result.Should().BeAssignableTo<BadRequestObjectResult>();
                var badRequest = result as BadRequestObjectResult;
                badRequest.Value.Should().NotBeNull().And.BeOfType<string>().And.Be(InvalidQueryParamaterMessage);
                badRequest.StatusCode.Should().Be(400);
            }
        }

        [Fact]
        public void Given_InValid_Query_Controller_Logs_Error()
        {
            //Use MockLogger to instantiate Controller, needed for testing logging behaviour
            var mockLogger = Substitute.For<MockLogger<SimulationController>>();

            var sut = new SimulationController(mockLogger, _simulation);
            _ = sut.Get(_queryInvalidChangeDoors);

            mockLogger.Received().Log(LogLevel.Error, InvalidQueryParamaterMessage);
        }


        [Fact]
        public void Given_Valid_Query_Controller_Logs_Result_As_Information()
        {
            //Use MockLogger to instantiate Controller, needed for testing logging behaviour
            var mockLogger = Substitute.For<MockLogger<SimulationController>>();

            var sut = new SimulationController(mockLogger, _simulation);
            _ = sut.Get(new SimulationQueryModel() { ChangeDoors = true, Simulations = 10 });

            mockLogger.Received().Log(LogLevel.Information, ExpectedLogMEssage);
        }
    }
}
