using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

/*
 * [Namespace] MineBeat.Config.UI.Elements.Game
 * '게임 설정' 카테고리 항목에 대한 이벤트 처리 부분입니다.
 */
namespace MineBeat.Config.UI.Elements.Game
{
	/*
	 * [Class] SkipOpening
	 * 오프닝 건너뛰기 선택을 관리합니다.
	 */
	public class SkipOpening : SelectCategoryElement
	{
		protected override void Start()
		{
			currentSelection = ConfigManager.Instance.GetConfig().skipInGameOpening ? 1 : 0;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.skipInGameOpening = currentSelection == 1;
			ConfigManager.Instance.SetConfig(config);
			base.OnValueChanged();
		}
	}
}
