using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.Config.UI.Elements.Graphic
{
	/// <summary>
	/// FPS 카운터 선택을 관리합니다.
	/// </summary>
	public class FPSCount : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = ConfigManager.Instance.GetConfig().fpsCounter ? 1 : 0;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.fpsCounter = currentSelection == 1;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
