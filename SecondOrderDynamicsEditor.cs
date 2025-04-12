using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SecondOrderDynamics))]
public class SecondOrderDynamicsEditor : Editor
{
	private const int resolution = 200;
	private const float simulationTime = 5f;

	private float xScaleMin = 1f;
	private float xScaleMax = 1.5f;
	private float yScaleMin = 0.5f;
	private float yScaleMax = 1.5f;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GUILayout.Space(10);
		GUILayout.Label("📈 Response Curve Preview", EditorStyles.boldLabel);

		xScaleMin = EditorGUILayout.FloatField("X Axis Min", xScaleMin);
		xScaleMax = EditorGUILayout.FloatField("X Axis Max", xScaleMax);
		yScaleMin = EditorGUILayout.FloatField("Y Axis Min", yScaleMin);
		yScaleMax = EditorGUILayout.FloatField("Y Axis Max", yScaleMax);

		Rect graphRect = GUILayoutUtility.GetRect(250, 100);
		EditorGUI.DrawRect(graphRect, new Color(0f, 0f, 0f, 0.1f));
		Handles.DrawSolidRectangleWithOutline(graphRect, Color.clear, Color.gray);

		SecondOrderDynamics dyn = (SecondOrderDynamics)target;
		float F = dyn.F;
		float Z = dyn.Z;
		float R = dyn.R;

		float dt = simulationTime / resolution;

		float k1 = Z / (Mathf.PI * F);
		float k2 = 1f / (Mathf.Pow(2f * Mathf.PI * F, 2f));
		float k3 = R * Z / (2f * Mathf.PI * F);

		float y = 0f;
		float yd = 0f;

		float prevX = 0f;
		float x = 0f;
		float xd = 0f;

		Vector3[] points = new Vector3[resolution];
		int pointIndex = 0;

		for (int i = 0; i < resolution; i++)
		{
			float t = Mathf.Lerp(xScaleMin, xScaleMax, i / (float)(resolution - 1));

			x = Mathf.Clamp01(t);
			xd = (x - prevX) / dt;
			prevX = x;

			float k2_stable = Mathf.Max(k2, dt * dt / 2f + dt * k1 / 2f, dt * k1);

			y += dt * yd;
			yd += dt * (x + k3 * xd - y - k1 * yd) / k2_stable;

			float normX = Mathf.Lerp(graphRect.x, graphRect.xMax, Mathf.InverseLerp(xScaleMin, xScaleMax, t));
			float normY = Mathf.Lerp(graphRect.yMax, graphRect.y, Mathf.InverseLerp(yScaleMin, yScaleMax, y));

			if (pointIndex < resolution)
				points[pointIndex++] = new Vector3(normX, normY, 0);
		}

		// y = 1 기준선
		float lineY = Mathf.Lerp(graphRect.yMax, graphRect.y, Mathf.InverseLerp(yScaleMin, yScaleMax, 1f));
		Handles.color = new Color(0.7f, 0.7f, 0.7f, 1f);
		Handles.DrawLine(new Vector2(graphRect.x, lineY), new Vector2(graphRect.xMax, lineY));

		// 축 숫자 표기
		GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 9, normal = { textColor = Color.gray } };
		for (int i = 0; i <= 3; i++)
		{
			float tx = Mathf.Lerp(graphRect.x, graphRect.xMax, i / 3f);
			float labelValue = Mathf.Lerp(xScaleMin, xScaleMax, i / 3f);
			GUI.Label(new Rect(tx - 10, graphRect.yMax + 2, 40, 15), labelValue.ToString("0.00"), labelStyle);
		}
		for (int j = 0; j <= 2; j++)
		{
			float ty = Mathf.Lerp(graphRect.yMax, graphRect.y, j / 2f);
			float labelValue = Mathf.Lerp(yScaleMin, yScaleMax, j / 2f);
			GUI.Label(new Rect(graphRect.x - 35, ty - 7, 30, 15), labelValue.ToString("0.00"), labelStyle);
		}

		Handles.color = Color.green;
		Handles.DrawAAPolyLine(2f, points);
	}
}
