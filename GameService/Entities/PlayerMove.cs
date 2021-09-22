using GameService.Interfaces;
using GameService.Models;


namespace GameService.Entities
{
    public class PlayerMove : IPlayerMove
    {
        public string Name { get; set; }
        public PlayerType Type { get ; set; }
        public int Score { get ; set ; }
        public MoveType MovementType { get ; set ; }

        public PlayerMove(string name, PlayerType playerType)
        {
            Name = name;
            Type = playerType;
        }
    }
}
