using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.UI;
using MineBeat.SongSelectSingle.Score;

/*
 * [Namespace] MineBeat.SongSelectSingle.Song
 * Description
 */
namespace MineBeat.SongSelectSingle.Song
{
	/*
	 * [Class] SongManager
	 * 곡 선택을 관리합니다.
	 */
	public class SongManager : MonoBehaviour
	{
		private int selected = 0;
		private List<ulong> songs = new List<ulong>();

		private SongDetail songDetail;
		private PreviewSong previewSong;
		private SongListDisplay songListDisplay;
		private ScoreHistoryManager scoreHistoryManager;

		private void Start()
		{
			songs = PackageManager.Instance.GetAllPackageId();

			songListDisplay = GameObject.Find("UIManagers").GetComponent<SongListDisplay>();

			songListDisplay.Display(songs, selected);
		}

		/*
		 * [Method] SelectedChange(int i): void
		 * 현재 선택된 항목을 변경합니다.
		 * 
		 * <int i>
		 * 변경할 항목의 위치를 지정합니다.
		 */
		public void SelectedChage(int i)
		{
			selected = i;
			songListDisplay.Display(songs, selected);
		}

		/*
		 * [Method] SelectedUp(): void
		 * 현재 선택된 항목 바로 위의 항목을 선택합니다.
		 */
		public void SelectedUp()
		{
			if (selected == songs.Count - 1) selected = 0;
			else selected++;

			songListDisplay.Display(songs, selected);
		}

		/*
		 * [Method] SelectedUp(): void
		 * 현재 선택된 항목 바로 아래의 항목을 선택합니다.
		 */
		public void SelectedDown()
		{
			if (selected == 0) selected = songs.Count - 1;
			else selected--;

			songListDisplay.Display(songs, selected);
		}
	}
}
