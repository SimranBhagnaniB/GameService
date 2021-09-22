using Gameservice;
using GameService.Models;
using System.Collections.Generic;

namespace GameService.Interfaces
{
    public interface IMatch
    {
        public List<IGame> Games { get; set; }
        public List<IPlayerMove> Players {get;set;}
        public void StartMatch();
        
        public void EndMatch();
        void AddPlayertoMatch(string name, PlayerType playerType);
        bool DoesPlayerExist(string name, PlayerType playerType);
        List<IPlayerMove> WinnerOfMatch { get; set; }


    }
}
