using GameServiceWithController.Interfaces;
using GameServiceWithController.Models;
using System;

namespace GameServiceWithController.Entities
{
    public class Movement : IMovement
    {
        
        public static string ChooseMoveTypeInformation { get; set; } = $"Choose Move Type {GetAllMoveTypes()} ";
      
        
        public MoveType MovementType { get ; set ; }

        public Movement(MoveType type)
        {
            MovementType = type;
        }
        public static string GetAllMoves()
        {
            return string.Join(",", Enum.GetNames(typeof(MoveType)));
        }
        public static string GetAllMoveTypes()
        {
            return string.Join(",", Enum.GetNames(typeof(MoveType)));
        }

        public static bool IsValidMoveType(string value)
        {
            return Enum.IsDefined(typeof(MoveType), value.ToUpper());
        }
        public static MoveType GetMoveType(string value)
        {
            return (MoveType)Enum.Parse(typeof(MoveType), value.ToUpper());
        }
      
       
        
    }
}
