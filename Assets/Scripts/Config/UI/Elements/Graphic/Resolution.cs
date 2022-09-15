using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.Config.UI.Elements.Graphic
{
	/// <summary>
	/// 해상도 선택을 관리합니다.
	/// </summary>
	public class Resolution : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = (int)ConfigManager.Instance.GetConfig().resolutionHeight;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.resolutionHeight = (ResolutionHeight)currentSelection;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
