using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.InGameSingle.Score
 * Description
 */
namespace MineBeat.InGameSingle.Score
{
	/*
	 * [Class] ScoreManager
	 * 플레이어의 점수를 관리합니다.
	 */
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
	}
}
