using UnityEngine;
using System.Collections;

public class ObjectMover : MonoBehaviour
{
	[Header("Inspector Speed Settings")]
	[Range(1, 20)] public int speed1 = 5;   // 1단계 이동 속도 (x+ 방향)
	[Range(1, 20)] public int speed2 = 10;  // 3단계 이동 속도 (x- 방향)

	private void Start()
	{
		StartCoroutine(MoveRoutine());
	}

	IEnumerator MoveRoutine()
	{
		while (true)
		{
			// 1단계: x=10까지 이동 (속도 speed1)
			yield return MoveToX(3f, speed1);
			yield return new WaitForSeconds(2f);

			// 2단계: x=0으로 순간이동
			transform.position = new Vector3(0f, transform.position.y, transform.position.z);
			yield return new WaitForSeconds(2f);

			// 3단계: x=-10까지 이동 (속도 speed2)
			yield return MoveToX(-3f, speed2);
			yield return new WaitForSeconds(2f);

			// 4단계: x=0으로 순간이동
			transform.position = new Vector3(0f, transform.position.y, transform.position.z);
			yield return new WaitForSeconds(2f);
		}
	}

	IEnumerator MoveToX(float targetX, float speed)
	{
		Vector3 start = transform.position;
		Vector3 target = new Vector3(targetX, start.y, start.z);

		while (Mathf.Abs(transform.position.x - targetX) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
			yield return null;
		}
		transform.position = target;
	}
}
