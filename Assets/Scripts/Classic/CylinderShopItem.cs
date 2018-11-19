using UnityEngine;

public class CylinderShopItem : IShopItemWithThumb
{


	private CylinderProfile _profile;
	private int _id;


	public int id => _id;
	public CylinderProfile profile => _profile;
	public int cost => _profile.cost;
	public Sprite thumbnail => _profile.thumbnail;

	public bool isBought
	{
		get
		{
			return _profile.isBought;
		}

		set
		{
			_profile.isBought = value;
		}
	}




	public CylinderShopItem(CylinderProfile profile, int id)
	{
		_profile = profile;
		_id = id;
	}


	public void Equip()
	{
		throw new System.NotImplementedException();
	}


	public void Purchase()
	{
		profile.isBought = true;
	}


}

