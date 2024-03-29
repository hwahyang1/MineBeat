using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using MineBeat.InGameSingle.Box;
using MineBeat.InGameSingle.Player;

namespace MineBeat.InGameSingle.Notes
{
	/// <summary>
	/// 배치된 노트에 붙어 노트를 제어합니다.
	/// </summary>
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
		private Vector3Int warningTilemapPosition;
		
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
		private PlaceNote placeNote;

		public bool IsActive { get; private set; } = true;

		[Header("아래 변수들은 모두 GameEditorScene과 동일하게 입력합니다.")]
		[SerializeField]
		private float normalNoteSpeed;
		[SerializeField]
		private float verticalTime;

		private void Awake()
		{
			if (transform.parent.gameObject.TryGetComponent(typeof(PlayNote), out Component component)) Destroy(this); // Vertical Note일 경우

			List<GameObject> managers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Managers"));

			gameManager = managers.Find(target => target.name == "GameManager").GetComponent<GameManager>();
			placeNote = managers.Find(target => target.name == "NoteManager").GetComponent<PlaceNote>();
			spriteRenderer = GetComponent<SpriteRenderer>();
			boxManager = GameObject.Find("Tilemaps").GetComponent<BoxManager>();
		}

		public void Init(float startAfter, Note noteInfo, Tilemap warningTilemap, GameObject prefab)
		{
			#if UNITY_EDITOR
				gameObject.name = string.Format("{0} - {1}({2}) - ({3},{4})", noteInfo.timeCode, noteInfo.type.ToString(), noteInfo.color.ToString(), noteInfo.position.x, noteInfo.position.y);
			#endif

			previousBoxSize = 0;
			positionAddX = 0;
			positionAddY = 0;

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

			triggered = false;
			IsActive = true;
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

				int boxSize = boxManager.Size;

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

		/// <summary>
		/// 노트가 박스와 충돌했을 때 (또는 그에 상응하는 이벤트가 발생했을 때) 이벤트를 처리합니다.
		/// </summary>
		private void OnBoxEnter()
		{
			// 점수 처리 (Normal Note의 경우)
			if (IsActive)
			{
				gameManager.ChangeScore(noteInfo.color);
			}

			// Vertical의 경우, 자식 GameObject 제거
			for (int i = 0; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}

			// 반환
			//Destroy(gameObject);
			transform.SetParent(placeNote.DisabledParent);
			gameObject.SetActive(false);
		}

		/// <summary>
		/// 다른 GameObject와 충돌했을 때 판정을 처리합니다.
		/// </summary>
		/// <param name="collision">충돌한 GameObject를 입력합니다.</param>
		public void OnTriggered(Collider2D collision)
		{
			// 맞으면 판정 처리를 GameManager에게 넘김
			if (IsActive && collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<PlayerController>().IsCoolTime)
			{
				gameManager.ChangeHP(noteInfo.type, noteInfo.color);
				spriteRenderer.color = inactiveColor;
				IsActive = false;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnTriggered(collision);
		}
	}
}
