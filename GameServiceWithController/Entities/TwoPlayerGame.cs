
using GameServiceWithController.Exceptions;
using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServiceWithController.Entities
{
    public class TwoPlayerGame : IGame,ITwoPlayerGame
    {
        public Guid GameId { get; }
        public event EventHandler GameFinished;
        public bool IsDraw { get; private set; } = false;
              
        private IResultEngineService _resultEngineService { get; set; }
        public GameType GameType { get; set; }
        public IPlayerMove PlayerA { get ; set ; }
        public IPlayerMove PlayerB { get ; set ; }
        public List<IPlayerMove> WinnerOfGame { get; set; }

        public TwoPlayerGame(IResultEngineService resultEngineService)
        {
            GameId = Guid.NewGuid();
            GameType = GameType.TWOPLAYER;
            IsDraw = false;
            _resultEngineService = resultEngineService;
            PlayerA =new PlayerMove();
            PlayerB = new PlayerMove(); 
            WinnerOfGame = new List<IPlayerMove>();
        }

        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return (PlayerA != null && PlayerA.Name == name  && PlayerA.Type == playerType || PlayerB !=null && PlayerB.Name == name && PlayerB.Type == playerType) ;
        }
        public void AddPlayertoGame(IPlayerMove playerA, IPlayerMove playerB)
        {
            PlayerA = new PlayerMove(playerA.PlayerId, playerA.Name, playerA.Type);
            PlayerB = new PlayerMove(playerB.PlayerId, playerB.Name, playerB.Type);
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
                WinnerOfGame.Add(PlayerA);
                WinnerOfGame.Add(PlayerB);
            }
            else
            {
                MoveType winnerMove = _resultEngineService.CompareMoves(PlayerA.MovementType, PlayerB.MovementType);
                if (PlayerA.MovementType == winnerMove)
                {
                    PlayerA.Score += 1;
                    WinnerOfGame.Add(PlayerA);
                }
                else
                {
                    PlayerB.Score += 1;
                    WinnerOfGame.Add(PlayerB);
                }
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

        public void SetMoveForPlayer(Guid playerId, MoveType moveType)
        {
            if (PlayerA.PlayerId == playerId)
                PlayerA.MovementType = moveType;
            else if (PlayerB.PlayerId == playerId)
                PlayerB.MovementType = moveType;
            else
                throw new MatchException("Player move could not be set");
        }

       
        public void SetMoveForPlayer(string playerName, PlayerType playerType ,MoveType moveType)
        {
            if (PlayerA.Name == playerName && PlayerA.Type == playerType)
                PlayerA.MovementType = moveType;
            else if (PlayerB.Name == playerName && PlayerB.Type == playerType)
                PlayerB.MovementType = moveType;
            else
                throw new MatchException("Player move could not be set");
        }
    }


}
