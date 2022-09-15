using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.Preload.Config
{
	/// <summary>
	/// 설정 파일의 입출력과 설정 적용을 관리합니다.
	/// </summary>
	public class ConfigManager : Singleton<ConfigManager>
	{
		private string _configFilePath;
		private string configFilePath
		{
			get { return _configFilePath; }
			set { if (!protectModify) _configFilePath = value; }
		}
		private bool protectModify = false;

		private RootConfig rootConfig;

		private AudioSource backgroundSound;
		private AudioSource effectSound;

		[SerializeField]
		private GameObject fpsCanvas;
		
		protected override void Awake()
		{
			base.Awake();

			List<GameObject> audioSources = new List<GameObject>(GameObject.FindGameObjectsWithTag("AudioSource"));
			backgroundSound = audioSources.Find(target => target.name == "BackgroundSound").GetComponent<AudioSource>();
			effectSound = audioSources.Find(target => target.name == "EffectSound").GetComponent<AudioSource>();

			configFilePath = Application.persistentDataPath + @"\Minebeat.cfg";
			protectModify = true;

			LoadConfig();

			ApplyConfig();
		}

		/// <summary>
		/// 현재 설정값을 반환합니다.
		/// </summary>
		/// <returns></returns>
		public RootConfig GetConfig() => rootConfig;

		/// <summary>
		/// 현재 설정값을 갱신하고 저장합니다.
		/// </summary>
		/// <param name="data">갱신할 설정값을 입력합니다.</param>
		public void SetConfig(RootConfig data)
		{
			rootConfig = data;
			ApplyConfig();
			SaveConfig();
		}

		/// <summary>
		/// rootConfig 변수에 맞춰 설정을 적용합니다.
		/// </summary>
		private void ApplyConfig()
		{
			/* Graphic Settings */
			FullScreenMode fullScreenMode = (rootConfig.displayMode == DisplayMode.Fullscreen_Window) ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
			int resolutionHeight = 0;
			switch (rootConfig.resolutionHeight)
			{
				case ResolutionHeight._540:
					resolutionHeight = 540;
					break;
				case ResolutionHeight._720:
					resolutionHeight = 720;
					break;
				case ResolutionHeight._900:
					resolutionHeight = 900;
					break;
				case ResolutionHeight._1080:
					resolutionHeight = 1080;
					break;
				case ResolutionHeight._1152:
					resolutionHeight = 1152;
					break;
				case ResolutionHeight._1440:
					resolutionHeight = 1440;
					break;
				case ResolutionHeight._2160:
					resolutionHeight = 2160;
					break;
				case ResolutionHeight._2304:
					resolutionHeight = 2304;
					break;
				case ResolutionHeight._4320:
					resolutionHeight = 4320;
					break;
			}
			Screen.SetResolution(resolutionHeight / 9 * 16, resolutionHeight, fullScreenMode);

			int antiAliasing = 0;
			switch (rootConfig.antiAliasing)
			{
				case AntiAliasing.None:
					antiAliasing = 0;
					break;
				case AntiAliasing.MSAA2x:
					antiAliasing = 2;
					break;
				case AntiAliasing.MSAA4x:
					antiAliasing = 4;
					break;
				case AntiAliasing.MSAA8x:
					antiAliasing = 8;
					break;
			}
			QualitySettings.antiAliasing = antiAliasing;

			int frameRate = 0;
			switch (rootConfig.frameRate)
			{
				case FrameRate._30:
					frameRate = 30;
					break;
				case FrameRate._60:
					frameRate = 60;
					break;
				case FrameRate._120:
					frameRate = 120;
					break;
				case FrameRate._144:
					frameRate = 144;
					break;
				case FrameRate.Infinity:
					frameRate = -1;
					break;
			}
			Application.targetFrameRate = frameRate;

			QualitySettings.vSyncCount = rootConfig.vSync ? 1 : 0;
			fpsCanvas.SetActive(rootConfig.fpsCounter);

			/* Audio Settings */
			backgroundSound.volume = rootConfig.master * rootConfig.background;
			effectSound.volume = rootConfig.master * rootConfig.effect;

			/* Game Settings */

			/* Input Settings */
		}

		/// <summary>
		/// 데이터를 rootConfig로 불러옵니다.
		/// </summary>
		private void LoadConfig()
		{
			if (!File.Exists(configFilePath))
			{
				FileStream fs = File.Create(configFilePath);
				fs.Close();
				rootConfig = new RootConfig();
				SaveConfig();
				return;
			}

			string data = File.ReadAllText(configFilePath);
			rootConfig = JsonUtility.FromJson<RootConfig>(data);
		}

		/// <summary>
		/// rootConfig 데이터를 저장합니다.
		/// </summary>
		private void SaveConfig()
		{
			string data = JsonUtility.ToJson(rootConfig);
			File.WriteAllText(configFilePath, data);
		}
	}
}
