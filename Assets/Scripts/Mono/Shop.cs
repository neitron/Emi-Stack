using System;
using System.Collections.Generic;
using UnityEngine;



public class Shop : MonoBehaviour
{


	[SerializeField] private GameEventProfiler _closeShop;
	[SerializeField] private ShopItemsProfile _goods;
	[SerializeField] private ShopReadyProfileSet _goodsProfiles;
	[SerializeField] private IntProfile _coins;

	
	private GamePrefs _gamePrefs;
	private ShopSaveData _shopSaveData;



	private void Awake()
	{
		_gamePrefs = new GamePrefs();
		_goods.tryToSelect += TryToSelect;
	}


	private void Start()
	{
		_goodsProfiles.items.Sort((cpl, cpr) => { return cpl.cost.CompareTo(cpr.cost); });

		LoadShopData();
		GenerateGoods();

		TryToSelect(_gamePrefs.selectedCylinderId);
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
		GenerateGoods(_goodsProfiles.items, (id, profile) => { return new ShopItem(profile, id); });
		_goods.onChanged?.Invoke();
	}
	

	private void GenerateGoods<T>(List<T> itemProfiles, Func<int, T, IShopItem> caster)
	{
		if (caster == null)
		{
			Debug.LogError("Caster is null");
			return;
		}

		foreach (T goodProfile in itemProfiles)
		{
			int id = _goods.Count;
			IShopItem shopItem = caster(id, goodProfile);
			shopItem.isBought = _shopSaveData.shopBoughtItems[id].isBought;
			_goods.Add(shopItem);
		}
	}


	public void TryToSelect(int id)
	{
		if(_goods[id].isBought)
		{
			Select(id);
			return;
		}

		if (_goods[id].cost <= _coins.value)
		{
			Purchase(id);
		}
	}


	private void Select(int id)
	{
		_goods[id].Select();
		_gamePrefs.selectedCylinderId = id;

		_closeShop.Raice();
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


	[ContextMenu("Reset Shop")]
	private void ResetShop()
	{
		_shopSaveData = new ShopSaveData();
		SetDefaultData();
		SaveShopData();
	}


	[ContextMenu("Add 10 Cons")]
	private void AddCoins()
	{
		_coins.value += 10;
		_gamePrefs.coins = _coins.value;
	}


}

