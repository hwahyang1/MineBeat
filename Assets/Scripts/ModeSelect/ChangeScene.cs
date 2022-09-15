using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.UI;
using MineBeat.Preload.Scene;

namespace MineBeat.ModeSelect
{
	/// <summary>
	/// Description
	/// </summary>
	public class ChangeScene : MonoBehaviour
	{
		public void Change(string sceneName)
		{
			if (sceneName == "GameEditorScene") AlertManager.Instance.Show("확인", "게임 에디터는 개발용으로 설계되었습니다.\n에디터에 대한 충분한 설명을 받은 이후에 진입하시기 바랍니다.", AlertManager.AlertButtonType.Double, new string[] { "계속", "취소" }, () => { SceneChange.Instance.ChangeScene(sceneName); }, () => { });
			else SceneChange.Instance.ChangeScene(sceneName);
		}
	}
}
