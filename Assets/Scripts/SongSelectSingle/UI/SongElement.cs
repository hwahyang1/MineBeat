using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.SongSelectSingle.Song;

namespace MineBeat.SongSelectSingle.UI
{
	/// <summary>
	/// UI 우측 곡 목록의 각 항목을 관리합니다.
	/// </summary>
	public class SongElement : MonoBehaviour
	{
		private SongManager songManager;

		// 목록에서의 순서
		private int order = -1;
		public int Order
		{
			get { return order; }
			set { if (order == -1) order = value; }
		}

		private ulong id = 0;
		public ulong ID
		{
			get { return id; }
			set { if (id == 0) id = value; }
		}

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			songManager = managers.Find(target => target.name == "SongManager").GetComponent<SongManager>();
		}

		/// <summary>
		/// 곡 정보를 설정합니다.
		/// </summary>
		/// <param name="name">곡 이름을 입력합니다.</param>
		/// <param name="author">작곡가를 입력합니다.</param>
		/// <param name="rank">랭크를 입력합니다.</param>
		public void Set(string name, string author, PlayRank rank)
		{
			gameObject.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0} <size=28>{1}</size>", name, author);
			gameObject.transform.GetChild(1).GetComponent<Text>().text = rank == PlayRank.X ? "-" : rank.ToString();
		}

		/* Events */
		public void Clicked()
		{
			songManager.SelectedChage(Order);
		}
		public void Enter()
		{
			songManager.Enter();
		}
	}
}
