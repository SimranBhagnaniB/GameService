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
         
            List<Game> games = match.GetAllGames();
            foreach (var game in games)
            {
                foreach (Player player in game.GetPlayersofGame())
                {
                    if (player.Name == "SimRan")
                        game.SetMoveForPlayer(player, MoveType.ROCK);
                    else
                        game.SetMoveForPlayer(player, MoveType.PAPER);
                }
                game.EndGame();
            }
            match.EndMatch();
            
            Assert.True(match.WinnerOfMatch.Count==1);
            Assert.True(match.WinnerOfMatch[0].Name == "XYX");
        }

        [Fact]
        public void TestPlayerExistsInGame()
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
        public void TestGetWinnerOfGame()
        {

            Player player1 = new Player("Test1", PlayerType.HUMAN);
            Player player2 = new Player("Test2", PlayerType.HUMAN);
            List<Player> players = new List<Player>
            {
                player1,
                player2,
            };
            Game game = new Game(Guid.NewGuid(), players);
            foreach (Player player in game.GetPlayersofGame())
            {
                if (player.Name == "Test1")
                {
                    game.SetMoveForPlayer(player, MoveType.PAPER);
                }
                else
                {
                    game.SetMoveForPlayer(player, MoveType.SCISSOR);
                }
            }
            game.EndGame();
            List<Player> winners = game.WinnerOfGame;
            Assert.True(winners.Count==1);
            Assert.True(winners[0].Name == player2.Name);
        }

        [Fact]
        public void TestCompareMoves()
        {
            List < MoveType > moveTypes = new List<MoveType> { MoveType.ROCK, MoveType.SCISSOR, MoveType.PAPER };
            MoveType type = Movement.CompareMoves(moveTypes);
            Assert.True(type == MoveType.PAPER);
        }

        [Fact]
        public void TestCompareMoves2()
        {
            MoveType x = MoveType.ROCK;
            MoveType y = MoveType.SCISSOR;
            MoveType type = Movement.CompareMoves(x,y);
            Assert.True(type == MoveType.ROCK);
        }

    }
}
