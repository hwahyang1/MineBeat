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
	 * [Class] FPSCount
	 * FPS 카운터 선택을 관리합니다.
	 */
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
