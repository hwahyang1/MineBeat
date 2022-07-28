using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Scene;

/*
 * [Namespace] MineBeat.Preload
 * Description
 */
namespace MineBeat.Preload
{
	/*
	 * [Class] GameManager
	 * Scene의 전반적인 실행을 관리합니다.
	 */
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> canvases = new List<GameObject>();

		[SerializeField]
		private List<GameObject> soundObjects = new List<GameObject>();

		[SerializeField]
		private SceneChange sceneChange;

		private void Start()
		{
			foreach (GameObject obj in canvases)
			{
				DontDestroyOnLoad(obj);
			}

			foreach (GameObject obj in soundObjects)
			{
				DontDestroyOnLoad(obj);
			}

			StartCoroutine("DelayedStart");
		}

		public IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(1f);

			sceneChange.ChangeScene("ModeSelectScene", false, true);
		}
	}
}
