#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace MineBeat
{
	/// <summary>
	/// UnityEditor에서 PlayMode 시작 시, 시작되는 Scene을 변경합니다.
	/// </summary>
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