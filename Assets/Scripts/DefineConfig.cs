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
	enum DisplayMode
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
	enum ResolutionHeight
	{
		_540, // qHD
		_720, // HD
		_900, // HD+
		_1080, // FHD
		_1152, // QWXGA
		_1440, // QHD
		_2160, // 4K UHD
		_2304, // HWXGA
		_4320, // 8K UHD
	}

	/*
	 * [Enum] AntiAliasing
	 * 안티앨리어싱을 지정합니다.
	 */
	enum AntiAliasing
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
	enum FrameRate
	{

	}

	/*
	 * [Class] RootConfig
	 * 모든 설정값을 담습니다.
	 */
	class RootConfig
	{
		public DisplayMode displayMode;
		public ResolutionHeight resolutionHeight;
		public AntiAliasing antiAliasing;
		public FrameRate frameRate;
	}
}