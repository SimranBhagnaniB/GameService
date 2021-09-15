using Gameservice;
using System.Collections.Generic;
using static GameService.IPlayer;

namespace GameService
{
    public interface IMatch
    {
        void AddGameToMatch();
        void AddPlayertoMatch(string name, PlayerType playerType);
        bool DoesPlayerExist(string name, PlayerType playerType);
        List<Player> GetPlayersofMatch();
        string GetWinnerOfMatch();
    }
}
