using UnityEngine;
using DG.Tweening;




public class Cylinder : MonoBehaviour, IEquipable<CylinderProfile>
{


	private MaterialPropertyBlock _materialPropertyBlock;
	private MeshRenderer _renderer;
	private int _emiColorKey;
	private int _tintColorKey;
	private int _mainTextureKey;
	private int _bumpTextureKey;
	private int _emiTextureKey;



	private void Awake()
	{
		_materialPropertyBlock = new MaterialPropertyBlock();
		_renderer = GetComponentInChildren<MeshRenderer>();

		_emiColorKey = Shader.PropertyToID("_EmissionColor");
		_tintColorKey = Shader.PropertyToID("_Color");
		_mainTextureKey = Shader.PropertyToID("_MainTex");
		_bumpTextureKey = Shader.PropertyToID("_BumpMap");
		_emiTextureKey = Shader.PropertyToID("_EmissionMap");

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
		_renderer.sharedMaterial.SetColor(_tintColorKey, toEquip.tintColor);
		_renderer.sharedMaterial.SetTexture(_mainTextureKey, toEquip.mainMap);
		_renderer.sharedMaterial.SetTexture(_bumpTextureKey, toEquip.normalMap);
		_renderer.sharedMaterial.SetTexture(_emiTextureKey, toEquip.emissionMap);
	}
	

}
