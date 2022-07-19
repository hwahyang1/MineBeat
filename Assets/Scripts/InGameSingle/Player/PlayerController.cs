using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*
 * [Namespace] MineBeat.InGameSingle.Player
 * Description
 */
namespace MineBeat.InGameSingle.Player
{
	/*
	 * [Class] PlayerController
	 * 플레이어의 이동과 쿨타임을 관리합니다.
	 */
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float moveSpeed = 5f;

		private Rigidbody2D rb;

		[SerializeField]
		private float coolTime = 0.5f;
		private float nowCoolTime = 0f;

		private bool _isCoolTime = false;
		public bool isCoolTime
		{
			get { return _isCoolTime; }
			private set { _isCoolTime = value; }
		}

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			if (isCoolTime)
			{
				if (nowCoolTime >= coolTime)
				{
					isCoolTime = false;
					nowCoolTime = 0f;
				}
				nowCoolTime += Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				rb.AddForce(new Vector2(0f, moveSpeed * Time.deltaTime));
				//transform.Translate(new Vector3(0f, moveSpeed * Time.deltaTime, 0f));
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				rb.AddForce(new Vector2(0f, -moveSpeed * Time.deltaTime));
				//transform.Translate(new Vector3(0f, -moveSpeed * Time.deltaTime, 0f));
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				rb.AddForce(new Vector2(-moveSpeed * Time.deltaTime, 0f));
				//transform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f));
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				rb.AddForce(new Vector2(moveSpeed * Time.deltaTime, 0f));
				//transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0f, 0f));
			}
		}

		/*
		 * [Method] StartCoolTime(): void
		 * 플레이어의 체력이 깎였을 때 쿨타임을 실행합니다.
		 */
		public void StartCoolTime()
		{
			isCoolTime = true;
		}
	}
}
