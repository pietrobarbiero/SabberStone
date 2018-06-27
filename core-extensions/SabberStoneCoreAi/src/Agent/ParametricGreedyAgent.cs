using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SabberStoneCoreAi.src.Agent
{
	class ParametricGreedyAgent : AbstractAgent
	{
		public override void FinalizeAgent()
		{
			
		}

		public override void FinalizeGame()
		{
			
		}

		public override PlayerTask GetMove(POGame.POGame poGame)
		{
			/*Console.WriteLine("STARTING GET MOVE");
			List<PlayerTask> options = poGame.CurrentPlayer.Options();
			
			PlayerTask bestTask = null;
			int bestTaskScore = Int32.MinValue;
			foreach (PlayerTask task in options) {
				Console.Write("---->POSSIBLE ");
				printTask(task);

				if (task.HasSource)
				{
					Console.WriteLine("HAS SOURCE. SOURCE IS -->" + task.Source);
				}

				if (task.PlayerTaskType == PlayerTaskType.MINION_ATTACK && task.Target == poGame.CurrentOpponent.Hero)
				{
					Console.WriteLine("MINION ATTACKING OPONENT HERO");
				}

				if (task.HasTarget)
				{
					Console.WriteLine("HAS TARGET. TARGET IS -->"+task.Target);
					Console.WriteLine("HAS TARGET. CARD IS----->" + task.Target.Card.ToString());

				}
				else {
					Console.WriteLine("NOT TARGET");
				}

				

				if (task.PlayerTaskType == PlayerTaskType.PLAY_CARD) {
					Console.WriteLine("PLAYING CARD");
				}

				if (task.PlayerTaskType == PlayerTaskType.MINION_ATTACK && task.Target == poGame.CurrentOpponent.Hero) {
					Console.Write("¡¡¡¡¡¡¡ATTACKING ENEMY HERO!!!!!!!!!");
					printTask(task);
					return task;
				}

				
				if (task.PlayerTaskType == PlayerTaskType.MINION_ATTACK && task.Target is Minion)
				{
					Console.Write("¡¡¡¡¡¡¡¡ATTACKING MINION!!!!!!!!!");
					printTask(task);
					return task;
				}

				int score = 0;
				if (task.PlayerTaskType != PlayerTaskType.END_TURN)
				{
					List<PlayerTask> toSimulate = new List<PlayerTask>();
					toSimulate.Add(task);
					Dictionary<PlayerTask, POGame.POGame> simulated = poGame.Simulate(toSimulate);
					Console.WriteLine("SIMULATION COMPLETE");
					int score = scoreTask(poGame, simulated[task]);
				}

				if (score >= bestTaskScore) {
					bestTask = task;
					bestTaskScore = score;
				}


				
			}

			int myManaUsed = poGame.CurrentPlayer.TotalManaSpentThisGame;
			int myHeroPowerInGame = poGame.CurrentPlayer.NumTimesHeroPowerUsedThisGame;
			int myCurrentCardsToDraw = poGame.CurrentPlayer.NumCardsToDraw;
			int enemyMinionsKilled = poGame.CurrentOpponent.NumFriendlyMinionsThatDiedThisTurn;
			int currentTurn = poGame.Turn;

			Console.WriteLine("TURN: "+currentTurn+" "+ myManaUsed + " "+ myHeroPowerInGame + " "+ myCurrentCardsToDraw + " "+ enemyMinionsKilled);
			Console.WriteLine("SELECTED TASK TO EXECUTE ");
			printTask(bestTask);*/

			Console.WriteLine("CURRENT TURN: " + poGame.Turn);
			KeyValuePair<PlayerTask,double> p = getBestTask(poGame);
			Console.WriteLine("SELECTED TASK TO EXECUTE HAS A SCORE OF "+p.Value);
			printTask(p.Key);
			Console.WriteLine("-------------------------------------");
			//Console.ReadKey();

			return p.Key;
		}

		//Mejor hacer esto con todas las posibles en cada movimiento
		public double scoreTask(POGame.POGame before, POGame.POGame after) {
			double score = 0;

			if (after.CurrentOpponent.Hero.Health <= 0)
			{
				Console.WriteLine("KILLING ENEMY!!!!!!!!");
				return Int32.MaxValue;
			}
			if (after.CurrentPlayer.Hero.Health <= 0)
			{
				Console.WriteLine("WARNING: KILLING MYSELF!!!!!");
				return Int32.MinValue;
			}

			int myDiffHealth = before.CurrentPlayer.Hero.Health - after.CurrentPlayer.Hero.Health;
			int enemyDiffHealth = before.CurrentOpponent.Hero.Health - after.CurrentOpponent.Hero.Health;

			
			score = score + enemyDiffHealth - myDiffHealth;

			int myDiffArmour = before.CurrentPlayer.Hero.Armor - after.CurrentPlayer.Hero.Armor;
			int enemyDiffArmour = before.CurrentOpponent.Hero.Armor - after.CurrentOpponent.Hero.Armor;

			Console.WriteLine(
				  before.CurrentPlayer.Hero.Health + "->" + after.CurrentPlayer.Hero.Health + " "
				+ before.CurrentOpponent.Hero.Health + "->" + after.CurrentOpponent.Hero.Health+ " "
				+ before.CurrentPlayer.Hero.Armor +"->"+ after.CurrentPlayer.Hero.Armor + " "
				+ before.CurrentOpponent.Hero.Armor +"->"+ after.CurrentOpponent.Hero.Armor
				);




			/*foreach (Minion m in before.CurrentOpponent.BoardZone.GetAll()) {
				Console.WriteLine("ENEMY HAS " + m + "(+" + m.Health + ")" + " in Deckzone");
			}

			foreach (Minion m in after.CurrentOpponent.BoardZone.GetAll())
			{
				Console.WriteLine("NOW ENEMY HAS " + m + "(+"+m.Health+")" + " in Deckzone");
			}*/

			//Console.WriteLine("CALCULATING ENEMY MINIONS");
			double scoreEnemyMinions = 0;
			//double scoreEnemyMinions = calculateScoreMinions(before.CurrentOpponent.BoardZone,after.CurrentOpponent.BoardZone);
			Console.WriteLine("CALCULATING MY MINIONS");
			double scoreMyMinions = calculateScoreMinions(before.CurrentPlayer.BoardZone, after.CurrentPlayer.BoardZone);

			return score+scoreMyMinions+scoreEnemyMinions; //CHANGE SIGNS ACCORDINGLY!!!
		}

		double calculateScoreMinions(SabberStoneCore.Model.Zones.BoardZone before, SabberStoneCore.Model.Zones.BoardZone after) {
			foreach (Minion m in before.GetAll())
			{
				Console.WriteLine("BEFORE "+m + "(+" + m.Health + ")" + "");
			}

			foreach (Minion m in after.GetAll())
			{
				Console.WriteLine("AFTER  " + m + "(+" + m.Health + ")" + "");
			}


			double scoreDamaged = 0;
			double scoreAttack = 0;
			double scoreDead = 0;
			double scorePlayed = 0;

			//Minions modified?
			foreach (Minion mb in before.GetAll())
			{
				bool survived = false;
				foreach (Minion ma in after.GetAll())
				{
					if (ma.Id == mb.Id)
					{
						scoreDamaged = scoreDamaged + (mb.Health - ma.Health);
						Console.WriteLine("Difference in health of " + mb + " is " + (mb.Health - ma.Health));
						survived = true;
						scoreAttack = scoreAttack + (mb.AttackDamage - ma.AttackDamage); //CHECK ATTACK DAMAGE
					}
				}

				if (survived == false)
				{
					Console.WriteLine(mb + " was killed");
					scoreDead = scoreDead+1; //WHATEVER
				}
			}

			//New Minions on play?
			foreach (Minion ma in after.GetAll())
			{
				bool existed = false;
				foreach (Minion mb in before.GetAll())
				{
					if (ma.Id == mb.Id)
					{
						existed = true;
					}
				}
				if (existed == false) {
					Console.WriteLine(ma + " is NEW!!");
					scorePlayed = scorePlayed + 1;//WHATEVER
				}
			}

			return scoreDamaged+scoreAttack+scoreDead+scorePlayed; //CHANGE THESE SIGNS ACCORDINGLY!!!

		}


		KeyValuePair<PlayerTask, double> getBestTask(POGame.POGame state) {
			double bestScore = Double.MinValue;
			PlayerTask bestTask = null;
			List<PlayerTask> list  = state.CurrentPlayer.Options();
			foreach (PlayerTask t in list) {
				Console.Write("---->POSSIBLE ");
				printTask(t);
				double score = 0;
				POGame.POGame before = state;
				if (t.PlayerTaskType == PlayerTaskType.END_TURN)
				{
					score = 0;
				}
				else
				{
					List<PlayerTask> toSimulate = new List<PlayerTask>();
					toSimulate.Add(t);
					Dictionary<PlayerTask, POGame.POGame> simulated = state.Simulate(toSimulate);
					//Console.WriteLine("SIMULATION COMPLETE");
					score = scoreTask(state, simulated[t]);
				}
				Console.WriteLine("SCORE " + score);
				if (score >= bestScore)
				{
					bestTask = t;
					bestScore = score;
				}

			}

			return new KeyValuePair<PlayerTask, double>(bestTask,bestScore);
		}

		public override void InitializeAgent()
		{
			
		}

		public override void InitializeGame()
		{
			
		}

		private void printTask(PlayerTask task) {
			Console.Write("TASK: " + task.PlayerTaskType + " " + task.Source + "----->" + task.Target+" ");
			if (task.Target != null)
				Console.Write(task.Target.Controller.PlayerId);
			else
				Console.Write("No target");
			Console.Write("\n");
		}
	}
}
