using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.Config.UI.Elements.Graphic
{
	/// <summary>
	/// 안티앨리어싱 선택을 관리합니다.
	/// </summary>
	public class AntiAliasing : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = (int)ConfigManager.Instance.GetConfig().antiAliasing;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.antiAliasing = (MineBeat.AntiAliasing)currentSelection;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
