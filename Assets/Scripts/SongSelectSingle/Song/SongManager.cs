using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MineBeat.SongSelectSingle.UI;
using MineBeat.SongSelectSingle.Score;
using MineBeat.SongSelectSingle.Extern;
using MineBeat.SongSelectSingle.KeyInput;

using MineBeat.Preload.Song;

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
		private HotKeyInputManager hotKeyInputManager;
		private ScoreHistoryManager scoreHistoryManager;

		private void Start()
		{
			songs = PackageManager.Instance.GetAllPackageId();

			songDetail = GameObject.Find("UIManagers").GetComponent<SongDetail>();
			previewSong = gameObject.GetComponent<PreviewSong>();
			songListDisplay = GameObject.Find("UIManagers").GetComponent<SongListDisplay>();
			hotKeyInputManager = GameObject.Find("HotKeyInputManager").GetComponent<HotKeyInputManager>();
			scoreHistoryManager = GameObject.Find("ScoreHistoryManager").GetComponent<ScoreHistoryManager>();

			GameObject selectedSongInfo = GameObject.Find("SelectedSongInfo");
			if (selectedSongInfo == null)
			{
				selected = Mathf.CeilToInt(songs.Count / 2f) - 1;
			}
			else
			{
				ulong target = selectedSongInfo.GetComponent<SelectedSongInfo>().id;
				for (int i = 0; i < songs.Count; i++)
				{
					if (songs[i] == target)
					{
						selected = i;
						break;
					}
				}
				Destroy(selectedSongInfo);
			}
			UpdateData();
		}

		/*
		 * [Method] Enter(): void
		 * 선택된 곡으로 게임을 실행합니다.
		 */
		public void Enter()
		{
			SelectedSongInfo selectedSongInfo = new GameObject("SelectedSongInfo").AddComponent<SelectedSongInfo>();
			selectedSongInfo.id = songs[selected];
			hotKeyInputManager.ChangeScene("InGameSingleScene");
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
			UpdateData();
		}

		/*
		 * [Method] SelectedUp(): void
		 * 현재 선택된 항목 바로 위의 항목을 선택합니다.
		 */
		public void SelectedUp()
		{
			if (selected == 0) selected = songs.Count - 1;
			else selected--;

			UpdateData();
		}

		/*
		 * [Method] SelectedUp(): void
		 * 현재 선택된 항목 바로 아래의 항목을 선택합니다.
		 */
		public void SelectedDown()
		{
			if (selected == songs.Count - 1) selected = 0;
			else selected++;

			UpdateData();
		}

		/*
		 * [Method] async UpdateData(): void
		 * 현재 선택된 항목에 맞춰 화면과 미리듣기를 갱신합니다.
		 */
		private async void UpdateData()
		{
			if (scoreHistoryManager == null) await System.Threading.Tasks.Task.Delay(5);

			SongInfo songInfo = PackageManager.Instance.GetSongInfo(songs[selected]);
			var medias = PackageManager.Instance.GetMedias(songs[selected]);
			float[] timecodes = new float[] { songInfo.notes.Find(target => target.type == NoteType.PreviewS).timeCode, songInfo.notes.Find(target => target.type == NoteType.PreviewE).timeCode };

			songListDisplay.Display(songs, selected);

			songDetail.UpdateInfo(songInfo, scoreHistoryManager.GetHistory(songs[selected]), medias.Item1);
			previewSong.Play(medias.Item2, timecodes);
		}
	}
}
