using UnityEngine;
using DG.Tweening;
using System;



public class Cylinder : MonoBehaviour
{ 
	

	public void ChangeEmissionColor(Color color, float duration, TweenCallback callback = null)
	{
		GetComponentInChildren<MeshRenderer>().material
			.DOColor(color, "_EmissionColor", duration)
			.OnComplete(callback);
	}


}
