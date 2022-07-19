using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.InGameSingle.HP
 * Description
 */
namespace MineBeat.InGameSingle.HP
{
	/*
	 * [Class] HPManager
	 * 플레이어의 체력을 관리합니다.
	 */
	public class HPManager : MonoBehaviour
	{
		[Range(0, 100)]
		private short _maxHp = 100;
		public short maxHp
		{
			get { return _maxHp; }
		}

		private short _hp;
		public short hp
		{
			get { return _hp; }
			set
			{
				if (value <= 0) _hp = 0;
				else if (value >= maxHp) _hp = maxHp;
				else _hp = value;
			}
		}

		private void Start()
		{
			hp = maxHp;
		}
	}
}
