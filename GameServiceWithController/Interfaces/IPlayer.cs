using GameServiceWithController.Models;
using System;

namespace GameServiceWithController.Interfaces
{
    public interface IPlayer
    {
        public Guid PlayerId { get; set; }
        string Name { get; set; }
        PlayerType Type { get; set; }
        int Score { get; set; }
      
    }
}
