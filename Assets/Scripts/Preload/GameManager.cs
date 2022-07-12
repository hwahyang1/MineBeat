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
		private GameObject canvas;

		[SerializeField]
		private GameObject managersObject;

		[SerializeField]
		private SceneChange sceneChange;

		private void Start()
		{
			DontDestroyOnLoad(canvas);
			DontDestroyOnLoad(managersObject);

			StartCoroutine("DelayedStart");
		}

		public IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(1f);

			sceneChange.ChangeScene("ModeSelectScene", false, true);
		}
	}
}
