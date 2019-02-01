using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class CylinderStyles : EditorWindow
{


	private string _styleName;
	private Material _baseMaterial;
	private Texture2D _emissionTexture;
	private int _cost;
	private ShopReadyProfileSet _cylinders;



	[MenuItem("Emi Stack/Cylinder Styles #c")]
	private static void Init()
	{
		CylinderStyles window = CreateInstance<CylinderStyles>();
		window.position = new Rect(Screen.width / 2.0f, Screen.height / 2.0f, 250, 150);
		window.Show();
	}


	[MenuItem("Emi Stack/Close All")]
	private static void CloseAll()
	{
		CylinderStyles[] windows = FindObjectsOfType<CylinderStyles>();
		foreach (CylinderStyles window in windows)
		{
			window.Close();
		}
	}


	private void OnEnable()
	{
		string time = DateTime.Now.ToString("dd-MM-yy");
		_styleName = $"New Cylinder {time}";
	}


	private void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("New Cylinder Style");
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		_styleName = EditorGUILayout.TextField("Name: ",  _styleName);
		_cost = EditorGUILayout.IntField("Cost: ", _cost);
		_baseMaterial = (Material)EditorGUILayout.ObjectField("Base material: ", _baseMaterial, typeof(Material), false);
		_emissionTexture = (Texture2D)EditorGUILayout.ObjectField("Emission texture: ", _emissionTexture, typeof(Texture2D), false);
		_cylinders = (ShopReadyProfileSet)EditorGUILayout.ObjectField("Shop Ready Items: ", _cylinders, typeof(ShopReadyProfileSet), false);

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		if (GUILayout.Button("Create") && _baseMaterial != null)
		{
			string path = $"Assets/Materials/Cylinder skins/{_styleName}";

			Material material = new Material(_baseMaterial);
			material.SetTexture("_EmissionMap", _emissionTexture);
			AssetDatabase.CreateAsset(material, $"{path}.mat");

			path = $"Assets/Profiles/Cylinders/{_styleName}";
			CylinderProfile cp = CreateInstance<CylinderProfile>();
			SerializedObject so = new SerializedObject(cp);
			so.FindProperty("_cost").intValue = _cost;
			so.FindProperty("_skin").objectReferenceValue = material;
			so.ApplyModifiedProperties();
			AssetDatabase.CreateAsset(cp, $"{path}.asset");

			if (_cylinders != null)
			{
				_cylinders.Add(cp);
			}

			Debug.Log(path);
		}

		if (_cylinders == null)
		{
			return;
		}
		
		for (int i = _cylinders.Count - 1; i >= 0; i--)
		{
			string assetPath = AssetDatabase.GetAssetPath(_cylinders[i]);
			EditorGUILayout.BeginHorizontal();
			ShopReadyProfile srp = (ShopReadyProfile)EditorGUILayout.ObjectField(AssetDatabase.LoadAssetAtPath<ShopReadyProfile>(assetPath), typeof(ShopReadyProfile), false);

			if (GUILayout.Button("X", GUILayout.MaxWidth(30.0f)))
			{
				srp?.OnDestroy();
				_cylinders?.Remove(srp);

				AssetDatabase.DeleteAsset(assetPath);
			}

			EditorGUILayout.EndHorizontal();
		}
	}


}
