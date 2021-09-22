using Gameservice;
using GameService.Entities;
using GameService.Interfaces;
using GameService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameService
{
    public class Match :IMatch
    {
        public const int GAMESINMATCH = 3;
        public const int PLAYERSINMATCH = 2;

        public static string StartMatchInformation { get; set; } = $"Lets start the Match. Match Has {GAMESINMATCH} Games";
        public List<IPlayerMove> Players { get; set; }
        public List<IPlayerMove> WinnerOfMatch { get;  set; }
        public List<IGame> Games { get; set ; }

        private readonly ILogger<Match> _logger;
        private readonly IGameFactory _gameService;

        public Match(IGameFactory gameFactory, ILogger<Match> logger)
        {
            _gameService = gameFactory;
            _logger = logger;
        }
       
       
        public void EndMatch()
        {
            int topScore = Players.Max(x => x.Score);
            WinnerOfMatch= Players.Where(x => x.Score == topScore).ToList();
        }

      
        private void Game_GameFinished(object sender, EventArgs e)
        {
            IGame game = (IGame)sender;
            List<IPlayerMove> playersList = _gameService.GetWinnerOfGame(game);
            foreach(IPlayerMove player in Players)
            {
                if(playersList.Any(x=>x.Name == player.Name && x.Type == player.Type))
                {
                    player.Score += 1;
                }
            }
        }

        public bool CanMatchBeStarted()
        {
            if (Players.Count == PLAYERSINMATCH)
                return true;
            return false;
        }

        public void StartMatch()
        {
            _gameService.GenerateGame(GAMESINMATCH,GameType.TWOPLAYER);
            _logger.LogInformation($"Match Initalized with  {GAMESINMATCH} rounds");
        }

        public void StartGame(IGame game)
        {
            game.GameFinished += Game_GameFinished;
            game.StartGame(Players);
        }


        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return Players.Any(x => x.Name == name && x.Type == playerType);
        }
        public void AddPlayertoMatch(string name, PlayerType playerType)
        {
            Players.Add(new PlayerMove(name, playerType));
        }
        
    }
}
