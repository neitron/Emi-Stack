using System;
using UnityEngine;



[CreateAssetMenu(fileName = "Cylinder")]
public class CylinderProfile : ShopReadyProfile
{


	public static event Action<CylinderProfile> OnCylinderSelected;


	[SerializeField] private Color _tintColor;
	public Color tintColor => _tintColor;

	[SerializeField] private Texture2D _mainMap;
	public Texture2D mainMap => _mainMap;

	[SerializeField] private Texture2D _emissionMap;
	public Texture2D emissionMap => _emissionMap;

	[SerializeField] private Texture2D _normalMap;
	public Texture2D normalMap => _normalMap;


	public override void Select()
	{
		OnCylinderSelected?.Invoke(this);
	}


}