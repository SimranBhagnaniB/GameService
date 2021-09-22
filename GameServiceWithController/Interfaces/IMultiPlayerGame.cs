using GameServiceWithController.Models;
using System.Collections.Generic;

namespace GameServiceWithController.Interfaces
{
    public interface IMultiPlayerGame
    {
        List<IPlayerMove> PlayersOfGame { get; set; }
        List<IPlayerMove> WinnerOfGame { get; }

        void AddPlayerToGame(IPlayerMove player);
        void AddPlayertoGame(string name, PlayerType playerType);
        bool DoesPlayerExist(string name, PlayerType playerType);
        void SetMoveForPlayer(IPlayerMove player, MoveType move);
    }
}
