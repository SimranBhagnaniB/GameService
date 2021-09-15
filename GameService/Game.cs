using Gameservice;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static GameService.IMovement;
using static GameService.IPlayer;

namespace GameService
{
    public class Game : IGame
    {
        public Guid GameId { get; }
        private readonly ConcurrentDictionary<Player, Movement> GameRecord;
        public event EventHandler GameFinished;
        public bool IsDraw { get; private set; } = false;
        public List<Player> WinnerOfGame { get; private set; }

        public Game(Guid id, List<Player> players)
        {
            GameId = id;
            GameRecord = new ConcurrentDictionary<Player, Movement>();
            IsDraw = false;
            WinnerOfGame = new List<Player>();
            AddPlayertoGame(players);
        }

        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return GameRecord.Keys.Any(x => x.Name == name && x.PlayerType == playerType);
        }
        private void AddPlayertoGame(List<Player> players)
        {
            foreach (Player player in players)
            {
               AddPlayertoGame(player.Name, player.PlayerType);
            }
        }
        public void AddPlayertoGame(string name, PlayerType playerType)
        {
            GameRecord.TryAdd(new Player(name, playerType), null);
        }
        public List<Player> GetPlayersofGame()
        {
            return GameRecord.Keys.ToList();
        }

        public void SetMoveForPlayer(Player player, MoveType move)
        {
            GameRecord.TryUpdate(player, new Movement(move), null);
        }

        public void EndGame()
        {
            UpdatePlayerScoreInGame();
            GameFinished?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateScoreForAllPlayersInGame()
        {
            foreach (Player player in GameRecord.Keys)
            {
                player.Score += 1;
                WinnerOfGame.Add(player);
            }
        }

        private void UpdateWinnerPlayerScoreInGame(MoveType winnerMove)
        {
            
            foreach (KeyValuePair<Player, Movement> record in GameRecord)
            {
                if (record.Value.MoveType == winnerMove)
                {
                    record.Key.Score = record.Key.Score + 1;
                    WinnerOfGame.Add(record.Key);
                }
            }

        }

        private void UpdatePlayerScoreInGame()
        {
            List<MoveType> types = GameRecord.Values.Select(x => x.MoveType).Distinct().ToList();
            IsDraw = types.Count == 1;
            if (IsDraw) UpdateScoreForAllPlayersInGame();
            else
            {
                MoveType winnerMove = Movement.CompareMoves(types);
                UpdateWinnerPlayerScoreInGame(winnerMove);
            }
            
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }
    }
}
