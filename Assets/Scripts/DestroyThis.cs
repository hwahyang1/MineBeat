using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat
{
	/// <summary>
	/// 일정 시간 이후 이 스크립트가 붙은 GameObject를 Destroy 합니다.
	/// </summary>
	public class DestroyThis : MonoBehaviour
	{
		public float timeOut;

		private void Start()
		{
			StartCoroutine("TimeOut");
		}

		private IEnumerator TimeOut()
		{
			yield return new WaitForSeconds(timeOut);
			Destroy(gameObject);
		}
	}
}
