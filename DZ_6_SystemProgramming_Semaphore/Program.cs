using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_6_SystemProgramming_Semaphore
{
	internal class Program
	{
		static Random random = new Random();

		const int MaxPlayersAtTable = 5;
		const int MinPlayers = 20;
		const int MaxPlayers = 100;
		const int MinRouletteCount = 0;
		const int MaxRouletteCount = 36;
		const int RoundTime = 25;

		static void Main(string[] args)
		{
			Casino casino = new Casino(MaxPlayersAtTable, MinRouletteCount, MaxRouletteCount, random.Next(MinPlayers, MaxPlayers), RoundTime);
			casino.StartGame();
		}
	}
}
