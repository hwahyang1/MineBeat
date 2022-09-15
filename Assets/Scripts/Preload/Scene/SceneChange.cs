using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MineBeat.Preload.Scene
{
	/// <summary>
	/// Scene 변경과 트랜지션을 관리합니다.
	/// </summary>
	public class SceneChange : Singleton<SceneChange>
	{
		[Header("GameObject (Canvas)")]
		[SerializeField]
		private GameObject canvas;
		[SerializeField]
		private Image loadingCover;
		[SerializeField]
		private Text loadingPercent;

		[Header("Unused GameObject (Canvas)")]
		[SerializeField]
		private GameObject cover;
		[SerializeField]
		private GameObject loadingText;

		[Header("Speed")]
		[SerializeField]
		private float transitionTime = 1f;

		/// <summary>
		/// Scene을 변경합니다.
		/// </summary>
		/// <param name="sceneName">변경할 Scene의 이름을 입력합니다.</param>
		/// <param name="fadeIn">Scene 변경 시 페이드 인 트랜지션을 적용 할 지 결정합니다.</param>
		/// <param name="fadeOut">Scene 변경 시 페이드 아웃 트랜지션을 적용 할 지 결정합니다.</param>
		public void ChangeScene(string sceneName, bool fadeIn = true, bool fadeOut = true)
		{
			StartCoroutine(ChangeSceneCoroutine(sceneName, fadeIn, fadeOut));
		}

		public IEnumerator ChangeSceneCoroutine(string sceneName, bool fadeIn, bool fadeOut)
		{
			yield return new WaitForSeconds(0.05f); // 직전에 AlertManager가 실행 중이었으면 알림창이 종료 될 때 canvas를 꺼버려서 페이드인이 안먹음

			loadingPercent.gameObject.SetActive(false);

			if (fadeIn || fadeOut)
			{
				cover.SetActive(false);
				loadingText.SetActive(false);

				loadingCover.color = new Color(1f, 1f, 1f, 0f);
				loadingCover.gameObject.SetActive(true);
				canvas.SetActive(true);
			}

			if (fadeIn)
			{
				float accumulateTime = 0f;
				while (loadingCover.color.a < 1f)
				{
					accumulateTime += Time.deltaTime;
					Color color = new Color(1f, 1f, 1f, accumulateTime / transitionTime);
					loadingCover.color = color;
					yield return null;
				}
				yield return new WaitForSeconds(0.25f);
			}

			if (fadeOut)
			{
				loadingPercent.gameObject.SetActive(true);
				loadingPercent.text = "0%";
				loadingCover.color = new Color(1f, 1f, 1f, 1f);
				yield return new WaitForSeconds(0.25f);
			}

			AsyncOperation sceneChange = SceneManager.LoadSceneAsync(sceneName);
			while (!sceneChange.isDone)
			{
				loadingPercent.text = string.Format("{0}%", Mathf.Round(sceneChange.progress * 1000) * 0.1f);
				yield return null;
			}

			loadingPercent.text = "100%";
			yield return new WaitForSeconds(0.2f);

			loadingPercent.gameObject.SetActive(false);
			loadingPercent.text = "0%";
			yield return new WaitForSeconds(0.05f);

			if (fadeOut)
			{
				float accumulateTime = 0f;
				while (loadingCover.color.a > 0f)
				{
					accumulateTime += Time.deltaTime;
					Color color = new Color(1f, 1f, 1f, 1f - (accumulateTime / transitionTime));
					loadingCover.color = color;
					yield return null;
				}
			}

			if (fadeIn || fadeOut)
			{
				cover.SetActive(true);
				loadingText.SetActive(true);
				loadingCover.gameObject.SetActive(false);
				canvas.SetActive(false);
			}
		}
	}
}
