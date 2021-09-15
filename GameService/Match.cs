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
        private readonly List<Player> matchPlayers;
        public List<Player> WinnerOfMatch { get; private set; }
        //Maintain the GameRecord with Game and Winner Players of the each Game
        private ConcurrentDictionary<Game, List<Player>> MatchRecord { get; set; }

        public Match()
        {
            MatchRecord = new ConcurrentDictionary<Game, List<Player>>();
            matchPlayers = new List<Player>();
            WinnerOfMatch = new List<Player>();
        }
        private void IntializeMatch()
        {
            if (MatchRecord.Count == 0)
            {
                for (int i = 0; i < GAMESINMATCH; i++)
                {
                    Game game = new Game(Guid.NewGuid(), matchPlayers);
                    game.GameFinished += Game_GameFinished;
                    MatchRecord.TryAdd(game,null);
                }
            }
        }

        public void EndMatch()
        {
            int topScore = matchPlayers.Max(x => x.Score);
            WinnerOfMatch= matchPlayers.Where(x => x.Score == topScore).ToList();
        }

      
        private void Game_GameFinished(object sender, EventArgs e)
        {
            Game game = (Game)sender;
            List<Player> playersList = game.WinnerOfGame;
            MatchRecord.TryUpdate(game, playersList, null);
            foreach (var matchplayer in from Player matchplayer in matchPlayers
                                        from _ in
                                            from Player gameWinnerPlayer in playersList
                                            where matchplayer.Name == gameWinnerPlayer.Name
                                            select new { }
                                        select matchplayer)
            {
                matchplayer.Score += 1;
            }
        }

        public bool CanMatchBeStarted()
        {
            if (matchPlayers.Count == PLAYERSINMATCH)
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
            return MatchRecord.Keys.ToList();
        }

        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return matchPlayers.Any(x => x.Name == name && x.PlayerType == playerType);
        }
        public void AddPlayertoMatch(string name, PlayerType playerType)
        {
            matchPlayers.Add(new Player(name, playerType));
        }

        public List<Player> GetPlayersofMatch()
        {
            return matchPlayers;
        }

    }
}
