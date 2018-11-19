public class CylinderShopItem : IShopItem<CylinderProfile>
{


	private CylinderProfile _profile;
	private int _id;


	public int id => _id;
	public CylinderProfile profile => _profile;
	public bool isBought => _profile.isBought;



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

