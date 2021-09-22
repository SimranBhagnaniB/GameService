using GameServiceWithController.Entities;
using GameServiceWithController.Interfaces;
using GameServiceWithController.Services;
using GameServiceWithController.Models;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;

namespace GameConsoleClient
{
    public class TestGameService
    {

        [Fact]
        public void CreationOfNewMatchIsSuccess()
        {
            var resultEngineService = new Mock<IResultEngineService>();
            resultEngineService.Setup(x => x.CompareMoves(MoveType.ROCK, MoveType.SCISSOR)).Returns(MoveType.ROCK);
            var gameService = new Mock<IGameFactory>();
            var loggerMock = Mock.Of<ILogger<MatchService>>();
            var movementService = new Mock<IMovementFactory>();
            IMatchFactory matchFactory = new MatchService(gameService.Object, movementService.Object,loggerMock );
            var matchId = matchFactory.NewMatchForGameType(GameType.TWOPLAYER);
            Assert.NotNull(matchId.ToString());
        }
        
        private Mock<IGameFactory> AddTwoPlayerGames()
        {
            var resultEngineService = new Mock<IResultEngineService>();
            resultEngineService.Setup(x => x.CompareMoves(MoveType.ROCK, MoveType.SCISSOR)).Returns(MoveType.ROCK);
            resultEngineService.Setup(x => x.CompareMoves(MoveType.SCISSOR, MoveType.ROCK)).Returns(MoveType.ROCK);
            resultEngineService.Setup(x => x.CompareMoves(MoveType.SCISSOR, MoveType.PAPER)).Returns(MoveType.SCISSOR);
            resultEngineService.Setup(x => x.CompareMoves(MoveType.PAPER, MoveType.SCISSOR)).Returns(MoveType.SCISSOR);
            resultEngineService.Setup(x => x.CompareMoves(MoveType.PAPER, MoveType.ROCK)).Returns(MoveType.PAPER);
            resultEngineService.Setup(x => x.CompareMoves(MoveType.ROCK, MoveType.PAPER)).Returns(MoveType.PAPER);
            List<IPlayerMove> players = GetPlayers();
            var gameService = new Mock<IGameFactory>();
            List<IGame> games = new List<IGame>();
            IGame myGameA = new TwoPlayerGame(resultEngineService.Object);
            ((ITwoPlayerGame)myGameA).PlayerA = players[0];
            ((ITwoPlayerGame)myGameA).PlayerB = players[1];
            games.Add(myGameA);
          
            IGame myGameB = new TwoPlayerGame(resultEngineService.Object);
            ((ITwoPlayerGame)myGameA).PlayerA = players[0];
            ((ITwoPlayerGame)myGameA).PlayerB = players[1];
            games.Add(myGameB);

            IGame myGameC = new TwoPlayerGame(resultEngineService.Object);
            ((ITwoPlayerGame)myGameA).PlayerA = players[0];
            ((ITwoPlayerGame)myGameA).PlayerB = players[1];
            games.Add(myGameC);
                       
            gameService.Setup(x => x.GenerateGame(3, GameType.TWOPLAYER)).Returns(games);
            
            return gameService;
        }

        private List<IPlayerMove> GetPlayers()
        {
            List<IPlayerMove> players = new List<IPlayerMove>();
            IPlayerMove player1 = new PlayerMove("X", PlayerType.HUMAN);
            IPlayerMove player2 = new PlayerMove("Y", PlayerType.COMPUTER);

            players.Add(player1);
            players.Add(player2);
            return players;
        }

        [Fact]
        public void StartOfNewMatchWithPlayersAndGameTypeIsSuccess()
        {
         
            IGameFactory gameService  = AddTwoPlayerGames().Object;
            var loggerMock = Mock.Of<ILogger<MatchService>>();
            var movementService = new Mock<IMovementFactory>();
            IMatchFactory matchFactory = new MatchService(gameService, movementService.Object,loggerMock);
            var matchId = matchFactory.NewMatchForGameType(GameType.TWOPLAYER);
            List<IPlayerMove> players = GetPlayers();
            Guid playerId = matchFactory.AddPlayerToMatch(matchId.ToString(), players[0].Name, players[0].Type );
            Assert.True(playerId.ToString() != string.Empty);
            playerId = matchFactory.AddPlayerToMatch(matchId.ToString(), players[1].Name, players[1].Type);
            Assert.True(playerId.ToString() != string.Empty);
        }

