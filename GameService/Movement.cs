using System;
using System.Collections.Generic;
using static GameService.IMovement;

namespace GameService
{
    public class Movement : IMovement
    {
        public MoveType MoveType { get; set; }
        public static string ChooseMoveTypeInformation { get; set; } = $"Choose Move Type {GetAllMoveTypes()} ";

        public Movement(MoveType type)
        {
           MoveType = type;
        }
        public static string GetAllMoves()
        {
            return string.Join(",", Enum.GetNames(typeof(MoveType)));
        }
        public static string GetAllMoveTypes()
        {
            return string.Join(",", Enum.GetNames(typeof(MoveType)));
        }

        public static bool IsValidMoveType(string value)
        {
            return Enum.IsDefined(typeof(MoveType), value.ToUpper());
        }
        public static MoveType GetMoveType(string value)
        {
            return (MoveType)Enum.Parse(typeof(MoveType), value.ToUpper());
        }
        public static  MoveType GetRandomMove()
        {
            MoveType[] values = (MoveType[])Enum.GetValues(typeof(MoveType));
            return values[new Random().Next(0, values.Length)];
        }
        public static MoveType CompareMoves(List<MoveType> types)
        {
            MoveType winnerMove = types[0];
            for (int i = 0; i < types.Count; i++)
            {
                winnerMove = CompareMoves(winnerMove, types[i]);
            }
            return winnerMove;
        }
        public static MoveType CompareMoves(MoveType x, MoveType y)
        {
            if ((x == MoveType.ROCK && y == MoveType.PAPER) || (y == MoveType.ROCK && x == MoveType.PAPER))
                return MoveType.PAPER;
            else if ((x == MoveType.PAPER && y == MoveType.SCISSOR) || (x== MoveType.SCISSOR && y == MoveType.PAPER))
                return MoveType.SCISSOR;
            else if ((x == MoveType.SCISSOR && y == MoveType.ROCK)|| (y == MoveType.SCISSOR && x == MoveType.ROCK))
                return MoveType.ROCK;
            else
                return x;
        }
    }
}
