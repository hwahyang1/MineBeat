using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.Config.UI.Elements
 * Description
 */
namespace MineBeat.Config.UI.Elements
{
	/*
	 * [Class] CategoryElement
	 * 공통 - 카테고리 각 항목을 관리합니다.
	 */
	public abstract class CategoryElement : MonoBehaviour
	{
		[SerializeField]
		protected string description = "작성된 설명이 없습니다.";

		/*
		 * [Method] GetDescription(): void
		 * 설명을 반환합니다.
		 */
		public string GetDescription()
		{
			return description;
		}
	}
}
