using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.Config.UI.Elements.Audio
{
	/// <summary>
	/// 배경음 음량 선택을 관리합니다.
	/// </summary>
	public class Background : SliderCategoryElement
	{
		protected override void Start()
		{
			currentValue = ConfigManager.Instance.GetConfig().Background;
			base.Start();
		}

		public override void OnValueChanged(float currentValue)
		{
			base.OnValueChanged(currentValue);
			RootConfig config = ConfigManager.Instance.GetConfig();
			float adjustedValue = Mathf.Floor(currentValue * 100) / 100f;
			config.Background = adjustedValue;
			ConfigManager.Instance.SetConfig(config);
		}
	}
}
