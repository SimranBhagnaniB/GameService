using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using System;

namespace GameServiceWithController.Entities
{
    public class PlayerMove : IPlayerMove
    {
        public string Name { get; set; }
        public PlayerType Type { get ; set; }
        public int Score { get ; set ; }
        public MoveType MovementType { get ; set ; }
        public Guid PlayerId { get; set; }

        public PlayerMove()
        {
            PlayerId = Guid.NewGuid();
        }

        public PlayerMove(string name, PlayerType playerType)
        {
            PlayerId =  Guid.NewGuid();
            Name = name;
            Type = playerType;
        }
        public PlayerMove(Guid playerId,string name, PlayerType playerType)
        {
            PlayerId = playerId;
            Name = name;
            Type = playerType;
        }
    }
}
