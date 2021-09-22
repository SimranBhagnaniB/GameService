using GameServiceWithController.Entities;
using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace GameServiceWithController.Services
{
    public class GameService : IGameFactory
    {
        private IResultEngineService _resultEngineService { get; set; }

        private readonly ILogger<GameService> _logger;

        public GameService(IResultEngineService resultEngineService, ILogger<GameService> logger)
        {
            _resultEngineService = resultEngineService;
            _logger = logger;
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
            _logger.LogInformation($"Games generated");
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
                    results.AddRange(((ITwoPlayerGame)game).WinnerOfGame);
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
        public void StartGame(IGame game, List<IPlayerMove> players)
        {
            game.StartGame(players);
        }

        public void EndGame(IGame game)
        {
            game.EndGame();
        }
    }
}
