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

        public Game(Guid id, List<Player> players)
        {
            GameId = id;
            GameRecord = new ConcurrentDictionary<Player, Movement>();
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
                GameRecord.TryAdd(player, null);
            }
        }
        public void AddPlayertoGame(string name,PlayerType playerType)
        {
            GameRecord.TryAdd(new Player(name,playerType), null);
        }
        public List<Player> GetPlayersofGame()
        {
            return GameRecord.Keys.ToList();
        }

        public void SetMoveForPlayer(Player player, MoveType move)
        {
            GameRecord.TryUpdate(player, new Movement(move),null);
        }

        public void EndGame()
        {
            GameFinished?.Invoke(this, EventArgs.Empty);
        }
        public List<Player> GetWinnerOfGame()
        {
            List<MoveType> types = GameRecord.Values.Select(x => x.MoveType).ToList();
            MoveType winnerMove = Movement.CompareMoves(types);
            return (from record in GameRecord.ToList()
                    where record.Value.MoveType == winnerMove
                    select record.Key).ToList();
            
        }
       

    }
}
