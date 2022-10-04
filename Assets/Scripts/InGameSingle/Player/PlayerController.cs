using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MineBeat.InGameSingle.Player
{
	/// <summary>
	/// 플레이어의 이동과 쿨타임을 관리합니다.
	/// </summary>
	public class PlayerController : MonoBehaviour
	{
		[Header("Control")]
		[SerializeField]
		private float moveSpeed = 5f;

		private Rigidbody2D rb;

		[Header("CoolTime")]
		[SerializeField]
		private float coolTime = 0.5f;
		private float nowCoolTime = 0f;

		private bool _isCoolTime = false;
		public bool isCoolTime
		{
			get { return _isCoolTime; }
			private set { _isCoolTime = value; }
		}

		[Header("Color")]
		[SerializeField]
		private Color normalColor = new Color(1f, 1f, 1f, 1f);
		[SerializeField]
		private Color coolTimeColor = new Color(1f, 1f, 1f, 1f);

		private SpriteRenderer spriteRenderer;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			if (isCoolTime)
			{
				if (nowCoolTime >= coolTime)
				{
					spriteRenderer.color = normalColor;
					isCoolTime = false;
					nowCoolTime = 0f;
				}
				else
				{
					nowCoolTime += Time.deltaTime;
				}
			}

			Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			direction.Normalize();

			rb.velocity = moveSpeed * direction;
		}

		/// <summary>
		/// 플레이어의 체력이 깎였을 때 쿨타임을 실행합니다.
		/// </summary>
		public void StartCoolTime()
		{
			spriteRenderer.color = coolTimeColor;
			isCoolTime = true;
		}
	}
}
