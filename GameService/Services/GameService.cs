using GameService.Entities;
using GameService.Interfaces;
using GameService.Models;
using System.Collections.Generic;


namespace GameService.Services
{
    public class GameService : IGameFactory
    {
        private IResultEngineService _resultEngineService { get; set; }
        public GameService(IResultEngineService resultEngineService)
        {
            _resultEngineService = resultEngineService;
        }
        public List<IGame> GenerateGame(int rounds, GameType gameType)
        {
            List<IGame> games  = new List<IGame>();
            switch (gameType)
            {
                case GameType.MULTIPLAYER:
                    return GetMultiPlayerGames(rounds, games);
                case GameType.TWOPLAYER:
                    return GetTwoPlayerGames(rounds, games);
                case GameType.SINGLEPLAYER:
                    break;
                default:
                    break;
            }
            return games;
        }

        public List<IPlayerMove> GetWinnerOfGame(IGame game)
        {
            List<IPlayerMove> results = new List<IPlayerMove>();
            switch(game.GameType)
            {
                case GameType.MULTIPLAYER:
                    results.AddRange(((IMultiPlayerGame)game).WinnerOfGame);
                    break;
                case GameType.TWOPLAYER:
                    results.Add(((ITwoPlayerGame)game).WinnerOfGame);
                    break;

            }
            return results;
        }

        private List<IGame> GetTwoPlayerGames(int rounds, List<IGame> games)
        {
            for (int i = 0; i < rounds; i++)
            {
                games.Add(new TwoPlayerGame(_resultEngineService));
            }
            return games;
        }

        private List<IGame> GetMultiPlayerGames(int rounds, List<IGame> games)
        {
            for (int i = 0; i < rounds; i++)
            {
                games.Add(new MultiPlayerGame(_resultEngineService));
            }
            return games;
        }
    }
}
