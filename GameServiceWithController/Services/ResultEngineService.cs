using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace GameServiceWithController.Services
{
    public class ResultEngineService :IResultEngineService
    {
        private ILogger<ResultEngineService> _logger;

        public ResultEngineService(ILogger<ResultEngineService> logger)
        {
            _logger = logger;
        }
        public  MoveType CompareMoves(List<MoveType> types)
        {
            MoveType winnerMove = types[0];
            for (int i = 0; i < types.Count; i++)
            {
                winnerMove = CompareMoves(winnerMove, types[i]);
            }
            return winnerMove;
        }

        public  MoveType CompareMoves(MoveType x, MoveType y)
        {
            if ((x == MoveType.ROCK && y == MoveType.PAPER) || (y == MoveType.ROCK && x == MoveType.PAPER))
                return MoveType.PAPER;
            else if ((x == MoveType.PAPER && y == MoveType.SCISSOR) || (x == MoveType.SCISSOR && y == MoveType.PAPER))
                return MoveType.SCISSOR;
            else if ((x == MoveType.SCISSOR && y == MoveType.ROCK) || (y == MoveType.SCISSOR && x == MoveType.ROCK))
                return MoveType.ROCK;
            else
                return x;
        }
    }
}
