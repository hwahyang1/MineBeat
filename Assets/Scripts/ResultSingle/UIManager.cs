using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace MineBeat.ResultSingle
{
	/// <summary>
	/// UI 노출을 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private Transform top;
		[SerializeField]
		private Transform middleLeft;
		[SerializeField]
		private Transform middleCenter;
		[SerializeField]
		private Transform middleRight;

		private ScoreManager scoreManager;
		
		private void Start()
		{
			scoreManager = GetComponent<ScoreManager>();

			SongInfo songInfo = scoreManager.GetSongInfo();
			top.GetChild(0).GetComponent<Text>().text = songInfo.songName;
			top.GetChild(1).GetComponent<Text>().text = songInfo.songAuthor;
			top.GetChild(2).GetComponent<Text>().text = string.Format("Level {0:D2}", songInfo.songLevel);

			var scoreComboHistory = scoreManager.GetScoreComboHistory();
			middleLeft.GetChild(1).GetComponent<Text>().text = string.Format("{0:D4}", scoreComboHistory[1].Item2);
			middleLeft.GetChild(3).GetComponent<Text>().text = string.Format("{0:D4}", scoreComboHistory[1].Item1);
			middleLeft.GetChild(4).gameObject.SetActive(scoreComboHistory[1].Item3);

			middleCenter.GetChild(1).GetComponent<Text>().text = string.Format("{0:D6}", scoreComboHistory[0].Item2);
			middleCenter.GetChild(3).GetComponent<Text>().text = string.Format("{0:D6}", scoreComboHistory[0].Item1);
			middleCenter.GetChild(4).gameObject.SetActive(scoreComboHistory[0].Item3);

			middleRight.GetChild(1).GetComponent<Text>().text = scoreManager.GetPlayRank().ToString();
			middleRight.GetChild(2).gameObject.SetActive(scoreComboHistory[0].Item3);
		}
	}
}
