using System;
using UnityEngine;
using Object = UnityEngine.Object;



public abstract class ShopReadyProfile : ScriptableObject, IShopReadyProfile
{


	[SerializeField] private int _cost;
	public int cost => _cost;

	[SerializeField] private Sprite _thumbnail;
	public Sprite thumbnail => _thumbnail;



	public abstract void Select();


	public virtual void OnDestroy()
	{
		Debug.Log($"{this.name} is destroyed");
	}


}
