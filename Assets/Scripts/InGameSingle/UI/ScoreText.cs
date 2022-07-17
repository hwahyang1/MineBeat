using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.InGameSingle.Score;

/*
 * [Namespace] MineBeat.InGameSingle.UI
 * Description
 */
namespace MineBeat.InGameSingle.UI
{
	/*
	 * [Class] ScoreText
	 * 현재 점수 표기를 관리합니다.
	 */
	public class ScoreText : MonoBehaviour
	{
		[SerializeField]
		private Text score;

		private ScoreManager scoreManager;

		private void Start()
		{
			scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		}

		private void Update()
		{
			score.text = string.Format("{0:D6}", scoreManager.score);
		}
	}
}
