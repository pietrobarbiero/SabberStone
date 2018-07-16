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
		private static CardClass stringToCardClass(string c ) {
			if (c.Equals("MAGE"))
				return CardClass.MAGE;
			if (c.Equals("PALADIN"))
				return CardClass.PALADIN;
			if (c.Equals("PRIEST"))
				return CardClass.PRIEST;

			if (c.Equals("DRUID"))
				return CardClass.DRUID;
			if (c.Equals("HUNTER"))
				return CardClass.HUNTER;
			if (c.Equals("ROGUE"))
				return CardClass.ROGUE;

			if (c.Equals("SHAMAN"))
				return CardClass.SHAMAN;
			if (c.Equals("WARLOCK"))
				return CardClass.WARLOCK;
			if (c.Equals("WARRIOR"))
				return CardClass.WARRIOR;


			throw new Exception("CARD CLASS NOT VALID");

		}

		private static List<Card> stringToDeck(string c)
		{
			if (c.Equals("RenoKazakusMage"))
				return Decks.RenoKazakusMage;
			if (c.Equals("MidrangeJadeShaman")) 
				return Decks.MidrangeJadeShaman;
			if (c.Equals("AggroPirateWarrior"))
				return Decks.AggroPirateWarrior;
			throw new Exception("DECK DOES NOT EXIST");

		}

		private static GameConfig gameConfigCoevoluationary(string[] args) {
			GameConfig gameConfig = new GameConfig
			{
				StartPlayer = 1,
				Player1HeroClass = stringToCardClass(args[1]),
				Player2HeroClass = stringToCardClass(args[4]),
				FillDecks = false,
				Logging = false,
				Player1Deck = stringToDeck(args[0]),
				Player2Deck = stringToDeck(args[3]) //RenoKazakusMage
			};

			return gameConfig;

		}


		

		private static void Main(string[] args)
		{

			
			Console.WriteLine("Setup gameConfig");



			//todo: rename to Main
			/*GameConfig gameConfig = new GameConfig
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

			GameConfig gameConfig = gameConfigCoevoluationary(args);

			Console.WriteLine("Setup POGameHandler");
			AbstractAgent player1agent = new ParametricGreedyAgent();
			((ParametricGreedyAgent)player1agent).setAgeintWeightsFromString(args[2]);
			AbstractAgent player2agent = new ParametricGreedyAgent();
			((ParametricGreedyAgent)player2agent).setAgeintWeightsFromString(args[5]);
			POGameHandler gameHandler = new POGameHandler(gameConfig, player1agent, player2agent, debug:false);
			gameConfig.StartPlayer = -1; //Pick random start player

			Console.WriteLine("STARTING GAMES");
			int numGames = Int32.Parse(args[6]);

			gameHandler.PlayGames(numGames);
			GameStats gameStats = gameHandler.getGameStats();
			//gameStats.printResults();
			int p1wins = gameStats.PlayerA_Wins;
			int p2wins = gameStats.PlayerB_Wins;
			Console.WriteLine(p1wins+" "+p2wins+" "+ numGames+ " " +
				gameStats.PlayerA_TurnsToWin+" "+
				gameStats.PlayerA_TurnsToLose+" "+
				gameStats.PlayerA_HealthDifferenceWinning + " " +
				gameStats.PlayerA_HealthDifferenceLosing
				);



			//Console.ReadLine();
		}
	}
}
