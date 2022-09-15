using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.Config.UI.Elements
{
	/// <summary>
	/// 공통 - 카테고리 각 항목을 관리합니다.
	/// </summary>
	public abstract class CategoryElement : MonoBehaviour
	{
		[SerializeField]
		protected string description = "작성된 설명이 없습니다.";

		private Categories categories;

		protected virtual void Awake()
		{
			description = description.Replace(@"\n", "\n");

			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));
			categories = managers.Find(target => target.name == "CategoryManager").GetComponent<Categories>();
		}

		/// <summary>
		/// 설명을 반환합니다.
		/// </summary>
		/// <returns></returns>
		public string GetDescription()
		{
			return description;
		}

		private void OnMouseOver()
		{
			categories.ChangeDescription(description);
		}

		private void OnMouseExit()
		{
			categories.ChangeDescription();
		}
	}
}
