#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/*
 * [Namespace] MineBeat
 * Description
 */
namespace MineBeat
{
	/*
	 * [Class] DefaultSceneLoader
	 * UnityEditor에서 PlayMode 시작 시, 시작되는 Scene을 변경합니다.
	 */
	[InitializeOnLoadAttribute]
	public static class DefaultSceneLoader
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void InitScene()
		{
			if (SceneManager.GetActiveScene().name.CompareTo("PreloadScene") != 0)
			{
				SceneManager.LoadScene("PreloadScene");
			}
		}
	}
}
#endif