using Gameservice;
using GameService;
using System;
using System.Collections.Generic;
using Xunit;
using static GameService.IMovement;
using static GameService.IPlayer;

namespace GameConsoleClient
{
    public class TestGameService
    {
        
        [Fact]
        public void TestDoesPlayerExistInMatch()
        {
            Match match = new Match();
            match.AddPlayertoMatch("Sim",PlayerType.HUMAN);
            match.AddPlayertoMatch("X", PlayerType.COMPUTER);
            Assert.False(match.DoesPlayerExist("T", PlayerType.HUMAN));
        }
       
        [Fact]
        public void AreTheGamesSetupOnMatchStart()
        {
            Match match = new Match();
            match.AddPlayertoMatch("Simran", PlayerType.HUMAN);
            match.AddPlayertoMatch("XYX", PlayerType.COMPUTER);
            match.StartMatch();
            Assert.Equal(match.GetAllGames().Count, Match.GAMESINMATCH);
        }

        [Fact]
        public void TestCanMatchBeStarted()
        {
            Match match = new Match();
            match.AddPlayertoMatch("Sim", PlayerType.HUMAN);
            Assert.False(match.CanMatchBeStarted());
        }

        [Fact]
        public void TestWinerOfMatch()
        {
            Match match = new Match();
            match.AddPlayertoMatch("SimRan", PlayerType.HUMAN);
            match.AddPlayertoMatch("XYX", PlayerType.COMPUTER);
            match.StartMatch();
            List<Player> players =match.GetPlayersofMatch();
            List<Game> games = match.GetAllGames();
            match._matchRecord.TryUpdate(games[0], players[0], null);
            match._matchRecord.TryUpdate(games[1], players[0], null);
            match._matchRecord.TryUpdate(games[2], players[1], null);
            Assert.Equal(match.GetWinnerOfMatch(),players[0].Name);
        }

        [Fact]
        private void TestPlayerExistsInGame()
        {
            Player player1 = new Player("Test1", PlayerType.HUMAN);
            Player player2 = new Player("Test2", PlayerType.HUMAN);
            List<Player> players = new List<Player>
            {
                player1,
                player2,
            };
            Game game = new Game(Guid.NewGuid(), players);
            bool bExists =game.DoesPlayerExist("Test1", PlayerType.COMPUTER);
            Assert.False(bExists);
        }

        [Fact]
        private void TestGetWinnerOfGame()
        {

            Player player1 = new Player("Test1", PlayerType.HUMAN);
            Player player2 = new Player("Test2", PlayerType.HUMAN);
            List<Player> players = new List<Player>
            {
                player1,
                player2,
            };
            Game game = new Game(Guid.NewGuid(), players);
            game.SetMoveForPlayer(player1, MoveType.PAPER);
            game.SetMoveForPlayer(player2, MoveType.SCISSOR);
            List<Player> winners = game.GetWinnerOfGame();
            Assert.True(winners.Count==1);
            Assert.True(winners[0].Name == player2.Name);
        }

        [Fact]
        private void TestCompareMoves()
        {
            List < MoveType > moveTypes = new List<MoveType> { MoveType.ROCK, MoveType.SCISSOR, MoveType.PAPER };
            MoveType type = Movement.CompareMoves(moveTypes);
            Assert.True(type == MoveType.PAPER);
        }

        [Fact]
        private void TestCompareMoves2()
        {
            MoveType x = MoveType.ROCK;
            MoveType y = MoveType.SCISSOR;
            MoveType type = Movement.CompareMoves(x,y);
            Assert.True(type == MoveType.ROCK);
        }

    }
}
