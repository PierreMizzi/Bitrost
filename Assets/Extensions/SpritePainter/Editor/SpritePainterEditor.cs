using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpritePainter))]
public class SpritePainterEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

	}

	void OnSceneGUI()
	{
		Vector3 mousePosition = Event.current.mousePosition;
		mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
		mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
		mousePosition.y = -mousePosition.y
	}
}