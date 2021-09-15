using Gameservice;
using GameService;
using System;
using System.Collections.Generic;
using static GameService.IMovement;

namespace GameClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Match.StartMatchInformation);
            Match match = new Match();
            AddPlayersToMatch(match);
            StartMatch(match);
       }

        private static void StartMatch(Match match)
        {
            if (match.CanMatchBeStarted())
                match.StartMatch();
            else
                Console.WriteLine("Match cannot be started. Prerequisties missing");
            foreach (Game game in match.GetAllGames())
            {
                Console.Write("Start Game {0} :", game.GameId);
                StartGame(game);
                GetWinnerOfGame(game);
                EndGame(game);
            }
            GetWinnerOfMatch(match);
        }

        private static void GetWinnerOfMatch(Match match)
        {
            string player = match.GetWinnerOfMatch();
            Console.WriteLine("Congratulations {0} , You have won the match", player);
        }

        private static void EndGame(Game game)
        {
            game.EndGame();
        }

        private static void GetWinnerOfGame(Game game)
        {
            List<Player> players = game.GetWinnerOfGame();
            foreach (Player player in players)
            {
                Console.WriteLine("Congratulations winner of the game : {0} - {1}", player.Name, player.PlayerType);
            }
        }
        private static void StartGame(Game game)
        {
            foreach (Player player in game.GetPlayersofGame())
            {


                if (player.PlayerType == IPlayer.PlayerType.HUMAN)
                {
                    while (true)
                    {
                        Console.WriteLine("\n{0}, {1} - {2}", Movement.ChooseMoveTypeInformation, player.Name, player.PlayerType);
                        var response = Console.ReadLine();
                        if (Movement.IsValidMoveType(response))
                        {
                            game.SetMoveForPlayer(player, Movement.GetMoveType(response));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Move Type not recognized. Please re-enter");
                        }
                    }
                }
                else if (player.PlayerType == IPlayer.PlayerType.COMPUTER)
                {
                    MoveType move = Movement.GetRandomMove();
                    Console.WriteLine("\n Automated Move is : {0}, {1} - {2}", move, player.Name, player.PlayerType);

                    game.SetMoveForPlayer(player, move);
                }

            }
        }

        private static void AddPlayersToMatch(Match match)
        {
            for (int i = 0; i < Match.PLAYERSINMATCH; i++)
            {
                Console.WriteLine(string.Format("Player:  {0}", i + 1));
                Console.WriteLine(Player.ChoosePlayerTypeInformation);
                var response = Console.ReadLine();

                if (Player.IsValidPlayerType(response))
                {
                    var name = GetNameOfPlayer(i, response, match);
                    Console.WriteLine("Adding Player to the match");
                    match.AddPlayertoMatch(name, Player.GetPlayerType(response));
                }
                else
                {
                    Console.WriteLine("Player Type not recognized. Please re-enter");
                    i--;
                }
            }
        }

        private static string GetNameOfPlayer(int i, string type, Match match)
        {
            while (true)
            {
                Console.WriteLine(string.Format("Enter a name for Player: {0}", i + 1));
                var name = Console.ReadLine();
                if (match.DoesPlayerExist(name, Player.GetPlayerType(type)))
                {
                    Console.WriteLine("Player already exists. Please re-enter another name");
                }
                else
                {
                    return name;
                }
            }
        }
    }
}
