using Gameservice;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static GameService.IPlayer;

namespace GameService
{
    public class Match
    {
        public const int GAMESINMATCH = 3;
        public const int PLAYERSINMATCH = 2;


        public static string StartMatchInformation { get; set; } = $"Lets start the Match. Match Has {GAMESINMATCH} Games";
        private readonly List<Player> players;
        public ConcurrentDictionary<Game, Player> _matchRecord { get; set; }

        public Match()
        {
            _matchRecord = new ConcurrentDictionary<Game, Player>();
            players = new List<Player>();
        }
        private void IntializeMatch()
        {
            if (_matchRecord.Count == 0)
            {
                for (int i = 0; i < GAMESINMATCH; i++)
                {
                    Game game = new Game(Guid.NewGuid(), players);
                    game.GameFinished += Game_GameFinished;
                    _matchRecord.TryAdd(game,null);
                }
            }
        }

        

        public string GetWinnerOfMatch()
        {
            var topWinner = new Dictionary<string, int>();
            foreach (var  record in _matchRecord)
            {

                if (topWinner.TryGetValue(record.Value.Name, out int count))
                {
                    topWinner[record.Value.Name] = count + 1;
                }
                else
                {
                    topWinner.Add(record.Value.Name, 1);
                }
            }
           return topWinner.OrderByDescending(kvp => kvp.Value).Take(1).Select(kvp => kvp.Key).FirstOrDefault();

        }

        private void Game_GameFinished(object sender, EventArgs e)
        {
            Game game = (Game)sender;
            List<Player> playersList = game.GetWinnerOfGame();
            _matchRecord.TryUpdate(game, playersList[0], null);
        }

        public bool CanMatchBeStarted()
        {
            if (players.Count == PLAYERSINMATCH)
                return true;
            return false;
        }
        
        public void StartMatch()
        {
            if (CanMatchBeStarted())
            {
                IntializeMatch();
            }
            else
            {
                throw new MatchException("Match setup not complete, Match cannot be started");
            }
        }
        public List<Game> GetAllGames()
        {
            return _matchRecord.Keys.ToList();
        }

        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return players.Any(x => x.Name == name && x.PlayerType == playerType);
        }
        public void AddPlayertoMatch(string name, PlayerType playerType)
        {
            players.Add(new Player(name, playerType));
        }

        public List<Player> GetPlayersofMatch()
        {
            return players;
        }

    }
}
