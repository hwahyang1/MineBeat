using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*
 * [Namespace] MineBeat.SongSelectSingle.UI
 * Description
 */
namespace MineBeat.SongSelectSingle.UI
{
	/*
	 * [Class] SongDetail
	 * UI 좌측 현재 곡 정보를 띄워줍니다.
	 */
	public class SongDetail : MonoBehaviour
	{
		[SerializeField]
		private Transform songInfoArea;
		[SerializeField]
		private Transform recordArea;

		/*
		 * [Method] UpdateInfo(SongInfo song, PlayHistory history, Sprite cover = null): void
		 * 곡 정보를 갱신합니다.
		 * 
		 * <SongInfo song>
		 * 곡 정보를 입력합니다.
		 * 
		 * <PlayHistory history>
		 * 과거의 플레이 기록을 입력합니다.
		 * 
		 * <Sprite cover = null>
		 * 커버이미지를 입력합니다.
		 * 입력되지 않을 경우(null), 기본 이미지를 출력합니다.
		 */
		public void UpdateInfo(SongInfo song, PlayHistory history, Sprite cover = null)
		{
			songInfoArea.GetChild(0).GetComponent<Image>().sprite = cover;
			songInfoArea.GetChild(1).GetComponent<Text>().text = song.songName;
			songInfoArea.GetChild(2).GetComponent<Text>().text = song.songAuthor;
			songInfoArea.GetChild(3).GetComponent<Text>().text = string.Format("Level {0:00}", song.songLevel);

			recordArea.GetChild(0).GetComponent<Text>().text = history.rank == PlayRank.X ? "-" : history.rank.ToString();
			recordArea.GetChild(2).GetComponent<Text>().text = string.Format("{0:D6}", history.score);
			recordArea.GetChild(4).GetComponent<Text>().text = string.Format("{0:D4}", history.maxCombo);
		}
	}
}
