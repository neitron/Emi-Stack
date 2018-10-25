using UnityEngine;
using DG.Tweening;
using System;



public class Cylinder : MonoBehaviour
{
	

	MaterialPropertyBlock _materialPropertyBlock;
	MeshRenderer _renderer;
	int _emiColorKey;


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

		
		
		//GetComponentInChildren<MeshRenderer>().material
		//	.DOColor(color, "_EmissionColor", duration)
		//	.OnComplete(callback);
	}


}
