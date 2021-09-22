using GameService.Interfaces;
using GameService.Models;
using System;

namespace Gameservice
{
    public class Player : IPlayer
    {
        public PlayerType Type { get; set; }
        public string Name { get; set; }
        public int Score { get;  set; }
        public static string ChoosePlayerTypeInformation { get; set; } = $"Choose Player Type {GetAllPlayerTypes()} ";
        public MoveType MoveType { get ; set ; }

        public Player(string name,PlayerType type)
        {
            Name = name;
            Type = type;
            Score = 0;
        }

        public Player(string name, PlayerType type,MoveType move)
        {
            Name = name;
            Type = type;
            Score = 0;
            MoveType = move;
        }

        public static string GetAllPlayerTypes()
        {
            return string.Join(",", Enum.GetNames(typeof(PlayerType)));
        }

        public static bool IsValidPlayerType(string value)
        {
            return Enum.IsDefined(typeof(PlayerType), value.ToUpper());
        }

        public static PlayerType GetPlayerType(string value)
        {
            PlayerType myType = (PlayerType)Enum.Parse(typeof(PlayerType), value.ToUpper());
            return myType;
        }

     }
}
