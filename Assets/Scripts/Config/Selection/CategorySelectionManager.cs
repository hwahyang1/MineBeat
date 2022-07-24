using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.Config.Selection
 * Description
 */
namespace MineBeat.Config.Selection
{
	public enum Category
	{
		Graphic,
		Audio,
		Game,
		Input
	}

	/*
	 * [Class] CategorySelectionManager
	 * 카테고리 선택을 관리합니다.
	 */
	public class CategorySelectionManager : MonoBehaviour
	{
		private Category _selectedCategory = Category.Graphic;
		public Category selectedCategory
		{
			get { return _selectedCategory; }
			private set { _selectedCategory = value; }
		}

		/*
		 * [Method] ChangeCategory(bool isUpy): void
		 * 현재 카테고리를 올리거나 내립니다.
		 * 
		 * <bool isUp>
		 * 카테고리의 위치를 결정합니다.
		 * true면 위의 카테고리를 선택하고, false면 아래의 카테고리를 선택합니다.
		 */
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

		/*
		 * [Method] ChangeCategory(Category category): void
		 * 현재 카테고리를 변경합니다.
		 * 
		 * <Category category>
		 * 변경할 카테고리를 지정합니다.
		 */
		public void SetCategory(Category category)
		{
			selectedCategory = category;
		}
		/*
		 * [Method] ChangeCategory(int category): void
		 * 현재 카테고리를 변경합니다.
		 * 
		 * <int category>
		 * 변경할 카테고리를 지정합니다.
		 */
		public void SetCategory(int category)
		{
			if (category < 0 || category > (int)Category.Input) return;
			selectedCategory = (Category)category;
		}
	}
}
