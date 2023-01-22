using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.Config.UI.Elements.Audio
{
	/// <summary>
	/// 마스터 음량 선택을 관리합니다.
	/// </summary>
	public class Master : SliderCategoryElement
	{
		protected override void Start()
		{
			currentValue = ConfigManager.Instance.GetConfig().Master;
			base.Start();
		}

		public override void OnValueChanged(float currentValue)
		{
			base.OnValueChanged(currentValue);
			RootConfig config = ConfigManager.Instance.GetConfig();
			float adjustedValue = Mathf.Floor(currentValue * 100) / 100f;
			config.Master = adjustedValue;
			ConfigManager.Instance.SetConfig(config);
		}
	}
}
