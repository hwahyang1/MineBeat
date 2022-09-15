using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Scene;

namespace MineBeat.ResultSingle
{
	/// <summary>
	/// 단축키 입력을 처리합니다.
	/// </summary>
	public class HotKeyManager : MonoBehaviour
	{
		private bool blockInput = false;

		private void Update()
		{
			if (blockInput) return;

			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				OnEnterKeyPressed();
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				OnRestartKeyPressed();
			}
		}

		/* Key Events */
		public void OnEnterKeyPressed()
		{
			if (blockInput) return;

			//Destroy(GameObject.Find("SelectedSongInfo"));
			//Destroy(GameObject.Find("ScoreHistoryManager"));

			SceneChange.Instance.ChangeScene("SongSelectSingleScene");
			blockInput = true;
		}
		public void OnRestartKeyPressed()
		{
			if (blockInput) return;

			SceneChange.Instance.ChangeScene("InGameSingleScene");
			blockInput = true;
		}
	}
}
