using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(GameEventProfiler))]
public class GameEventProfilerEditor : Editor
{

	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector();

		GUI.enabled = Application.isPlaying;

		if (GUILayout.Button("Race"))
		{
			GameEventProfiler e = target as GameEventProfiler;
			e.Raice();
		}
	}
}
