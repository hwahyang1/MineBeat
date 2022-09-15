using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.InGameSingle.Score;

namespace MineBeat.InGameSingle.UI
{
	/// <summary>
	/// 현재 점수 표기를 관리합니다.
	/// </summary>
	public class ScoreText : MonoBehaviour
	{
		[SerializeField]
		private Text score;

		private ScoreManager scoreManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			scoreManager = managers.Find(target => target.name == "ScoreManager").GetComponent<ScoreManager>();
		}

		private void Update()
		{
			score.text = string.Format("{0:D6}", scoreManager.score);
		}
	}
}
