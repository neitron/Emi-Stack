using UnityEngine;



public interface IShopItem
{


	int cost { get; }
	bool isBought { get; set; }
	int id { get; }

	void Purchase();
	void Equip();


}



public interface IHasThumbnail
{


	Sprite thumbnail { get; }


}


public interface IShopItemWithThumb : IShopItem, IHasThumbnail
{}
