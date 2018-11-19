using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(GameCore))]
class GameCoreHelper : Editor
{
	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector();

		if (GUILayout.Button("Clean Player Prefs"))
		{
			GamePrefs _gamePrefs = new GamePrefs();
			_gamePrefs.Reset();
			
			Caching.ClearCache();
		}
	}
}
