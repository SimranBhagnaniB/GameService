using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameServiceWithController.Services
{
    public class MovementService :IMovementFactory
    {
        private ILogger<MovementService> _logger;

        public MovementService( ILogger<MovementService> logger)
        {
            _logger = logger;
        }
        public MoveType GetRandomMove()
        {
            MoveType[] values = (MoveType[])Enum.GetValues(typeof(MoveType));
            return values[new Random().Next(0, values.Length)];
        }
    }
}
