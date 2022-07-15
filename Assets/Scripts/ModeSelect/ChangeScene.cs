using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
		public void Change(string sceneName)
		{
			SceneChange.Instance.ChangeScene(sceneName);
		}
	}
}
