
using GameService.Models;
using System;
using System.Collections.Generic;

namespace GameService.Interfaces
{
    public interface IGame
    {
        public Guid GameId { get; }
        GameType GameType { get; set; }
        bool IsDraw { get; }

        public event EventHandler GameFinished;
        void StartGame(List<IPlayerMove> players);

        void StartGame();
        void EndGame();
       
    }
}