using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.Preload.Song;
using MineBeat.SongSelectSingle.Extern;

/*
 * [Namespace] MineBeat.InGameSingle.UI
 * Description
 */
namespace MineBeat.InGameSingle.UI
{
	/*
	 * [Class] SongInfoManager
	 * 곡 정보 표기를 관리합니다.
	 */
	public class SongInfoManager : MonoBehaviour
	{
		[SerializeField]
		private Transform top_SongInfo;
		[SerializeField]
		private Transform bottom_CopyRight;

		private SelectedSongInfo selectedSongInfo;

		private void Start()
		{
			selectedSongInfo = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>();
			SongInfo songInfo = PackageManager.Instance.GetSongInfo(selectedSongInfo.id);

			top_SongInfo.GetChild(0).GetComponent<Text>().text = songInfo.songName;
			top_SongInfo.GetChild(1).GetComponent<Text>().text = songInfo.songAuthor;

			bottom_CopyRight.GetChild(0).GetComponent<Text>().text = string.Format("© {0}.", songInfo.songAuthor);
		}
	}
}
