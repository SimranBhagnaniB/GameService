

using GameServiceWithController.Models;
using System.Collections.Generic;

namespace GameServiceWithController.Interfaces
{
    public interface IGameFactory
    {
        public List<IGame> GenerateGame(int rounds, GameType gameType);

        public List<IPlayerMove> GetWinnerOfGame(IGame game);

        public void StartGame(IGame  game, List<IPlayerMove> players);
        public void EndGame(IGame game);
    }
}
