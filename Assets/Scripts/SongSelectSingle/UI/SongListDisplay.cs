using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using MineBeat.SongSelectSingle.Score;

using MineBeat.Preload.Song;

namespace MineBeat.SongSelectSingle.UI
{
	/// <summary>
	/// 곡 목록을 표시합니다.
	/// </summary>
	public class SongListDisplay : MonoBehaviour
	{
		[SerializeField]
		private Transform parent;

		[SerializeField]
		private GameObject normalPrefab;
		[SerializeField]
		private GameObject selectedPrefab;

		[SerializeField]
		private ScrollRect scrollRect;
		[SerializeField]
		private RectTransform contentPanel;

		private bool isFirst = true;

		private ScoreHistoryManager scoreHistoryManager;

		private void Start()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			scoreHistoryManager = managers.Find(target => target.name == "ScoreHistoryManager").GetComponent<ScoreHistoryManager>();
		}

		/// <summary>
		/// 곡 목록을 다시 그립니다.
		/// </summary>
		/// <param name="ids">곡의 목록을 입력합니다.</param>
		/// <param name="selected">현재 선택된 항목의 위치를 입력합니다. (ids 변수 기준)</param>
		public async void Display(List<ulong> ids, int selected)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Destroy(parent.GetChild(i).gameObject);
			}

			if (scoreHistoryManager == null) await System.Threading.Tasks.Task.Delay(5);

			for (int i = 0; i < ids.Count; i++)
			{
				GameObject generated = Instantiate(i == selected ? selectedPrefab : normalPrefab, parent.position, Quaternion.identity, parent);
				SongElement songElement = generated.GetComponent<SongElement>();
				SongInfo songInfo = PackageManager.Instance.GetSongInfo(ids[i]);

				songElement.order = i;
				songElement.id = ids[i];
				songElement.Set(songInfo.songName, songInfo.songAuthor, scoreHistoryManager.GetHistory(ids[i]).rank);
			}

			SnapTo(parent.GetChild(selected).GetComponent<RectTransform>());
		}

		// https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
		private void SnapTo(RectTransform target)
		{
			if (isFirst)
			{
				Canvas.ForceUpdateCanvases();
				isFirst = false;
			}

			contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
		}
	}
}
