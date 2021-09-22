

using GameService.Models;
using System.Collections.Generic;

namespace GameService.Interfaces
{
    public interface IGameFactory
    {
        public List<IGame> GenerateGame(int rounds, GameType gameType);

        public List<IPlayerMove> GetWinnerOfGame(IGame game);
    }
}
