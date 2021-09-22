
using GameServiceWithController.Models;

namespace GameServiceWithController.Interfaces
{
    public interface IMovementFactory
    {
        public MoveType GetRandomMove();
    }
}
