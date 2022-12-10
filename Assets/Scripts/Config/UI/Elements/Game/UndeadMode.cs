using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

namespace MineBeat.Config.UI.Elements.Game
{
	/// <summary>
	/// 무적 모드 선택을 관리합니다.
	/// </summary>
	public class UndeadMode : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = ConfigManager.Instance.GetConfig().undeadMode ? 1 : 0;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.undeadMode = currentSelection == 1;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
