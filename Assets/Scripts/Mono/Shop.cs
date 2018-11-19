using System;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{


	[SerializeField] private ShopItemsProfile _goods;
	[SerializeField] private List<CylinderProfile> _goodsProfiles;
	[SerializeField] private IntProfile _coins;


	private GamePrefs _gamePrefs;
	private ShopSaveData _shopSaveData;



	private void Awake()
	{
		_gamePrefs = new GamePrefs();
		_goods.tryToSelect += PurchaseOrEquip;
	}


	private void Start()
	{
		_goodsProfiles.Sort((cpl, cpr) => { return cpl.cost.CompareTo(cpr.cost); });

		LoadShopData();
		GenerateGoods();
	}


	private void LoadShopData()
	{
		GameSaver gs = new GameSaver();
		
		if (!gs.Load(out _shopSaveData))
		{
			SetDefaultData();
		}
		else if (_shopSaveData.shopBoughtItems.Count != _goodsProfiles.Count)
		{
			SyncData();
		}
	}


	private void SaveShopData()
	{
		GameSaver gameSaver = new GameSaver();
		gameSaver.Save(_shopSaveData);

		gameSaver.SaveAsJson(_shopSaveData, true);
		gameSaver.SaveAsJson(_goods, true);
	}


	private void SyncData()
	{
		for (int i = _shopSaveData.shopBoughtItems.Count; i < _goodsProfiles.Count; i++)
		{
			_shopSaveData.AddDefault();
		}
	}


	private void SetDefaultData()
	{
		for (int id = 0; id < _goodsProfiles.Count; id++)
		{
			_shopSaveData.AddDefault();
		}
		_shopSaveData.shopBoughtItems[0].isBought = true;
	}
	

	private void GenerateGoods()
	{
		int id = 0;
		foreach (CylinderProfile goodProfile in _goodsProfiles)
		{
			IShopItem shopItem = new CylinderShopItem(goodProfile, id);
			shopItem.isBought = _shopSaveData.shopBoughtItems[id].isBought;
			_goods.Add(shopItem);
			id++;
		}

		_goods.onChanged?.Invoke();
	}


	public void PurchaseOrEquip(int id)
	{
		if(_goods[id].isBought)
		{
			_goods[id].Equip();
			return;
		}

		if (_goods[id].cost <= _coins.value)
		{
			Purchase(id);
		}
	}


	private void Purchase(int id)
	{
		_coins.value -= _goods[id].cost;
		_gamePrefs.coins = _coins.value;

		_goods.onBought?.Invoke(id);
		_goods[id].Purchase();
		_shopSaveData.shopBoughtItems[id].isBought = true;
		SaveShopData();
	}


}

