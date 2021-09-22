using GameService.Models;
using System.Collections.Generic;


namespace GameService.Interfaces
{
    public interface IResultEngineService
    {
        MoveType CompareMoves(List<MoveType> types);
        MoveType CompareMoves(MoveType x, MoveType y);
    }
}
