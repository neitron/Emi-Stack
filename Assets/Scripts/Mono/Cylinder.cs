using UnityEngine;
using DG.Tweening;
using System;



public class Cylinder : MonoBehaviour
{
	


	private MaterialPropertyBlock _materialPropertyBlock;
	private MeshRenderer _renderer;
	private int _emiColorKey;
	


	private void Awake()
	{
		_materialPropertyBlock = new MaterialPropertyBlock();
		_renderer = GetComponentInChildren<MeshRenderer>();
		_emiColorKey = Shader.PropertyToID("_EmissionColor");
	}


	public void ChangeEmissionColor(Color to, float duration, TweenCallback callback = null)
	{
		_renderer.GetPropertyBlock(_materialPropertyBlock);
		Color from = _materialPropertyBlock.GetColor(_emiColorKey);

		DOTween.To(
			() => from, 
			currentColor => 
			{
				_renderer.GetPropertyBlock(_materialPropertyBlock);
				_materialPropertyBlock.SetColor(_emiColorKey, currentColor);
				_renderer.SetPropertyBlock(_materialPropertyBlock);
			}, to, duration)
			.OnComplete(callback);
	}


	public void Purchase()
	{
		throw new NotImplementedException();
	}


	public void Equip()
	{
		throw new NotImplementedException();
	}


}
