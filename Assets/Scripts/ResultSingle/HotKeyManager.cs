using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Scene;

/*
 * [Namespace] MineBeat.ResultSingle
 * Description
 */
namespace MineBeat.ResultSingle
{
	/*
	 * [Class] HotKeyManager
	 * 단축키 입력을 처리합니다.
	 */
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

		/*
		 * [Method] OnEnterKeyPressed(): void
		 * [Method] OnRestartKeyPressed(): void
		 * 키 입력을 처리합니다.
		 */
		public void OnEnterKeyPressed()
		{
			if (blockInput) return;

			//Destroy(GameObject.Find("SelectedSongInfo"));

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
