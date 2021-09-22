
using GameServiceWithController.Models;
using System.Collections.Generic;

namespace GameServiceWithController.Interfaces
{
    public interface ITwoPlayerGame
    {
        public IPlayerMove PlayerA { get; set; }
        public IPlayerMove PlayerB { get; set; }
        public List<IPlayerMove> WinnerOfGame { get; set; }
        public void AddPlayertoGame(IPlayerMove playerA, IPlayerMove playerB);
        public void SetMoveForPlayerA(MoveType move);
        public void SetMoveForPlayerB(MoveType move);
    }
}
