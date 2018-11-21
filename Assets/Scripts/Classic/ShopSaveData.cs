using System.Collections.Generic;

[System.Serializable]
public class ShopSaveData : SaveData
{
	

	public List<ShopItemSaveData> shopBoughtItems;



	public ShopSaveData() : base ()
	{
		Init();
	}


	public ShopSaveData(string keyName) : base(keyName)
	{
		Init();
	}


	private void Init()
	{
		shopBoughtItems = new List<ShopItemSaveData>();
	}


	public void AddDefault()
	{
		ShopItemSaveData temp = new ShopItemSaveData()
		{
			isBought = false
		};
		shopBoughtItems.Add(temp);
	}


	public void Reset()
	{
		foreach (ShopItemSaveData item in shopBoughtItems)
		{
			item.isBought = false;
		}
		shopBoughtItems[0].isBought = true;
	}


}