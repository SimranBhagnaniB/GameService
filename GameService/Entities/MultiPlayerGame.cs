using Gameservice;
using GameService.Interfaces;
using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameService.Entities
{
    public class MultiPlayerGame : IGame, IMultiPlayerGame
    {
        public Guid GameId { get; internal set; }
      
        public bool IsDraw { get; private set; } = false;
        public List<IPlayerMove> WinnerOfGame { get; private set; }
        public List<IPlayerMove> PlayersOfGame { get; set; }
        private IResultEngineService _resultEngineService { get; set; }
        public GameType GameType { get; set; } = GameType.MULTIPLAYER;

        public MultiPlayerGame(IResultEngineService resultEngineService)
        {
            GameId = Guid.NewGuid();
            IsDraw = false;
            WinnerOfGame = new List<IPlayerMove>();
            PlayersOfGame = new List<IPlayerMove>();
            _resultEngineService = resultEngineService;
        }

        public event EventHandler GameFinished;

        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return PlayersOfGame.Any(x => x.Name == name && x.Type == playerType);
        }
        public void AddPlayertoGame(List<IPlayerMove> players)
        {
            PlayersOfGame = players;
        }
        public void AddPlayertoGame(string name, PlayerType playerType)
        {
            IPlayerMove player = new PlayerMove(name, playerType);
            PlayersOfGame.Add(player);
        }
        public List<IPlayerMove> GetPlayersofGame()
        {
            return PlayersOfGame.ToList();
        }

        public void SetMoveForPlayer(IPlayerMove player, MoveType move)
        {
            IPlayerMove  myplayer = PlayersOfGame.SingleOrDefault(x => x.Name == player.Name && x.Type == player.Type);
            if(myplayer != null)
            {
                myplayer.MovementType = move;
            }
        }

        public void EndGame()
        {
            UpdatePlayerScoreInGame();
            GameFinished?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateScoreForAllPlayersInGame()
        {
            foreach (IPlayerMove playerMove in PlayersOfGame)
            {
                playerMove.Score += 1;
                WinnerOfGame.Add(playerMove);
            }
        }

        private void UpdateWinnerPlayerScoreInGame(MoveType winnerMove)
        {
            foreach (var player in from IPlayerMove player in PlayersOfGame
                                   where player.MovementType == winnerMove
                                   select player)
            {
                player.Score += 1;
                WinnerOfGame.Add(player);
            }
        }

        private void UpdatePlayerScoreInGame()
        {
            List<MoveType> types = PlayersOfGame.Select(x => x.MovementType).Distinct().ToList();
            IsDraw = types.Count == 1;
            if (IsDraw) UpdateScoreForAllPlayersInGame();
            else
            {
                MoveType winnerMove = _resultEngineService.CompareMoves(types);
                UpdateWinnerPlayerScoreInGame(winnerMove);
            }
            
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }

        public void AddPlayerToGame(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void AddPlayerToGame(IPlayerMove player)
        {
            throw new NotImplementedException();
        }

        public void StartGame(List<IPlayerMove> players)
        {
            foreach(IPlayerMove player in players)
            {
                AddPlayertoGame(player.Name, player.Type);
            }
        }
    }

  
}
