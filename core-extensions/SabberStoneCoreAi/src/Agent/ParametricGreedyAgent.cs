using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using System;
using System.Collections.Generic;
using System.Text;

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
			Console.WriteLine("STARTING GET MOVE");
			List<PlayerTask> options = poGame.CurrentPlayer.Options();
			
			PlayerTask bestTask = null;
			foreach (PlayerTask task in options) {
				Console.WriteLine("POSSIBLE TASK---->"+ task.PlayerTaskType);
				Console.WriteLine("SOURCE IS ------->" + task.Source+" "+"TARGET IS -->" + task.Target);

				/*if (task.HasSource)
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
				}*/



				bestTask = task;

				
			}

			

			return bestTask;
		}

		public override void InitializeAgent()
		{
			
		}

		public override void InitializeGame()
		{
			
		}
	}
}
