using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.InGameSingle.Box;
using MineBeat.InGameSingle.Player;

/*
 * [Namespace] MineBeat.InGameSingle.Notes
 * Description
 */
namespace MineBeat.InGameSingle.Notes
{
	/*
	 * [Class] PlayNote
	 * 배치된 노트에 붙어 노트를 제어합니다.
	 */
	public class PlayNote : MonoBehaviour
	{
		[SerializeField, Tooltip("DefineNote.NoteDirection을 기준으로 입력합니다.")]
		private TileBase[] verticalWarnTiles;
		[SerializeField]
		private TileBase normalWarnTile;
		[SerializeField, Tooltip("DefineNote.NoteColor를 기준으로 입력합니다.")]
		private Sprite[] boxes;
		
		private Tilemap warningTilemap;
		Vector3Int warningTilemapPosition;
		
		private Note noteInfo;

		private bool triggered = false;
		private float timecode = 0f;

		private int positionAddX = 0;
		private int positionAddY = 0;

		private GameManager gameManager;
		private BoxManager boxManager;

		private bool isActive = true; // 해당 노트에 대한 판정이 살아있는지 (플레이어와 충돌 이후부터는 해당 노트는 판정에서 제함)

		/* GameEditorScene과 동일하게 입력합니다. */
		private float normalNoteSpeed = 20f;

		private void Start()
		{
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			boxManager = GameObject.Find("Tilemaps").GetComponent<BoxManager>();
		}

		public void Init(float startAfter, Note noteInfo, Tilemap warningTilemap)
		{
			#if UNITY_EDITOR
				gameObject.name = string.Format("{0} - {1}({2}) - ({3},{4})", noteInfo.timeCode, noteInfo.type.ToString(), noteInfo.color.ToString(), noteInfo.position.x, noteInfo.position.y);
			#endif

			gameObject.transform.position = new Vector3(noteInfo.position.x + 0.5f, noteInfo.position.y + 0.5f, 0f);
			timecode = -startAfter;
			this.noteInfo = noteInfo;
			this.warningTilemap = warningTilemap;

			switch (noteInfo.direction)
			{
				case NoteDirection.Up:
					positionAddY++;
					break;
				case NoteDirection.Down:
					positionAddY--;
					break;
				case NoteDirection.Left:
					positionAddX--;
					break;
				case NoteDirection.Right:
					positionAddX++;
					break;
			}
			warningTilemapPosition = new Vector3Int(noteInfo.position.x + positionAddX, noteInfo.position.y + positionAddY, 0);
		}

		private void Update()
		{
			if (timecode < 0f)
			{
				warningTilemap.SetTile(warningTilemapPosition, noteInfo.type == NoteType.Normal ? normalWarnTile : verticalWarnTiles[(int)noteInfo.direction]);
			}
			else
			{
				if (!triggered)
				{
					warningTilemap.SetTile(warningTilemapPosition, null);
					triggered = true;
				}

				int boxSize = boxManager.size;

				if (noteInfo.type == NoteType.Normal)
				{
					float posX = noteInfo.position.x + (-positionAddX * normalNoteSpeed * timecode) + 0.5f;
					float posY = noteInfo.position.y + (-positionAddY * normalNoteSpeed * timecode) + 0.5f;

					if (posX > boxSize + 1.5 || posX < 0.5 || posY > boxSize + 1.5 || posY < 0.5) OnBoxEnter();

					gameObject.transform.position = new Vector3(posX, posY, 0f);
				}
				else // NoteType.Vertical
				{

				}
			}
			timecode += Time.deltaTime;
		}

		/*
		 * [Method] OnBoxEnter(): void
		 * 노트가 박스와 충돌했을 때 (또는 그에 상응하는 이벤트가 발생했을 때) 이벤트를 처리합니다.
		 */
		private void OnBoxEnter()
		{
			if (noteInfo.type == NoteType.Normal)
			{
				gameManager.ChangeScore(noteInfo.color);
			}
			Destroy(gameObject);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (isActive && collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<PlayerController>().isCoolTime)
			{
				gameManager.ChangeHP(noteInfo.type, noteInfo.color);
				isActive = false;
			}
		}
	}
}
