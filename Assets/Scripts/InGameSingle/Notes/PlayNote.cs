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

		private SpriteRenderer spriteRenderer;
		private Tilemap warningTilemap;
		Vector3Int warningTilemapPosition;
		
		private Note noteInfo;

		private GameObject prefab;

		private bool triggered = false;
		private float timecode = 0f;

		private int previousBoxSize = 0;

		[SerializeField]
		private Color activeColor = new Color(1f, 1f, 1f, 1f);
		[SerializeField]
		private Color inactiveColor = new Color(1f, 1f, 1f, 1f);

		private int positionAddX = 0;
		private int positionAddY = 0;

		private GameManager gameManager;
		private BoxManager boxManager;

		private bool _isActive = true; // 해당 노트에 대한 판정이 살아있는지 (플레이어와 충돌 이후부터는 해당 노트는 판정에서 제함)
		public bool isActive
		{
			get { return _isActive; }
			private set { _isActive = value; }
		}

		/* 아래 변수들은 모두 GameEditorScene과 동일하게 입력합니다. */
		private float normalNoteSpeed = 20f;
		private float verticalTime = 0.2f;

		private void Awake()
		{
			if (transform.parent.gameObject.TryGetComponent(typeof(PlayNote), out Component component)) Destroy(this);

			spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
			boxManager = GameObject.Find("Tilemaps").GetComponent<BoxManager>();
		}

		public void Init(float startAfter, Note noteInfo, Tilemap warningTilemap, GameObject prefab)
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
					positionAddY--;
					break;
				case NoteDirection.Down:
					positionAddY++;
					break;
				case NoteDirection.Left:
					positionAddX++;
					break;
				case NoteDirection.Right:
					positionAddX--;
					break;
			}
			warningTilemapPosition = new Vector3Int(noteInfo.position.x + -positionAddX, noteInfo.position.y + -positionAddY, 0);

			this.prefab = prefab;
			spriteRenderer.sprite = boxes[(int)noteInfo.color];
			spriteRenderer.color = activeColor;
		}

		private void Update()
		{
			if (timecode < 0f)
			{
				warningTilemap.SetTile(warningTilemapPosition, noteInfo.type == NoteType.Normal ? normalWarnTile : verticalWarnTiles[(int)noteInfo.direction]);
			}
			else
			{
				if (!triggered && timecode >= 0.05f)
				{
					warningTilemap.SetTile(warningTilemapPosition, null);
					triggered = true;
				}

				int boxSize = boxManager.size;

				if (noteInfo.type == NoteType.Normal)
				{
					float posX = noteInfo.position.x + (positionAddX * normalNoteSpeed * timecode) + 0.5f;
					float posY = noteInfo.position.y + (positionAddY * normalNoteSpeed * timecode) + 0.5f;

					if (posX > boxSize + 1.5 || posX < 0.5 || posY > boxSize + 1.5 || posY < 0.5) OnBoxEnter();

					gameObject.transform.position = new Vector3(posX, posY, 0f);
				}
				else // NoteType.Vertical
				{
					if (previousBoxSize != boxSize)
					{
						for (int i = 0; i < transform.childCount; i++)
						{
							Destroy(transform.GetChild(i).gameObject);
						}

						float posX = noteInfo.position.x + 0.5f;
						float posY = noteInfo.position.y + 0.5f;

						for (int i = 0; i < boxSize; i++)
						{
							if (noteInfo.direction == NoteDirection.Up || noteInfo.direction == NoteDirection.Down)
							{
								Instantiate(prefab, new Vector3(posX, posY + ((i + 1) * positionAddY), 0f), Quaternion.identity, transform);
							}
							else
							{
								Instantiate(prefab, new Vector3(posX + ((i + 1) * positionAddX), posY, 0f), Quaternion.identity, transform);
							}
						}

						previousBoxSize = boxSize;
					}

					if (timecode >= verticalTime) OnBoxEnter();
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

			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}

			Destroy(gameObject);
		}

		/*
		 * [Method] OnTriggered(Collider2D collision): void
		 * 다른 GameObject와 충돌했을 때 판정을 처리합니다.
		 * 
		 * <Collider2D collision>
		 * 충돌한 GameObject를 입력합니다.
		 */
		public void OnTriggered(Collider2D collision)
		{

			if (isActive && collision.gameObject.tag == "Player" && !collision.gameObject.GetComponent<PlayerController>().isCoolTime)
			{
				gameManager.ChangeHP(noteInfo.type, noteInfo.color);
				spriteRenderer.color = inactiveColor;
				isActive = false;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnTriggered(collision);
		}
	}
}
