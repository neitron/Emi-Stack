using DG.Tweening;
using UnityEngine;




public class Cylinder : MonoBehaviour, IEquipable<CylinderProfile>
{


	private MaterialPropertyBlock _materialPropertyBlock;
	private MeshRenderer _renderer;
	private int _emiColorKey;



	private void Awake()
	{
		_materialPropertyBlock = new MaterialPropertyBlock();
		_renderer = GetComponentInChildren<MeshRenderer>();

		_emiColorKey = Shader.PropertyToID("_EmissionColor");

		CylinderProfile.OnCylinderSelected += Equip;
	}


	public void ChangeEmissionColor(Color to, float duration, TweenCallback callback = null)
	{
		_renderer.GetPropertyBlock(_materialPropertyBlock);
		Color from = _materialPropertyBlock.GetColor(_emiColorKey);

		DOTween.To(() => from, SetMaterialPropertyBlock, to, duration)
			.OnComplete(callback);
	}


	private void SetMaterialPropertyBlock(Color currentColor)
	{
		_renderer.GetPropertyBlock(_materialPropertyBlock);
		_materialPropertyBlock.SetColor(_emiColorKey, currentColor);
		_renderer.SetPropertyBlock(_materialPropertyBlock);
	}


	public void Equip(CylinderProfile toEquip)
	{
		_renderer.sharedMaterial = toEquip.skin;
	}
	

}
