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
	 * [Class] FrameRate
	 * 초당 프레임 제한 선택을 관리합니다.
	 */
	public class FrameRate : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = (int)ConfigManager.Instance.GetConfig().frameRate;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.frameRate = (MineBeat.FrameRate)currentSelection;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
