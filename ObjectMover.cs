using UnityEngine;
using System.Collections;

public class ObjectMover : MonoBehaviour
{
	[Header("Inspector Speed Settings")]
	[Range(1, 20)] public int speed1 = 5;   // 1�ܰ� �̵� �ӵ� (x+ ����)
	[Range(1, 20)] public int speed2 = 10;  // 3�ܰ� �̵� �ӵ� (x- ����)

	private void Start()
	{
		StartCoroutine(MoveRoutine());
	}

	IEnumerator MoveRoutine()
	{
		while (true)
		{
			// 1�ܰ�: x=10���� �̵� (�ӵ� speed1)
			yield return MoveToX(3f, speed1);
			yield return new WaitForSeconds(2f);

			// 2�ܰ�: x=0���� �����̵�
			transform.position = new Vector3(0f, transform.position.y, transform.position.z);
			yield return new WaitForSeconds(2f);

			// 3�ܰ�: x=-10���� �̵� (�ӵ� speed2)
			yield return MoveToX(-3f, speed2);
			yield return new WaitForSeconds(2f);

			// 4�ܰ�: x=0���� �����̵�
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
