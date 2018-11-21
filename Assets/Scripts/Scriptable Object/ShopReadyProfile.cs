using System;
using UnityEngine;



public abstract class ShopReadyProfile : ScriptableObject, IShopReadyProfile
{


	[SerializeField] private int _cost;
	public int cost => _cost;

	[SerializeField] private Sprite _thumbnail;
	public Sprite thumbnail => _thumbnail;



	public abstract void Select();


}
