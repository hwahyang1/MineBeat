using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.UI;
using MineBeat.Preload.Scene;

/*
 * [Method] MineBeat.ModeSelect
 * Description
 */
namespace MineBeat.ModeSelect
{
	/*
	 * [Class] ChangeScene
	 * Desciption
	 */
	public class ChangeScene : MonoBehaviour
	{
		private SceneChange sceneChange;

		private void Start()
		{
			sceneChange = GameObject.Find("PreloadScene Managers").GetComponent<SceneChange>();
		}

		public void Change(string sceneName)
		{
			sceneChange.ChangeScene(sceneName);
		}
	}
}
