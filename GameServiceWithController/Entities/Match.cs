using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameServiceWithController.Entities
{
    public class Match :IMatch
    {
        public const int GAMESINMATCH = 3;
        public const int PLAYERSINMATCH = 2;

        public static string StartMatchInformation { get; set; } = $"Lets start the Match. Match Has {GAMESINMATCH} Games";
        public List<IPlayerMove> Players { get; set; }
        public List<IPlayerMove> WinnerOfMatch { get;  set; }
        public List<IGame> Games { get; set ; }
        public Guid MatchId { get ; set ; }
        public GameType GameType { get ; set ; }
       

        public Match(GameType gameType)
        {
            MatchId =  Guid.NewGuid();
            GameType = gameType;
            Players = new List<IPlayerMove>();
        }

        public Match(GameType gameType, List<IPlayerMove> players)
        {
            MatchId = Guid.NewGuid();
            GameType = gameType;
            foreach (IPlayerMove player in players)
                AddPlayertoMatch(player.Name, player.Type);
        }

        public void EndMatch()
        {
            int topScore = Players.Max(x => x.Score);
            WinnerOfMatch= Players.Where(x => x.Score == topScore).ToList();
        }

      
       

        public bool CanMatchBeStarted()
        {
            if (Players.Count == PLAYERSINMATCH)
                return true;
            return false;
        }

      

      


        public bool DoesPlayerExist(string name, PlayerType playerType)
        {
            return Players.Any(x => x.Name == name && x.Type == playerType);
        }
        public Guid AddPlayertoMatch(string name, PlayerType playerType)
        {
            Guid id = Guid.NewGuid();
            Players.Add(new PlayerMove(id,name,playerType));
            return id;
        }

       

    }
}
