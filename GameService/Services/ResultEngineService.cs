using GameService.Interfaces;
using GameService.Models;
using System.Collections.Generic;

namespace GameService.Services
{
    public class ResultEngineService :IResultEngineService
    {
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
