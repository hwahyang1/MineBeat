using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

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
		private EventTrigger eventTrigger;

		protected virtual void Awake()
		{
			description = description.Replace(@"\n", "\n"); // 에디터상에서 줄바꿈이 안되네요..?

			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));
			categories = managers.Find(target => target.name == "CategoryManager").GetComponent<Categories>();

			eventTrigger = GetComponent<EventTrigger>();

			EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry() { eventID=EventTriggerType.PointerEnter };
			entry_PointerEnter.callback.AddListener((data) => { categories.ChangeDescription(description); });
			eventTrigger.triggers.Add(entry_PointerEnter);

			EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry() { eventID = EventTriggerType.PointerExit };
			entry_PointerExit.callback.AddListener((data) => { categories.ChangeDescription(); });
			eventTrigger.triggers.Add(entry_PointerExit);
		}

		/// <summary>
		/// 설명을 반환합니다.
		/// </summary>
		public string GetDescription() => description;
	}
}
