using Gameservice;
using System.Collections.Generic;
using static GameService.IMovement;
using static GameService.IPlayer;

namespace GameService
{
    public interface IGame
    {
        void AddPlayertoGame(string name,PlayerType playerType);
        bool DoesPlayerExist(string name, PlayerType playerType);
        void SetMoveForPlayer(Player player, MoveType move);
        List<Player> GetPlayersofGame();
        List<Player> GetWinnerOfGame();
        void EndGame();
    }
}