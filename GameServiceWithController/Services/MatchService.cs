

using GameServiceWithController.Entities;
using GameServiceWithController.Exceptions;
using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServiceWithController.Services
{
    public class MatchService : IMatchFactory
    {
        private IGameFactory _gameService { get; set; }
        private readonly ILogger<MatchService> _logger;
        public ConcurrentDictionary<Guid, IMatch > _matches { get; internal set; }

        private readonly IMovementFactory _movementService;

        public MatchService(IGameFactory gameService, IMovementFactory movementService, ILogger<MatchService> logger)
        {
            _gameService = gameService;
            _logger = logger;
            _matches = new ConcurrentDictionary<Guid,IMatch>();
            _movementService = movementService;
        }
      

        public Guid NewMatchForGameType(GameType gameType)
        {
            IMatch match = new Match(gameType);
            _matches.TryAdd(match.MatchId,match);
            return match.MatchId;
        }

        public Guid AddPlayerToMatch(string matchId, string playerName, PlayerType type)
        {
            try
            {
                bool bSuccess = _matches.TryGetValue(Guid.Parse(matchId), out IMatch match);
                if (bSuccess)
                {
                    if (match.Players != null && match.Players.Count != 0)
                    {
                        if (!match.DoesPlayerExist(playerName, type))
                        {
                            return match.AddPlayertoMatch(playerName, type);
                        }
                        else
                        {
                            throw new MatchException("Player already exists");
                        }
                    }
                    else return match.AddPlayertoMatch(playerName, type);
                }
                else
                {
                    throw new MatchException("Match not found");
                }
                throw new MatchException("Failed to add Player to match");
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"AddPlayerToMatch failed:{e.Message}");
                throw new MatchException($"Failed to add Player to match:{e.Message}");
               
            }
            
        }

      
        public IMatch StartMatchWithTwoPlayersAndGameType(string matchId,string playerAName, string playerBName, PlayerType playerAType, PlayerType playerBType, GameType gameType )
        {
            try
            {
                List<IPlayerMove> players = new List<IPlayerMove>
            {
                new PlayerMove(playerAName, playerAType),
                new PlayerMove(playerBName, playerBType)
            };
                bool bSuccess = _matches.TryGetValue(Guid.Parse(matchId), out IMatch match);
                if (bSuccess)
                {
                    match.Players = players;
                }
                match.Games = _gameService.GenerateGame(3, gameType);
                return match;
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"StartMatch failed:{e.Message}");
                throw new MatchException($"Start Match Failed:{e.Message}");
            }
        }

        public List<Guid> StartMatchGetGames(string matchId)
        {
            List<Guid> results = new List<Guid>();
            bool bSuccess = _matches.TryGetValue(Guid.Parse(matchId), out IMatch match);
            if (bSuccess)
            {
                match.Games = _gameService.GenerateGame(3, match.GameType);
                results.AddRange(from IGame game in match.Games
                                 select game.GameId);
                return results;
            }
            else
            {
                throw new MatchException("Match not found");
            }
        }
        public List<Guid>  StartMatch(string matchId)
        {
            List<Guid> results = new List<Guid>();
            bool bSuccess = _matches.TryGetValue(Guid.Parse(matchId), out IMatch match);
            if (bSuccess)
            {
               match.Games =_gameService.GenerateGame(3, match.GameType);
               results.AddRange(from IGame game in match.Games
                                 select game.GameId);
               return results;
            }
            else
            {
                throw new MatchException("Match not found");
            }
        }

        private bool GetMatch(string matchId, out IMatch match)
        {
            match = null;
            bool bSuccess = _matches.TryGetValue(Guid.Parse(matchId), out IMatch currentMatch);
            if (bSuccess)
            {
                match = currentMatch;
            }
           return bSuccess;
        }

        private bool GetGame(IMatch match, string gameId, out IGame currentGame)
        {
            currentGame = null;
            bool bResult = false;
            List<IGame> games = match.Games;
            foreach(IGame game in games)
            {
                if (game.GameId.ToString() == gameId)
                {
                    currentGame = game;
                    bResult = true;
                }
            }

            return bResult;
            
        }

        public bool StartGameInMatch(string matchId, string gameId)
        {
            bool bSuccess = false;
            if(matchId != null || gameId != null)
            {
                if (GetMatch(matchId, out IMatch match))
                {
                    if(GetGame(match,gameId, out IGame game))
                    {
                        game.StartGame(match.Players);
                        game.GameFinished +=(sender,e)=> Game_GameFinished(sender,e,matchId);
                        bSuccess = true;
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Game cannot be started");
            }
            return bSuccess;
        }

        

        private void Game_GameFinished(object sender , EventArgs e , string matchId)
        {
            IGame game = (IGame)sender;
            List<IPlayerMove> playersList = null;

            switch (game.GameType)
            {
                case GameType.MULTIPLAYER:
                    playersList =  ((IMultiPlayerGame)game).WinnerOfGame;
                    break;
                case GameType.TWOPLAYER:
                    playersList= ((ITwoPlayerGame)game).WinnerOfGame;
                    break;


            }
            if (playersList != null)
            {
                GetMatch(matchId, out IMatch match);
                foreach (IPlayerMove player in match.Players)
                {
                    if (playersList.Any(x => x.Name == player.Name && x.Type == player.Type))
                    {
                        player.Score += 1;
                    }
                }
            }
        }
        public void EndMatch(string matchId)
        {
            GetMatch(matchId, out IMatch match);
            match.EndMatch();

        }
        public List<IPlayerMove> GetWinnerOfMatch(string matchId)
        {
            GetMatch(matchId, out IMatch match);
            return match.WinnerOfMatch;
        }
        public void SetAutomatedMoveForPlayerInGameInMatch(string matchId, string gameId, string playerName, PlayerType type)
        {
            if (matchId != null || gameId != null)
            {
                if (GetMatch(matchId, out IMatch match))
                {
                    if (GetGame(match, gameId, out IGame game))
                    {
                        game.SetMoveForPlayer(playerName,type, _movementService.GetRandomMove());
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Game cannot be started");
            }
        }

        public void SetAutomatedMoveForPlayerInGameInMatch(string matchId, string gameId, string playerId)
        {
            if (matchId != null || gameId != null)
            {
                if (GetMatch(matchId, out IMatch match))
                {
                    if (GetGame(match, gameId, out IGame game))
                    {
                        game.SetMoveForPlayer(Guid.Parse(playerId), _movementService.GetRandomMove());
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Game cannot be started");
            }
        }
       
        public void EndGameInMatch(string matchId, string gameId)
        {
            if (matchId != null || gameId != null)
            {
                if (GetMatch(matchId, out IMatch match))
                {
                    if (GetGame(match, gameId, out IGame game))
                    {
                        game.EndGame();
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Game cannot be started");
            }
        }

        public List<IPlayerMove> GetWinnerOfGameInMatch(string matchId, string gameId)
        {
            List<IPlayerMove> myPlayers = new List<IPlayerMove>();
            if (matchId != null || gameId != null)
            {

                if (GetMatch(matchId, out IMatch match))
                {
                    if (GetGame(match, gameId, out IGame game))
                    {
                        switch (game.GameType)
                        {
                            case GameType.MULTIPLAYER:
                                return ((IMultiPlayerGame)game).WinnerOfGame;
                                
                            case GameType.TWOPLAYER:
                                return ((ITwoPlayerGame)game).WinnerOfGame;


                        }
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Winner of Match not found");
            }
            return myPlayers;
        }
        public void SetMoveForPlayerInGameInMatch(string matchId, string gameId, string playerId, MoveType moveType)
        {
            if (matchId != null || gameId != null)
            {
                if (GetMatch(matchId, out IMatch match))
                {
                    if (GetGame(match, gameId, out IGame game))
                    {
                        game.SetMoveForPlayer(Guid.Parse(playerId), moveType);
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Game cannot be started");
            }
        }

        public void SetMoveForPlayerInGameInMatch(string matchId, string gameId, string playerName, PlayerType type, MoveType moveType)
        {
            if (matchId != null || gameId != null)
            {
                if (GetMatch(matchId, out IMatch match))
                {
                    if (GetGame(match, gameId, out IGame game))
                    {
                        game.SetMoveForPlayer(playerName,type, moveType);
                    }
                    else
                    {
                        throw new MatchException("Game not found");
                    }
                }
                else
                {
                    throw new MatchException("Match not found");
                }

            }
            else
            {
                throw new MatchException("Game cannot be started");
            }
        }

       
    }
}