        [Fact]
        public void TestStartOfMatchSetupForGamesIsSucess()
        {
            IGameFactory gameService = AddTwoPlayerGames().Object;
            var loggerMock = Mock.Of<ILogger<MatchService>>();
            var movementService = new Mock<IMovementFactory>();
            IMatchFactory matchFactory = new MatchService(gameService,movementService.Object, loggerMock);
            var matchId = matchFactory.NewMatchForGameType(GameType.TWOPLAYER);
            List<IPlayerMove> players = GetPlayers();
            Guid playerIdA = matchFactory.AddPlayerToMatch(matchId.ToString(), players[0].Name, players[0].Type);
            Guid playerIdB = matchFactory.AddPlayerToMatch(matchId.ToString(), players[1].Name, players[1].Type);
            List<Guid> GameIds = matchFactory.StartMatch(matchId.ToString());
            Assert.True(GameIds.Count == 3);
        }


        [Fact]
        public void TestStartOfGameSetupForGamePlayersIsSuccess()
        {
            IGameFactory gameService = AddTwoPlayerGames().Object;
            var movementService = new Mock<IMovementFactory>();
            var loggerMock = Mock.Of<ILogger<MatchService>>();
            IMatchFactory matchFactory = new MatchService(gameService, movementService.Object, loggerMock);
            var matchId = matchFactory.NewMatchForGameType(GameType.TWOPLAYER);
            List<IPlayerMove> players = GetPlayers();
            Guid playerIdA = matchFactory.AddPlayerToMatch(matchId.ToString(), players[0].Name, players[0].Type);
            Guid playerIdB = matchFactory.AddPlayerToMatch(matchId.ToString(), players[1].Name, players[1].Type);
            List<Guid> GameIds = matchFactory.StartMatch(matchId.ToString());
            Assert.True(GameIds.Count == 3);
            foreach (var gameId in GameIds)
            {
                bool bSuccess = matchFactory.StartGameInMatch(matchId.ToString(), gameId.ToString());
                Assert.True(bSuccess);
            }
        }

        [Fact]
        public void TestWinnerOfGame()
        {
            Mock<IGameFactory> gameService = AddTwoPlayerGames();
            var movementService = new Mock<IMovementFactory>();
            movementService.Setup(x => x.GetRandomMove()).Returns(MoveType.PAPER);
            var loggerMock = Mock.Of<ILogger<MatchService>>();
            IMatchFactory matchFactory = new MatchService(gameService.Object, movementService.Object, loggerMock);
            var matchId = matchFactory.NewMatchForGameType(GameType.TWOPLAYER);
            List<IPlayerMove> players = GetPlayers();
            Guid playerIdA = matchFactory.AddPlayerToMatch(matchId.ToString(), players[0].Name, players[0].Type);
            Guid playerIdB = matchFactory.AddPlayerToMatch(matchId.ToString(), players[1].Name, players[1].Type);
            List<Guid> Games = matchFactory.StartMatchGetGames(matchId.ToString());
            Assert.True(Games.Count == 3);
            
            foreach (var game in Games)
            {
                
                bool bSuccess = matchFactory.StartGameInMatch(matchId.ToString(), game.ToString());
                Assert.True(bSuccess);
                matchFactory.SetMoveForPlayerInGameInMatch(matchId.ToString(), game.ToString(), players[0].Name, players[0].Type, MoveType.ROCK);
                matchFactory.SetAutomatedMoveForPlayerInGameInMatch(matchId.ToString(), game.ToString(), players[1].Name, players[1].Type);
                matchFactory.EndGameInMatch(matchId.ToString(), game.ToString());
                List<IPlayerMove> winners = matchFactory.GetWinnerOfGameInMatch(matchId.ToString(), game.ToString());
                Assert.True(winners[0].Name == "Y");
                Assert.True(winners[0].Score == 1);
            }
        }


        [Fact]
        public void TestWinnerOfMatch()
        {
            Mock<IGameFactory> gameService = AddTwoPlayerGames();
            var movementService = new Mock<IMovementFactory>();
            movementService.Setup(x => x.GetRandomMove()).Returns(MoveType.SCISSOR);
            var loggerMock = Mock.Of<ILogger<MatchService>>();
            IMatchFactory matchFactory = new MatchService(gameService.Object, movementService.Object, loggerMock);
            var matchId = matchFactory.NewMatchForGameType(GameType.TWOPLAYER);
            List<IPlayerMove> players = GetPlayers();
            Guid playerIdA = matchFactory.AddPlayerToMatch(matchId.ToString(), players[0].Name, players[0].Type);
            Guid playerIdB = matchFactory.AddPlayerToMatch(matchId.ToString(), players[1].Name, players[1].Type);
            List<Guid> Games = matchFactory.StartMatchGetGames(matchId.ToString());
            Assert.True(Games.Count == 3);

            foreach (var game in Games)
            {

                bool bSuccess = matchFactory.StartGameInMatch(matchId.ToString(), game.ToString());
                Assert.True(bSuccess);
                matchFactory.SetMoveForPlayerInGameInMatch(matchId.ToString(), game.ToString(), players[0].Name, players[0].Type, MoveType.ROCK);
                matchFactory.SetAutomatedMoveForPlayerInGameInMatch(matchId.ToString(), game.ToString(), players[1].Name, players[1].Type);
                matchFactory.EndGameInMatch(matchId.ToString(), game.ToString());
                List<IPlayerMove> gameWinners = matchFactory.GetWinnerOfGameInMatch(matchId.ToString(), game.ToString());
                Assert.True(gameWinners[0].Name == "X");
                Assert.True(gameWinners[0].Score == 1);
            }
            matchFactory.EndMatch(matchId.ToString());
            List<IPlayerMove> winners = matchFactory.GetWinnerOfMatch(matchId.ToString());
            Assert.True(winners[0].Name == "X");
            Assert.True(winners[0].Score == 3);

        }




