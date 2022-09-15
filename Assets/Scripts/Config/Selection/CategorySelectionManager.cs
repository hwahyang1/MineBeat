using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.Config.Selection
{
	public enum Category
	{
		Graphic,
		Audio,
		Game,
		Input
	}

	/// <summary>
	/// 카테고리 선택을 관리합니다.
	/// </summary>
	public class CategorySelectionManager : MonoBehaviour
	{
		private Category _selectedCategory = Category.Graphic;
		public Category selectedCategory
		{
			get { return _selectedCategory; }
			private set { _selectedCategory = value; }
		}

		/// <summary>
		/// 현재 카테고리를 올리거나 내립니다.
		/// </summary>
		/// <param name="isUp">카테고리의 위치를 결정합니다. true면 위의 카테고리를 선택하고, false면 아래의 카테고리를 선택합니다.</param>
		public void ChangeCategory(bool isUp)
		{
			if (isUp)
			{
				if (selectedCategory == 0) return;
				selectedCategory -= 1;
			}
			else
			{
				if (selectedCategory == Category.Input) return;
				selectedCategory += 1;
			}
		}

		/// <summary>
		/// 현재 카테고리를 변경합니다.
		/// </summary>
		/// <param name="category">변경할 카테고리를 지정합니다.</param>
		public void SetCategory(Category category)
		{
			selectedCategory = category;
		}
		/// <summary>
		/// 현재 카테고리를 변경합니다.
		/// </summary>
		/// <param name="category">변경할 카테고리를 지정합니다.</param>
		public void SetCategory(int category)
		{
			if (category < 0 || category > (int)Category.Input) return;
			selectedCategory = (Category)category;
		}
	}
}
