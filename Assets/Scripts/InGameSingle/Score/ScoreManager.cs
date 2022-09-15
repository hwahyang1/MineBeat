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
		private uint _score = 0;
		public uint score
		{
			get { return _score; }
			private set { _score = value; }
		}

		private uint _combo = 0;
		public uint combo
		{
			get { return _combo; }
			private set { _combo = value; }
		}

		private uint _maxCombo = 0;
		public uint maxCombo
		{
			get { return _maxCombo; }
			private set { _maxCombo = value; }
		}

		/// <summary>
		/// 콤보 수를 변경합니다.
		/// </summary>
		/// <param name="num">변경할 콤보를 입력합니다.</param>
		public void ChangeCombo(uint num)
		{
			combo = num;
			if (combo >= maxCombo) maxCombo = combo;
		}

		/// <summary>
		/// 점수를 추가합니다.
		/// </summary>
		/// <param name="num">추가할 점수를 입력합니다.</param>
		public void AddScore(uint num)
		{
			score += num;
		}
	}
}
