using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.Preload.UI;

namespace MineBeat.ModeSelect
{
	/// <summary>
	/// 단축키 입력을 받습니다.
	/// </summary>
	public class InputManager : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				AlertManager.Instance.Show("확인")
			}
		}
	}
}
