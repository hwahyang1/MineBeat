using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

/*
 * [Namespace] MineBeat.Config.UI.Elements.Graphic
 * '그래픽 설정' 카테고리 항목에 대한 이벤트 처리 부분입니다.
 */
namespace MineBeat.Config.UI.Elements.Graphic
{
	/*
	 * [Class] VSync
	 * 수직동기화 선택을 관리합니다.
	 */
	public class VSync : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = ConfigManager.Instance.GetConfig().vSync ? 1 : 0;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.vSync = currentSelection == 1;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
