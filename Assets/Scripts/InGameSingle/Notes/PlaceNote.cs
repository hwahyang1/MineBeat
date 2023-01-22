using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.SongSelectSingle.Extern;

using MineBeat.Preload.Song;

using MineBeat.InGameSingle.Box;
using MineBeat.InGameSingle.Song;
using MineBeat.InGameSingle.HP;
using MineBeat.InGameSingle.UI;
using UnityEngine.Serialization;

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
		private Transform notesParent;
		[SerializeField]
		private Transform disabledParent;
		public Transform DisabledParent { get { return disabledParent; } }

		[SerializeField]
		private GameObject prefab;

		private GameObject player;

		private ImpactLine impactLine;
		private HPManager hpManager;
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

			hpManager = managers.Find(target => target.name == "HPManager").GetComponent<HPManager>();
			impactLine = managers.Find(target => target.name == "UIManagers").GetComponent<ImpactLine>();
			boxManager = managers.Find(target => target.name == "Tilemaps").GetComponent<BoxManager>();
			songPlayManager = managers.Find(target => target.name == "SongManager").GetComponent<SongPlayManager>();
			id = managers.Find(target => target.name == "SelectedSongInfo").GetComponent<SelectedSongInfo>().ID;

			notes = PackageManager.Instance.GetSongInfo(id).notes;

			player = GameObject.FindGameObjectWithTag("Player");
		}

		private void Update()
		{
			// aroundTime 후의 노트까지 미리 긁어오기
			List<Note> targets = notes.FindAll(target => previousTimecode < target.timeCode && target.timeCode <= songPlayManager.Timecode + aroundTime);
			previousTimecode = songPlayManager.Timecode + aroundTime;

			foreach (Note note in targets)
			{
				System.Action runAction = null;

				switch (note.type)
				{
					case NoteType.Normal:
					case NoteType.Vertical:
						if (DisabledParent.childCount > 0) // 반환된 GameObject가 있으면
						{
							GameObject obj = DisabledParent.GetChild(0).gameObject;
							obj.transform.SetParent(notesParent);
							obj.GetComponent<PlayNote>().Init(note.timeCode - songPlayManager.Timecode, note, warningTilemap, prefab);
							obj.SetActive(true);
						}
						else
						{
							Instantiate(prefab, Vector3.zero, Quaternion.identity, notesParent).GetComponent<PlayNote>().Init(note.timeCode - songPlayManager.Timecode, note, warningTilemap, prefab);
						}
						break;
					case NoteType.SizeChange:
						runAction = () => { boxManager.Draw(note.position.y, boxManager.Color); };
						break;
					case NoteType.BoxColorChange:
						runAction = () => { boxManager.Draw(boxManager.Size, note.color); };
						break;
					case NoteType.BlankS:
						runAction = () =>
						{
							boxManager.ChangeVisibility(false, false);
							player.SetActive(false);
						};
						break;
					case NoteType.BlankE:
						runAction = () =>
						{
							player.SetActive(true);
							boxManager.ChangeVisibility(true, true);
						};
						break;
					case NoteType.ImpactLine:
						runAction = () =>
						{
							hpManager.FixHp();
							impactLine.Run();
						};
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
