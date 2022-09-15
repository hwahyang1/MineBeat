using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace MineBeat.Preload.UI
{
	/// <summary>
	/// 현재 FPS를 표시합니다.
	/// </summary>
	public class FPSCounter : MonoBehaviour
	{
		private Text fpsText;
		private float deltaTime = 0.0f;

		private void Start()
		{
			fpsText = GetComponent<Text>();
		}

		private void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			float fps = 1.0f / deltaTime;
			fpsText.text = Mathf.Round(fps).ToString() + " FPS";
		}
	}
}
