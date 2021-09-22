using Gameservice;
using GameService.Interfaces;
using GameService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameService.Entities
{
    public class TwoPlayerGame : IGame,ITwoPlayerGame
    {
        public Guid GameId { get; }
        public event EventHandler GameFinished;
        public bool IsDraw { get; private set; } = false;
              
        private IResultEngineService _resultEngineService { get; set; }
        public GameType GameType { get; set; } = GameType.MULTIPLAYER;
        public IPlayerMove PlayerA { get ; set ; }
        public IPlayerMove PlayerB { get ; set ; }
        public IPlayerMove WinnerOfGame { get; set; }

        public TwoPlayerGame(IResultEngineService resultEngineService)
        {
            GameId = Guid.NewGuid();
            IsDraw = false;
            _resultEngineService = resultEngineService;
        }

        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return (PlayerA != null && PlayerA.Name == name  && PlayerA.Type == playerType || PlayerB !=null && PlayerB.Name == name && PlayerB.Type == playerType) ;
        }
        public void AddPlayertoGame(IPlayerMove playerA, IPlayerMove playerB)
        {
            PlayerA = new PlayerMove(playerA.Name, playerA.Type);
            PlayerB = new PlayerMove(playerB.Name, playerB.Type);
        }
       
         public void SetMoveForPlayerA(MoveType move)
        {
            PlayerA.MovementType = move;
        }
        public void SetMoveForPlayerB(MoveType move)
        {
            PlayerB.MovementType = move;
        }


        public void EndGame()
        {
            UpdatePlayerScoreInGame();
            GameFinished?.Invoke(this, EventArgs.Empty);
        }

       
        private void UpdatePlayerScoreInGame()
        {
            IsDraw = PlayerA.MovementType == PlayerB.MovementType;
             if (IsDraw)
            {
                PlayerA.Score += 1;
                PlayerB.Score += 1;
            }
            else
            {
                MoveType winnerMove = _resultEngineService.CompareMoves(PlayerA.MovementType, PlayerB.MovementType);
                if(PlayerA.MovementType == winnerMove) PlayerA.Score += 1;
                else PlayerB.Score += 1;
            }

        }

        public void StartGame(List<IPlayerMove> players)
        {
            AddPlayertoGame(players[0], players[1]);
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }


}