        //[Fact]
        //public void TestDoesPlayerExistInMatch()
        //{
        //    Match match = new Match();
        //    match.AddPlayertoMatch("Sim", PlayerType.HUMAN);
        //    match.AddPlayertoMatch("X", PlayerType.COMPUTER);
        //    Assert.False(match.DoesPlayerExist("T", PlayerType.HUMAN));
        //}

        //[Fact]
        //public void AreTheGamesSetupOnMatchStart()
        //{
        //    Match match = new Match();
        //    match.AddPlayertoMatch("Simran", PlayerType.HUMAN);
        //    match.AddPlayertoMatch("XYX", PlayerType.COMPUTER);
        //    match.StartMatch();
        //    Assert.Equal(match.GetAllGames().Count, Match.GAMESINMATCH);
        //}

        //[Fact]
        //public void TestCanMatchBeStarted()
        //{
        //    Match match = new Match();
        //    match.AddPlayertoMatch("Sim", PlayerType.HUMAN);
        //    Assert.False(match.CanMatchBeStarted());
        //}

        //[Fact]
        //public void TestWinerOfMatch()
        //{
        //    Match match = new Match();
        //    match.AddPlayertoMatch("SimRan", PlayerType.HUMAN);
        //    match.AddPlayertoMatch("XYX", PlayerType.COMPUTER);
        //    match.StartMatch();

        //    List<Game> games = match.GetAllGames();
        //    foreach (var game in games)
        //    {
        //        foreach (Player player in game.GetPlayersofGame())
        //        {
        //            if (player.Name == "SimRan")
        //                game.SetMoveForPlayer(player, MoveType.ROCK);
        //            else
        //                game.SetMoveForPlayer(player, MoveType.PAPER);
        //        }
        //        game.EndGame();
        //    }
        //    match.EndMatch();

        //    Assert.True(match.WinnerOfMatch.Count == 1);
        //    Assert.True(match.WinnerOfMatch[0].Name == "XYX");
        //}

        //[Fact]
        //public void TestPlayerExistsInGame()
        //{
        //    Player player1 = new Player("Test1", PlayerType.HUMAN);
        //    Player player2 = new Player("Test2", PlayerType.HUMAN);
        //    List<Player> players = new List<Player>
        //    {
        //        player1,
        //        player2,
        //    };
        //    Game game = new Game(Guid.NewGuid(), players);
        //    bool bExists = game.DoesPlayerExist("Test1", PlayerType.COMPUTER);
        //    Assert.False(bExists);
        //}

        //[Fact]
        //public void TestGetWinnerOfGame()
        //{

        //    Player player1 = new Player("Test1", PlayerType.HUMAN);
        //    Player player2 = new Player("Test2", PlayerType.HUMAN);
        //    List<Player> players = new List<Player>
        //    {
        //        player1,
        //        player2,
        //    };
        //    Game game = new Game(Guid.NewGuid(), players);
        //    foreach (Player player in game.GetPlayersofGame())
        //    {
        //        if (player.Name == "Test1")
        //        {
        //            game.SetMoveForPlayer(player, MoveType.PAPER);
        //        }
        //        else
        //        {
        //            game.SetMoveForPlayer(player, MoveType.SCISSOR);
        //        }
        //    }
        //    game.EndGame();
        //    List<Player> winners = game.WinnerOfGame;
        //    Assert.True(winners.Count == 1);
        //    Assert.True(winners[0].Name == player2.Name);
        //}

        //[Fact]
        //public void TestCompareMoves()
        //{
        //    List<MoveType> moveTypes = new List<MoveType> { MoveType.ROCK, MoveType.SCISSOR, MoveType.PAPER };
        //    MoveType type = Movement.CompareMoves(moveTypes);
        //    Assert.True(type == MoveType.PAPER);
        //}

        //[Fact]
        //public void TestCompareMoves2()
        //{
        //    MoveType x = MoveType.ROCK;
        //    MoveType y = MoveType.SCISSOR;
        //    MoveType type = Movement.CompareMoves(x, y);
        //    Assert.True(type == MoveType.ROCK);
        //}

    }
}
