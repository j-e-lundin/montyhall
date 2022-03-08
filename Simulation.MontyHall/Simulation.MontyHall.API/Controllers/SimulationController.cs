using Microsoft.AspNetCore.Mvc;
using Simulation.MontyHall.API.Models;

namespace Simulation.MontyHall.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly ISimulation _simulation;
        private readonly ILogger<SimulationController> _logger;

        public SimulationController(ILogger<SimulationController> logger, ISimulation simulation)
        {
            _logger = logger;
            _simulation = simulation;
        }


        [HttpGet(Name = "Simulate")]
        [ProducesResponseType(typeof(SimulationResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromQuery] SimulationQueryModel query)
        {
            if (!query.IsValid(out var paramaterName))
            {
                _logger.LogError("Invalid query paramater '{0}'", paramaterName);
                return BadRequest($"Invalid query paramater '{paramaterName}'");
            }

            var wins = Enumerable.Range(0, query.Simulations.Value)
                .Sum(i => _simulation.Simulate(Random.Shared.Next(1, 4), Random.Shared.Next(1, 4), query.ChangeDoors.Value));

            var result = new SimulationResponseModel
            {
                Losses = query.Simulations.Value - wins,
                Wins = wins,
                Simulations = query.Simulations.Value,
                WinPercentage = ((float)wins / query.Simulations.Value) * 100f
            };

            LogSimulationResult(result);
            return Ok(result);
        }

        private void LogSimulationResult(SimulationResponseModel result)
        {
            var message = string.Format("Simulation result: Wins {0}, Losses {1}, Winning percentage {2}, Simulations {3}",
                result.Wins, result.Losses, result.WinPercentage, result.Simulations);

            _logger.LogInformation(message);
        }
    }
}