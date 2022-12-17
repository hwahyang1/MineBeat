using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Scene;
using MineBeat.Preload.UI;

using MineBeat.PackageInfo.Files;

namespace MineBeat.PackageInfo.Inputs
{
	/// <summary>
	/// 단축키 입력을 관리합니다.
	/// </summary>
	public class HotkeyInputManager : MonoBehaviour
	{
		private FileManager fileManager;

		private void Start()
		{
			fileManager = GetComponent<FileManager>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				OnESCKeyPressed();
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				OnRKeyPressed();
			}
		}

		public void OnESCKeyPressed()
		{
			AlertManager.Instance.Show("확인", "모드 선택 화면으로 돌아갈까요?", AlertManager.AlertButtonType.Double, new string[] { "예", "아니요" }, ExitScene, null);
		}

		private void ExitScene()
		{
			SceneChange.Instance.ChangeScene("ModeSelectScene");
		}

		public void OnRKeyPressed()
		{
			fileManager.OpenPackageSelector();
		}
	}
}
