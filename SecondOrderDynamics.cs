using UnityEngine;

[ExecuteAlways]
public class SecondOrderDynamics : MonoBehaviour
{
	[Header("Second Order Dynamics Parameters")]
	[Range(0.1f, 10f)] public float F = 1.0f;  // Frequency
	[Range(-2f, 2f)] public float Z = 0.5f;     // Damping ratio
	[Range(-5f, 5f)] public float R = 1.0f;     // Response

	[Header("Target Transform")]
	public Transform target;

	private Vector3 xp;  // previous input
	private Vector3 y;   // output position
	private Vector3 yd;  // output velocity

	void Start()
	{
		if (target != null)
		{
			xp = target.position;
			y = target.position;
			yd = Vector3.zero;
		}
	}

	void Update()
	{
		if (target == null) return;

		float dt = Time.deltaTime;
		if (dt < Mathf.Epsilon) return;

		Vector3 x = target.position;
		Vector3 xd = (x - xp) / dt;
		xp = x;

		float k1 = Z / (Mathf.PI * F);
		float k2 = 1f / (Mathf.Pow(2f * Mathf.PI * F, 2f));
		float k3 = R * Z / (2f * Mathf.PI * F);

		float k2_stable = Mathf.Max(k2, dt * dt / 2f + dt * k1 / 2f, dt * k1);

		y += dt * yd;
		yd += dt * (x + k3 * xd - y - k1 * yd) / k2_stable;

		transform.position = y;
	}
}
