using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.InGameSingle.Score
{
	/// <summary>
	/// 플레이어의 점수를 관리합니다.
	/// </summary>
	public class ScoreManager : MonoBehaviour
	{
		public uint Score { get; private set; } = 0;

		public uint Combo { get; private set; } = 0;

		public uint MaxCombo { get; private set; } = 0;

		/// <summary>
		/// 콤보 수를 변경합니다.
		/// </summary>
		/// <param name="num">변경할 콤보를 입력합니다.</param>
		public void ChangeCombo(uint num)
		{
			Combo = num;
			if (Combo >= MaxCombo) MaxCombo = Combo;
		}

		/// <summary>
		/// 점수를 추가합니다.
		/// </summary>
		/// <param name="num">추가할 점수를 입력합니다.</param>
		public void AddScore(uint num)
		{
			Score += num;
		}
	}
}
