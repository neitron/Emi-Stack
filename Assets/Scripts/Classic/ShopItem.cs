using UnityEngine;



public class ShopItem : IShopItem, IShopItemView
{


	private readonly IShopReadyProfile _profile;
	private readonly int _id;

	
	public int id => _id;
	public bool isBought { get; set; }

	public int cost => _profile.cost;
	public Sprite thumbnail => _profile.thumbnail;



	public ShopItem(IShopReadyProfile profile, int id)
	{
		_profile = profile;
		_id = id;
	}


	public void Purchase()
	{
		isBought = true;
	}


	public void Select()
	{
		_profile.Select();
	}


}

