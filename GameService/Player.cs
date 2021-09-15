using GameService;
using System;
using static GameService.IPlayer;

namespace Gameservice
{
    public class Player : IPlayer
    {
        public PlayerType PlayerType { get; set; }
        public string Name { get; set; }
        public static string ChoosePlayerTypeInformation { get; set; } = $"Choose Player Type {GetAllPlayerTypes()} ";

        public Player(string name,PlayerType type)
        {
            Name = name;
            PlayerType = type;
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
