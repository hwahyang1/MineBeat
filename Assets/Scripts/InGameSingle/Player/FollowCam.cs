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
	 * [Class] FollowCam
	 * 카메라가 플레이어를 따라다니도록 설정합니다.
	 */
	public class FollowCam : MonoBehaviour
	{
        private Transform target;

		[SerializeField]
        private float lerpSpeed = 2.5f;

        private Vector3 offset;
        private Vector3 targetPos;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            offset = transform.position - target.position;
        }

        private void Update()
        {
            targetPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        }
    }
}
