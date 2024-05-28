using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DZ_6_SystemProgramming_Semaphore
{
	internal class Casino
	{

		public Random random = new Random();

		Semaphore Semaphore;

		int CasinoBank = 0;
		int MinRouletteCount;
		int MaxRouletteCount;
		int roundtime;
		int EndPlayersCount = 0;

		public List<Player> Players = new List<Player>();

		public Casino(int _numberPlayersAtTable, int _minRouletteCount, int _maxRouletteCount, int _playersCount, int _roundtime)
		{
			Semaphore = new Semaphore(_numberPlayersAtTable, _numberPlayersAtTable);


			MinRouletteCount = _minRouletteCount;
			MaxRouletteCount = _maxRouletteCount;
			for (int i = 0; i < _playersCount; i++)
			{
				Players.Add(new Player($"Игрок " + (i + 1), random.Next(10, 1000)));
			}

			roundtime = _roundtime;
		}

		public void StartGame()
		{
			for (int i = 0; i < Players.Count; i++)
			{
				Thread thr = new Thread(new ParameterizedThreadStart(DoPlay));
				thr.Start(Players[i]);
			}
		}


		int PlayInRoulette()
		{
			return new Random().Next(MinRouletteCount, MaxRouletteCount);
		}

		void DoPlay(object _player)
		{
			Player player = (Player)_player;

			try
			{
				Semaphore.WaitOne();
				Console.WriteLine($"{player.Name} сел за стол");

				//Раунд
				while (true)
				{
					Thread.Sleep(roundtime);
					int result = PlayInRoulette();
					lock (player)
					{
						player.Bet = random.Next(1, player.PlayerMoney + 1);
						CasinoBank += player.Bet;
						player.PlayerMoney -= player.Bet;
					}
					player.Number = random.Next(MinRouletteCount, MaxRouletteCount);
					Console.WriteLine($"Ставка {player.Name} равна {player.Bet} | Загаданное число игрока: {player.Number} | Результат рулетки: {result}");
					lock (player)
					{
						if (player.Number == result)
						{
							Console.WriteLine($"{player.Name} ВЫИГРАЛ! Его капитал {player.PlayerMoney}");
							player.PlayerMoney += 2 * player.Bet;
							CasinoBank -= 2 * player.Bet;
							int ImSmart = random.Next(0, 100);
							if (ImSmart > 50)
							{
								Console.WriteLine($"{player.Name} наигрался и ушел");
								EndPlayersCount++;
								break;
							}
						}
						else
						{
							Console.WriteLine($"{player.Name} ПРОИГРАЛ Его капитал {player.PlayerMoney}");
						}
					}

					if (player.PlayerMoney == 0)
					{
						Console.WriteLine($"{player.Name} ВЫБЫЛ");
						EndPlayersCount++;
						break;
					}
					Thread.Sleep(roundtime);
				}
			}
			finally
			{
				Semaphore.Release();
				Console.WriteLine($"Выигрыш казино {CasinoBank}");
			}
			if (EndPlayersCount == Players.Count)
			{
				string filename = "Results.txt";
				using (var sw = new StreamWriter(filename, false, Encoding.UTF8)) 
				{
					foreach (var item in Players)
					{
						sw.WriteLine($"{item.Name} Начальная сумма {item.PlayerStartMoney} Конечная сумма {item.PlayerMoney}");
					}
				}
                Console.WriteLine($"Файл Results.txt записан в директорию {Directory.GetCurrentDirectory()}");
            }
        }

	}
}
