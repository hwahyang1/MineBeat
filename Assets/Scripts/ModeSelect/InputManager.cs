using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using MineBeat.Preload.UI;

namespace MineBeat.ModeSelect
{
	/// <summary>
	/// 단축키 입력을 받습니다.
	/// </summary>
	public class InputManager : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				AlertManager.Instance.Show("확인", "게임을 종료할까요?", AlertManager.AlertButtonType.Double, new string[] { "종료", "취소" }, ExitGame, null);
			}
		}

		private void ExitGame()
		{
			#if UNITY_EDITOR
				EditorApplication.ExecuteMenuItem("Edit/Play");
			#else
				Application.Quit();
			#endif
		}
	}
}
