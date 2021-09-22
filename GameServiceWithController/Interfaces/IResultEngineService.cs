using GameServiceWithController.Models;
using System.Collections.Generic;


namespace GameServiceWithController.Interfaces
{
    public interface IResultEngineService
    {
        MoveType CompareMoves(List<MoveType> types);
        MoveType CompareMoves(MoveType x, MoveType y);
    }
}
