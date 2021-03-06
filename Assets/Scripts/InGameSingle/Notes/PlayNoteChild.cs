using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.InGameSingle.Notes
 * Description
 */
namespace MineBeat.InGameSingle.Notes
{
	/*
	 * [Class] PlayNoteChild
	 * PlayNote 하위에 붙어 판정을 처리합니다.
	 */
	public class PlayNoteChild : MonoBehaviour
	{
		private PlayNote parent;
		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private Color activeColor = new Color(1f, 1f, 1f, 1f);
		[SerializeField]
		private Color inactiveColor = new Color(1f, 1f, 1f, 1f);

		private void Awake()
		{
			bool result = transform.parent.gameObject.TryGetComponent(typeof(PlayNote), out Component component);
			if (result)
			{
				parent = (PlayNote)component;
				spriteRenderer = GetComponent<SpriteRenderer>();
				spriteRenderer.sprite = parent.GetComponent<SpriteRenderer>().sprite;
			}
			else
			{
				Destroy(this);
			}
		}

		private void Update()
		{
			spriteRenderer.color = parent.isActive ? activeColor : inactiveColor;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			parent.OnTriggered(collision);
		}
	}
}
