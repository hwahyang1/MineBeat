using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Namespace] MineBeat.Config.UI.Elements
 * Description
 */
namespace MineBeat.Config.UI.Elements
{
	/*
	 * [Class] SliderCategoryElement
	 * 슬라이더 항목 - 카테고리 각 항목을 관리합니다.
	 */
	public abstract class SliderCategoryElement : CategoryElement
	{
		[SerializeField]
		protected float minValue = 0f;
		[SerializeField]
		protected float maxValue = 1f;
		protected float currentValue = 0f;

		protected GameObject elementName;
		protected GameObject elementSlider;
		protected GameObject elementValue;

		protected override void Awake()
		{
			base.Awake();

			elementName = transform.GetChild(0).gameObject;
			elementSlider = transform.GetChild(1).gameObject;
			elementValue = transform.GetChild(2).gameObject;

			elementSlider.GetComponent<Slider>().onValueChanged.AddListener(OnValueChanged);
		}

		/// <summary>
		/// 해당 Method는 minValue, maxValue, currentValue 변수를 기준으로 UI를 업데이트 시킵니다.
		/// 모든 작업이 완료된 후 base.Start()를 호출합니다.
		/// </summary>
		protected virtual void Start()
		{
			elementSlider.GetComponent<Slider>().minValue = minValue;
			elementSlider.GetComponent<Slider>().maxValue = maxValue;
			elementSlider.GetComponent<Slider>().value = currentValue;
		}

		/*
		 * [Method] OnValueChanged(): void
		 * 선택 값이 바뀌었을 때 이벤트를 처리합니다.
		 */
		public virtual void OnValueChanged(float currentValue)
		{
			this.currentValue = currentValue;
			elementValue.GetComponent<Text>().text = (int)((currentValue - minValue) / (maxValue - minValue) * 100) + "%";
		}
	}
}
