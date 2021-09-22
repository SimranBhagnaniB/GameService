
using GameServiceWithController.Models;
using System;
using System.Collections.Generic;

namespace GameServiceWithController.Interfaces
{
    public interface IMatch
    {
        public Guid MatchId { get; set; }
        public GameType GameType { get; set; }
        public List<IGame> Games { get; set; }
        public List<IPlayerMove> Players {get;set;}
        public void EndMatch();
        public Guid AddPlayertoMatch(string name, PlayerType playerType);
        bool DoesPlayerExist(string name, PlayerType playerType);
        List<IPlayerMove> WinnerOfMatch { get; set; }
      
    }
}
