using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.U2D;

using MineBeat.Preload.Config;

namespace MineBeat.InGameSingle.PlayerCamera
{
	/// <summary>
	/// Pixel Perfect Camera의 Unit 값을 조정합니다.
	/// </summary>
	[RequireComponent(typeof(PixelPerfectCamera))]
	public class PixelPerfectCameraUnit : MonoBehaviour
	{
		private void Start()
		{
			int resolutionHeightIndex = (int)ConfigManager.Instance.GetConfig().resolutionHeight;
			GetComponent<PixelPerfectCamera>().assetsPPU = 40 + (resolutionHeightIndex * 10);
		}
	}
}
