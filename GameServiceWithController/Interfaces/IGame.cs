
using GameServiceWithController.Models;
using System;
using System.Collections.Generic;

namespace GameServiceWithController.Interfaces
{
    public interface IGame
    {
        public Guid GameId { get; }
        GameType GameType { get; set; }
        bool IsDraw { get; }
        public void SetMoveForPlayer(Guid playerId,MoveType moveType);
        public void SetMoveForPlayer(string name, PlayerType type, MoveType moveType);

        public event EventHandler GameFinished;
        void StartGame(List<IPlayerMove> players);

        public void StartGame();
        public void EndGame();
    }
}