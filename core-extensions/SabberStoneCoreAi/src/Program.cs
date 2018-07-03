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
using System.Globalization;

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

		private static AbstractAgent ParametricAgentFromString(string values)
		{
			ParametricGreedyAgent player = new ParametricGreedyAgent();

			string[] vs = values.Split("#");

			if (vs.Length != ParametricGreedyAgent.NUM_PARAMETERS)
				throw new Exception("NUM VALUES NOT CORRECT");

			double[] ws = new double[ParametricGreedyAgent.NUM_PARAMETERS];
			for(int i = 0; i<ws.Length;i++) {
				ws[i] = Double.Parse(vs[i], CultureInfo.InvariantCulture);				
			}

			player.setAgentWeights(ws);
			return player;

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
			AbstractAgent player1agent = ParametricAgentFromString(args[2]);
			AbstractAgent player2agent = ParametricAgentFromString(args[5]);
			POGameHandler gameHandler = new POGameHandler(gameConfig, player1agent, player2agent, debug:false);
			gameConfig.StartPlayer = -1; //Pick random start player

			Console.WriteLine("STARTING GAMES");
			int numGames = Int32.Parse(args[6]);

			gameHandler.PlayGames(numGames);
			GameStats gameStats = gameHandler.getGameStats();
			//gameStats.printResults();
			Console.WriteLine(gameStats.PlayerA_Wins+" "+gameStats.PlayerB_Wins+" "+ numGames);






			//Console.ReadLine();
		}
	}
}
