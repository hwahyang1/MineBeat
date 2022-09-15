using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat
{
	/// <summary>
	/// 싱글톤 패턴 템플릿입니다.
	/// © stageroad0820 (cloudysolar)
	/// </summary>
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();

					if (instance == null)
					{
						instance = new GameObject("Singleton_" + typeof(T).Name).AddComponent<T>();
					}
				}

				return instance;
			}
		}

		/// <summary>
		/// 싱글톤 디자인 패턴을 적용하기 위해 Awake() 메소드 실행문 맨 앞에 <c>base.Awake()</c>를 꼭 붙여주세요.
		/// </summary>
		protected virtual void Awake()
		{
			var inst = FindObjectsOfType<T>();

			if (inst.Length > 1)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
		}
	}
}
