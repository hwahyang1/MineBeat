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
	 * 플레이어의 이동을 관리합니다.
	 */
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private float moveSpeed = 5f;

		private Rigidbody2D rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
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
	}
}
