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

		[field: Range(0, 100)]
		public short MaxHp { get; } = 100;

		private short hp;
		public short Hp
		{
			get { return hp; }
			set
			{
				if (fixHp) return;

				if (undead && value <= 1) hp = 1;
				else if (!undead && value <= 0) hp = 0;
				else if (value >= MaxHp) hp = MaxHp;
				else hp = value;
			}
		}

		private void Start()
		{
			undead = ConfigManager.Instance.GetConfig().undeadMode;
			Hp = MaxHp;
		}

		public void FixHp()
		{
			Hp = MaxHp;
			fixHp = true;
		}
	}
}
