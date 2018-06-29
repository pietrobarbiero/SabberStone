using System;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.src.Agent;
using SabberStoneCoreAi.Meta;
using SabberStoneCore.Model;
using System.Collections.Generic;

namespace SabberStoneCoreAi
{
	internal class Program
	{
		class Individual {

		}

		
		private static void Main(string[] args)
		{

			
			Console.WriteLine("Setup gameConfigaaa");

			//todo: rename to Main
			GameConfig gameConfig = new GameConfig
			{
				StartPlayer = 1,
				Player1HeroClass = CardClass.MAGE,
				Player2HeroClass = CardClass.SHAMAN,
				FillDecks = false,
				Logging = false,
				Player1Deck = Decks.RenoKazakusMage,
				Player2Deck = Decks.MidrangeJadeShaman //RenoKazakusMage
			};


			/*foreach (Card c in Cards.All)
			{
				Console.WriteLine(c.Name);
			}*/
			

			Console.WriteLine("Setup POGameHandler");
			AbstractAgent player1 = new ParametricGreedyAgent();
			AbstractAgent player2 = new ParametricGreedyAgent();//FaceHunter();
			var gameHandler = new POGameHandler(gameConfig, player1, player2, debug:false);

			Console.WriteLine("STARTING GAMES");
			//gameHandler.PlayGame();
			gameHandler.PlayGames(100);
			GameStats gameStats = gameHandler.getGameStats();

			gameStats.printResults();


			Console.WriteLine("Test successful");
			Console.ReadLine();
		}
	}
}
