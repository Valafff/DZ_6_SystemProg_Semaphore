using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ_6_SystemProgramming_Semaphore
{
	public class Player
	{
		public string Name { get; set; }
		public int PlayerMoney { get; set; }
		public int Bet { get; set; }
		public int Number { get; set; }

        public Player(string _name, int _money)
        {
			Name = _name;
			PlayerMoney = _money;
        }
    }
}
