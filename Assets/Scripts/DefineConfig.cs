using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

namespace MineBeat
{
	/// <summary>
	/// 디스플레이 모드를 지정합니다.
	/// </summary>
	[System.Serializable]
	public enum DisplayMode
	{
		Fullscreen,
		BorderLessFullscreen,
		Windowed
	}

	/// <summary>
	/// 해상도의 높이를 지정합니다.
	/// Reference: https://ko.wikipedia.org/wiki/16:9
	/// </summary>
	[System.Serializable]
	public enum ResolutionHeight
	{
		_720, // HD
		_900, // HD+
		_1080, // FHD
		_1152, // QWXGA
		_1440, // QHD
		_2160, // 4K UHD
	}

	/// <summary>
	/// 안티앨리어싱을 지정합니다.
	/// </summary>
	[System.Serializable]
	public enum AntiAliasing
	{
		None,
		MSAA2X,
		MSAA4X,
		MSAA8X
	}

	/// <summary>
	/// 초당 프레임을 지정합니다.
	/// </summary>
	[System.Serializable]
	public enum FrameRate
	{
		_30,
		_60,
		_120,
		_144,
		Infinity
	}

	/// <summary>
	/// 모든 설정값을 담습니다.
	/// </summary>
	[System.Serializable]
	public class RootConfig
	{
		/* Graphic Settings */
		public DisplayMode displayMode;
		public ResolutionHeight resolutionHeight;
		public AntiAliasing antiAliasing;
		public FrameRate frameRate;
		public bool vSync;
		public bool fpsCounter;

		/* Audio Settings */
		[SerializeField] private float master;
		public float Master
		{
			get { return master; }
			set
			{
				if (value <= 0f) master = 0f;
				else if (value >= 1f) master = 1f;
				else master = value;
			}
		}
		[SerializeField] private float background;
		public float Background
		{
			get { return background; }
			set
			{
				if (value <= 0f) background = 0f;
				else if (value >= 1f) background = 1f;
				else background = value;
			}
		}
		[SerializeField] private float effect;
		public float Effect
		{
			get { return effect; }
			set
			{
				if (value <= 0f) effect = 0f;
				else if (value >= 1f) effect = 1f;
				else effect = value;
			}
		}

		/* Game Settings */
		public bool skipInGameOpening;
		public bool skipResult;
		public bool undeadMode;

		/* Input Settings */

		/* Constructor */
		public RootConfig()
		{
			displayMode = DisplayMode.Windowed;
			resolutionHeight = ResolutionHeight._720;
			antiAliasing = AntiAliasing.None;
			frameRate = FrameRate._60;
			vSync = true;
			fpsCounter = false;
			Master = 1f;
			Background = 1f;
			Effect = 1f;
			skipInGameOpening = false;
			skipResult = false;
			undeadMode = false;
		}
		public RootConfig(DisplayMode displayMode, ResolutionHeight resolutionHeight, AntiAliasing antiAliasing, FrameRate frameRate, bool vSync, bool fpsCounter, float master, float background, float effect, bool skipInGameOpening, bool skipResult, bool undeadMode)
		{
			this.displayMode = displayMode;
			this.resolutionHeight = resolutionHeight;
			this.antiAliasing = antiAliasing;
			this.frameRate = frameRate;
			this.vSync = vSync;
			this.fpsCounter = fpsCounter;
			this.Master = Master;
			this.Background = background;
			this.Effect = effect;
			this.skipInGameOpening = skipInGameOpening;
			this.skipResult = skipResult;
			this.undeadMode = undeadMode;
		}
	}
}