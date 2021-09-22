using GameService.Models;

namespace GameService.Interfaces
{
    public interface IPlayer
    {
        string Name { get; set; }
        PlayerType Type { get; set; }
        int Score { get; set; }
      
    }
}
