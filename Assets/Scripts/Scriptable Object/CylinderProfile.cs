using System;
using UnityEditor;
using UnityEngine;
#pragma warning disable 649



[CreateAssetMenu(fileName = "Cylinder")]
public class CylinderProfile : ShopReadyProfile
{


	public static event Action<CylinderProfile> OnCylinderSelected;

	
	[SerializeField] private Material _skin;
	public Material skin => _skin;



	public override void Select()
	{
		OnCylinderSelected?.Invoke(this);
	}


	public override void OnDestroy()
	{
		if (_skin == null)
		{
			return;
		}

#if UNITY_EDITOR
		string materialPath = AssetDatabase.GetAssetPath(_skin);
		AssetDatabase.DeleteAsset(materialPath);
#endif
	}


}