using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.InGameSingle.HP
{
	/// <summary>
	/// 플레이어의 체력을 관리합니다.
	/// </summary>
	public class HPManager : MonoBehaviour
	{
		[SerializeField]
		private bool undead = false;

		[SerializeField]
		private bool fixHp = false;

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
				if (fixHp) return;

				if (undead && value <= 1) _hp = 1;
				else if (!undead && value <= 0) _hp = 0;
				else if (value >= maxHp) _hp = maxHp;
				else _hp = value;
			}
		}

		private void Start()
		{
			undead = ConfigManager.Instance.GetConfig().undeadMode;
			hp = maxHp;
		}

		public void FixHp()
		{
			hp = maxHp;
			fixHp = true;
		}
	}
}
