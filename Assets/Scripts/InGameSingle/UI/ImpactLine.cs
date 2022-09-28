using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace MineBeat.InGameSingle.UI
{
	/// <summary>
	/// Impact Line의 연출을 관리합니다.
	/// </summary>
	public class ImpactLine : MonoBehaviour
	{
		[Header("Colors")]
		[SerializeField]
		private Color[] titleColors = new Color[2];

		[Header("GameObjects")]
		[SerializeField]
		private GameObject parent;
		[SerializeField]
		private Text title;
		[SerializeField]
		private Text description;

		private void Start()
		{
			parent.SetActive(false);
		}

		public void Run()
		{
			parent.SetActive(true);
			title.gameObject.SetActive(false);
			description.gameObject.SetActive(false);
			StartCoroutine("RunCoroutine");
		}

		public IEnumerator RunCoroutine()
		{
			title.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.25f);

			for (int i = 0; i < 4; i++)
			{
				title.color = titleColors[0];
				yield return new WaitForSeconds(0.1f);
				title.color = titleColors[1];
				yield return new WaitForSeconds(0.1f);
			}

			yield return new WaitForSeconds(0.75f);
			description.gameObject.SetActive(true);

			yield return new WaitForSeconds(2f);
			description.gameObject.SetActive(false);

			for (int i = 0; i < 4; i++)
			{
				title.color = titleColors[0];
				yield return new WaitForSeconds(0.1f);
				title.color = titleColors[1];
				yield return new WaitForSeconds(0.1f);
			}
			title.gameObject.SetActive(false);

			yield return new WaitForSeconds(0.25f);
			parent.SetActive(false);
		}
	}
}
