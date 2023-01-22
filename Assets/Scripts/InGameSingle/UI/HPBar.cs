using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.InGameSingle.HP;

namespace MineBeat.InGameSingle.UI
{
	/// <summary>
	/// 체력바 표기를 관리합니다.
	/// </summary>
	public class HPBar : MonoBehaviour
	{
		[SerializeField]
		private Text hpText;
		[SerializeField]
		private Slider hpSlider;

		private HPManager hpManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			hpManager = managers.Find(target => target.name == "HPManager").GetComponent<HPManager>();

			hpText.text = "HP: " + hpManager.Hp;
			hpSlider.value = 1f;
		}

		private void Update()
		{
			hpText.text = "HP: " + hpManager.Hp;
			hpSlider.value = Mathf.Lerp(hpSlider.value, hpManager.Hp / (float)hpManager.MaxHp, 2.5f * Time.deltaTime);
			//hpSlider.value = hpManager.hp / hpManager.maxHp;
		}
	}
}
