using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.InGameSingle.HP;

/*
 * [Namespace] MineBeat.InGameSingle.UI
 * Description
 */
namespace MineBeat.InGameSingle.UI
{
	/*
	 * [Class] HPBar
	 * 체력바 표기를 관리합니다.
	 */
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

			hpText.text = "HP: " + hpManager.hp;
			hpSlider.value = 1f;
		}

		private void Update()
		{
			hpText.text = "HP: " + hpManager.hp;
			hpSlider.value = Mathf.Lerp(hpSlider.value, hpManager.hp / (float)hpManager.maxHp, 2.5f * Time.deltaTime);
			//hpSlider.value = hpManager.hp / hpManager.maxHp;
		}
	}
}
