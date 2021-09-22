
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GameServiceWithController.Entities;
using GameServiceWithController.Interfaces;
using GameServiceWithController.Services;
using Newtonsoft.Json;

namespace GameClient
{
    static class Program
    {
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri("https://localhost:44309/");
            var val = "application/json";
            var media = new MediaTypeWithQualityHeaderValue(val);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(media);

            var request  = await client.GetAsync("api/match/GetNewMatchForTwoPlayerGame");
            var response = request.Content.ReadAsStringAsync();
            
            string myMatchId = JsonConvert.DeserializeObject<string>(response.Result);

             Console.WriteLine(myMatchId);
           
           
            
           
       }

        //private static void StartMatch(IMatch match)
        //{
        //    if (match.CanMatchBeStarted())
        //        match.StartMatch();
        //    else
        //        Console.WriteLine("Match cannot be started. Prerequisties missing");
        //    foreach (IGame game in match.Games))
        //    {
        //        Console.Write("Start Game {0} :", game.);
        //        StartGame(game);
        //        EndGame(game);
        //        GetWinnerOfGame(game);
                
        //    }
        //    GetWinnerOfMatch(match);
        //}

        //private static void GetWinnerOfMatch(Match match)
        //{
        //    match.EndMatch();
        //    List<IPlayerMove> players = match.WinnerOfMatch;
        //    foreach (IPlayerMove player in players)
        //    {
        //        Console.WriteLine("Congratulations {0} : {1} , You have won the match, match points :{2}", player.Name, player.PlayerType, player.Score);
        //    }
        //}

        //private static void EndGame(IGame game)
        //{
        //    game.EndGame();
        //}

       
        //private static void StartGame(IGame game)
        //{
        //    foreach (IPlayerMove player in game.GetPlayersofGame())
        //    {
        //        if (player.Type == PlayerType.HUMAN)
        //        {
        //            while (true)
        //            { 
        //                Console.WriteLine("\n{0}, {1} - {2}", Movement.ChooseMoveTypeInformation, player.Name, player.PlayerType);
        //                var response = Console.ReadLine();
        //                if (Movement.IsValidMoveType(response))
        //                {
        //                    game.SetMoveForPlayer(player, Movement.GetMoveType(response));
        //                    break;
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Move Type not recognized. Please re-enter");
        //                }
        //            }
        //        }
        //        else if (player.PlayerType == IPlayer.PlayerType.COMPUTER)
        //        {
        //            MoveType move = Movement.GetRandomMove();
        //            Console.WriteLine("\n Automated Move is : {0}, {1} - {2}", move, player.Name, player.PlayerType);

        //            game.SetMoveForPlayer(player, move);
        //        }

        //    }
        //}

        //private static void AddPlayersToMatch(Match match)
        //{
        //    for (int i = 0; i < Match.PLAYERSINMATCH; i++)
        //    {
        //        Console.WriteLine(string.Format("Player:  {0}", i + 1));
        //        Console.WriteLine(Player.ChoosePlayerTypeInformation);
        //        var response = Console.ReadLine();

        //        if (Player.IsValidPlayerType(response))
        //        {
        //            var name = GetNameOfPlayer(i, response, match);
        //            Console.WriteLine("Adding Player to the match");
        //            match.AddPlayertoMatch(name, Player.GetPlayerType(response));
        //        }
        //        else
        //        {
        //            Console.WriteLine("Player Type not recognized. Please re-enter");
        //            i--;
        //        }
        //    }
        //}

        //private static string GetNameOfPlayer(int i, string type, Match match)
        //{
        //    while (true)
        //    {
        //        Console.WriteLine(string.Format("Enter a name for Player: {0}", i + 1));
        //        var name = Console.ReadLine();
        //        if (match.DoesPlayerExist(name, Player.GetPlayerType(type)))
        //        {
        //            Console.WriteLine("Player already exists. Please re-enter another name");
        //        }
        //        else
        //        {
        //            return name;
        //        }
        //    }
        //}
    }
}
