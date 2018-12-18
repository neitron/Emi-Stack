using System;
using UnityEngine;



[CreateAssetMenu()]
public class Shop : ScriptableObject
{


	public delegate IShopItem GoodActivator(int id, IShopReadyProfile itemProfiles);


	public ShopItemSetProfile goods;
	public IntProfile coins;


	public Action<int> onSelect;
	public Action<int> onPurchase;



	public void Init()
	{
		goods.tryToSelect += TryToSelect;
	}


	public void GenerateGoods(ShopReadyProfileSet shopReadySet, GoodActivator activator)
	{
		foreach (IShopReadyProfile goodProfile in shopReadySet.items)
		{
			// TODO: Think about another way to generate ID, try to delegate this deal to ShopUser
			int id = goods.Count;
			IShopItem shopItem = activator(id, goodProfile);

			goods.Add(shopItem);
		}
		goods.onChanged?.Invoke();
	}


	public void TryToSelect(int id)
	{
		if (goods[id].isBought)
		{
			Select(id);
			return;
		}

		if (goods[id].cost <= coins.value)
		{
			Purchase(id);
		}
	}


	private void Select(int id)
	{
		goods[id].Select();
		onSelect?.Invoke(id);
	}


	private void Purchase(int id)
	{
		coins.value -= goods[id].cost;
		goods.onBought?.Invoke(id);
		
		goods[id].Purchase();
		onPurchase?.Invoke(id);
	}


}

