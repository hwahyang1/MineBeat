using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.Config;

/*
 * [Namespace] MineBeat.Config.UI.Elements.Audio
 * '오디오 설정' 카테고리 항목에 대한 이벤트 처리 부분입니다.
 */
namespace MineBeat.Config.UI.Elements.Audio
{
	/*
	 * [Class] Effect
	 * 효과음 음량 선택을 관리합니다.
	 */
	public class Effect : SliderCategoryElement
	{
		protected override void Start()
		{
			currentValue = ConfigManager.Instance.GetConfig().effect;
			base.Start();
		}

		public override void OnValueChanged(float currentValue)
		{
			base.OnValueChanged(currentValue);
			RootConfig config = ConfigManager.Instance.GetConfig();
			config.effect = currentValue;
			ConfigManager.Instance.SetConfig(config);
		}
	}
}
