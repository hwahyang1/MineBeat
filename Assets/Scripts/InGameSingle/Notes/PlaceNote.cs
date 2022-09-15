using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;

using MineBeat.InGameSingle.Box;
using MineBeat.InGameSingle.Song;

namespace MineBeat.InGameSingle.Notes
{
	/// <summary>
	/// 노트의 배치를 관리합니다.
	/// </summary>
	public class PlaceNote : MonoBehaviour
	{
		[SerializeField]
		private Tilemap warningTilemap;

		[SerializeField]
		private Transform parent;
		[SerializeField]
		private GameObject prefab;

		private BoxManager boxManager;
		private SongPlayManager songPlayManager;

		private ulong id;
		private List<Note> notes;

		private float previousTimecode = 0f;

		/* GameEditorScene과 동일하게 입력합니다. */
		private float aroundTime = 0.35f;

		private void Awake()
		{
			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			boxManager = managers.Find(target => target.name == "Tilemaps").GetComponent<BoxManager>();
			songPlayManager = managers.Find(target => target.name == "SongManager").GetComponent<SongPlayManager>();
			id = managers.Find(target => target.name == "SelectedSongInfo").GetComponent<SelectedSongInfo>().id;

			notes = PackageManager.Instance.GetSongInfo(id).notes;
		}

		private void Update()
		{
			// aroundTime 후의 노트까지 미리 긁어오기
			List<Note> targets = notes.FindAll(target => previousTimecode < target.timeCode && target.timeCode <= songPlayManager.timecode + aroundTime);
			previousTimecode = songPlayManager.timecode + aroundTime;

			foreach (Note note in targets)
			{
				System.Action runAction = null;

				switch (note.type)
				{
					case NoteType.Normal:
					case NoteType.Vertical:
						Instantiate(prefab, Vector3.zero, Quaternion.identity, parent).GetComponent<PlayNote>().Init(note.timeCode - songPlayManager.timecode, note, warningTilemap, prefab);
						break;
					case NoteType.SizeChange:
						runAction = () => { boxManager.Draw(note.position.y, boxManager.color); };
						break;
					case NoteType.BoxColorChange:
						runAction = () => { boxManager.Draw(boxManager.size, note.color); };
						break;
					case NoteType.BlankS:
						runAction = () => { boxManager.ChangeVisibility(false); };
						break;
					case NoteType.BlankE:
						runAction = () => { boxManager.ChangeVisibility(true); };
						break;
					case NoteType.ImpactLine:
						break;
				}

				if (runAction != null) StartCoroutine(DelayedRun(runAction));
			}
		}

		public IEnumerator DelayedRun(System.Action action)
		{
			yield return new WaitForSeconds(aroundTime);
			action();
		}
	}
}
