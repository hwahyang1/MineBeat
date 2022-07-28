using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] Minebeat
 * Desciption
 */
namespace MineBeat
{
	/*
	 * [Enum] DisplayMode
	 * 디스플레이 모드를 지정합니다.
	 */
	[System.Serializable]
	public enum DisplayMode
	{
		Fullscreen_Window,
		Windowed
	}

	/*
	 * [Enum] ResolutionHeight
	 * 해상도의 높이를 지정합니다.
	 * 
	 * Reference: https://ko.wikipedia.org/wiki/16:9
	 */
	[System.Serializable]
	public enum ResolutionHeight
	{
		_540, // qHD
		_720, // HD
		_900, // HD+
		_1080, // FHD
		_1152, // QWXGA
		_1440, // QHD
		_2160, // 4K UHD
		_2304, // HWXGA
		_4320 // 8K UHD
	}

	/*
	 * [Enum] AntiAliasing
	 * 안티앨리어싱을 지정합니다.
	 */
	[System.Serializable]
	public enum AntiAliasing
	{
		None,
		MSAA2x,
		MSAA4x,
		MSAA8x
	}

	/*
	 * [Enum] FrameRate
	 * 초당 프레임을 지정합니다.
	 */
	[System.Serializable]
	public enum FrameRate
	{
		_30,
		_60,
		_120,
		_144,
		Infinity
	}

	/*
	 * [Class] RootConfig
	 * 모든 설정값을 담습니다.
	 */
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
		[SerializeField] private float _master;
		public float master
		{
			get { return _master; }
			set
			{
				if (value <= 0f) _master = 0f;
				else if (value >= 1f) _master = 1f;
				else _master = value;
			}
		}
		[SerializeField] private float _background;
		public float background
		{
			get { return _background; }
			set
			{
				if (value <= 0f) _background = 0f;
				else if (value >= 1f) _background = 1f;
				else _background = value;
			}
		}
		[SerializeField] private float _effect;
		public float effect
		{
			get { return _effect; }
			set
			{
				if (value <= 0f) _effect = 0f;
				else if (value >= 1f) _effect = 1f;
				else _effect = value;
			}
		}

		/* Game Settings */
		public bool skipInGameOpening;
		public bool skipResult;

		/* Input Settings */

		/* Constructor */
		public RootConfig()
		{
			displayMode = DisplayMode.Windowed;
			resolutionHeight = ResolutionHeight._720;
			antiAliasing = AntiAliasing.None;
			frameRate = FrameRate._60;
			vSync = false;
			fpsCounter = false;
			master = 1f;
			background = 1f;
			effect = 1f;
			skipInGameOpening = false;
			skipResult = false;
		}
		public RootConfig(DisplayMode displayMode, ResolutionHeight resolutionHeight, AntiAliasing antiAliasing, FrameRate frameRate, bool vSync, bool fpsCounter, float master, float background, float effect, bool skipInGameOpening, bool skipResult)
		{
			this.displayMode = displayMode;
			this.resolutionHeight = resolutionHeight;
			this.antiAliasing = antiAliasing;
			this.frameRate = frameRate;
			this.vSync = vSync;
			this.fpsCounter = fpsCounter;
			this.master = master;
			this.background = background;
			this.effect = effect;
			this.skipInGameOpening = skipInGameOpening;
			this.skipResult = skipResult;
		}
	}
}