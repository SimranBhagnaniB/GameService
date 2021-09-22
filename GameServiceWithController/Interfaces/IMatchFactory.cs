

using GameServiceWithController.Models;
using System;
using System.Collections.Generic;

namespace GameServiceWithController.Interfaces
{
    public interface IMatchFactory
    {
        public Guid NewMatchForGameType(GameType gameType);

        public Guid AddPlayerToMatch(string matchId, string playerName, PlayerType type);
        public IMatch StartMatchWithTwoPlayersAndGameType(string matchId, string playerAName, string playerBName, PlayerType playerAType, PlayerType playerBType, GameType game);
        public List<Guid> StartMatchGetGames(string matchId);
        public List<Guid> StartMatch(string matchId);
        public bool StartGameInMatch(string matchId, string gameId);
        public void SetMoveForPlayerInGameInMatch(string matchId, string gameId, string playerId, MoveType moveType);
        public void SetMoveForPlayerInGameInMatch(string matchId, string gameId, string playerName, PlayerType type, MoveType moveType);
        public void SetAutomatedMoveForPlayerInGameInMatch(string matchId, string gameId, string playerId);
        public void SetAutomatedMoveForPlayerInGameInMatch(string matchId, string gameId, string playerName, PlayerType type);
        public List<IPlayerMove> GetWinnerOfGameInMatch(string matchId, string gameId);
        public List<IPlayerMove> GetWinnerOfMatch(string matchId);
        public void EndGameInMatch(string matchId, string gameId);
        public void EndMatch(string matchId);
    }

}
