using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.SongSelectSingle.Song;

/*
 * [Namespace] MineBeat.SongSelectSingle.UI
 * Description
 */
namespace MineBeat.SongSelectSingle.UI
{
	/*
	 * [Class] SongElement
	 * UI 우측 곡 목록의 각 항목을 관리합니다.
	 */
	public class SongElement : MonoBehaviour
	{
		private SongManager songManager;

		// 목록에서의 순서
		private int _order = -1;
		public int order
		{
			get { return _order; }
			set { if (_order == -1) _order = value; }
		}

		private ulong _id = 0;
		public ulong id
		{
			get { return _id; }
			set { if (_id == 0) _id = value; }
		}

		private void Start()
		{
			songManager = GameObject.Find("SongManager").GetComponent<SongManager>();
		}

		/*
		 * [Method] Set(string name, string author, PlayRank rank): void
		 * 곡 정보를 설정합니다.
		 * 
		 * <string name>
		 * 곡 이름을 입력합니다.
		 * 
		 * <string author>
		 * 작곡가를 입력합니다.
		 * 
		 * <PlayRank rank>
		 * 랭크를 입력합니다.
		 */
		public void Set(string name, string author, PlayRank rank)
		{
			gameObject.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0} <size=28>{1}</size>", name, author);
			gameObject.transform.GetChild(1).GetComponent<Text>().text = rank == PlayRank.X ? "-" : rank.ToString();
		}

		/*
		 * [Method] Clicked(): void
		 * 이 클래스가 붙은 GameObject(Button)이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void Clicked()
		{
			songManager.SelectedChage(order);
		}

		/*
		 * [Method] Enter(): void
		 * 이 클래스가 붙은 GameObject(Button)이 클릭되었을 때 이벤트를 처리합니다.
		 */
		public void Enter()
		{
			songManager.Enter();
		}
	}
}
