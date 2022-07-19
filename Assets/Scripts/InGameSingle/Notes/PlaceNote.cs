using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;

using MineBeat.InGameSingle.Song;

/*
 * [Namespace] MineBeat.InGameSingle.Notes
 * Description
 */
namespace MineBeat.InGameSingle.Notes
{
	/*
	 * [Class] PlaceNote
	 * 노트의 배치를 관리합니다.
	 */
	public class PlaceNote : MonoBehaviour
	{
		[SerializeField]
		private Tilemap warningTilemap;

		[SerializeField]
		private Transform parent;
		[SerializeField]
		private GameObject prefab;

		private SongPlayManager songPlayManager;

		private ulong id;
		private List<Note> notes;

		private float previousTimecode = 0f;

		private void Awake()
		{
			songPlayManager = GameObject.Find("SongManager").GetComponent<SongPlayManager>();
			id = GameObject.Find("SelectedSongInfo").GetComponent<SelectedSongInfo>().id;
			notes = PackageManager.Instance.GetSongInfo(id).notes;
		}

		private void Update()
		{
			// 1f초 후의 노트까지 미리 긁어오기
			List<Note> targets = notes.FindAll(target => previousTimecode < target.timeCode && target.timeCode <= songPlayManager.timecode + 1f);
			previousTimecode = songPlayManager.timecode + 1f;

			foreach (Note note in targets)
			{
				if (note.type == NoteType.Normal || note.type == NoteType.Vertical)
				{
					Instantiate(prefab, Vector3.zero, Quaternion.identity, parent).GetComponent<PlayNote>().Init(note.timeCode - songPlayManager.timecode, note, warningTilemap);
				}
			}
		}
	}
}
