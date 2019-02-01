using UnityEngine;
#pragma warning disable 649



public class ShopBehaviour : MonoBehaviour
{


	[SerializeField] private GameEventProfiler _closeShop;

	[SerializeField] private Shop _shop;
	[SerializeField] private ShopReadyProfileSet _shopReadySetProfile;
	

	private GamePrefs _gamePrefs;
	private ShopSaveData _shopSaveData;



	private void Awake()
	{
		_gamePrefs = new GamePrefs();
		_shop.Init();
	}


	private void Start()
	{
		_shopReadySetProfile.items.Sort((cpl, cpr) => cpl.cost.CompareTo(cpr.cost));

		LoadShopData();
		GenerateGoods();

		_shop.onSelect += Select;
		_shop.onPurchase += Purchase;

		_shop.TryToSelect(_gamePrefs.selectedCylinderId);
	}


	private void LoadShopData()
	{
		GameSaver gs = new GameSaver();
		
		if (!gs.Load(out _shopSaveData))
		{
			SetDefaultData();
		}
		else if (_shopSaveData.shopBoughtItems.Count != _shopReadySetProfile.Count)
		{
			SyncData();
		}
	}


	private void SaveShopData()
	{
		GameSaver gameSaver = new GameSaver();
		gameSaver.Save(_shopSaveData);

		gameSaver.SaveAsJson(_shopSaveData, true); // TODO: make it false
		gameSaver.SaveAsJson(_shop.goods, true); // TODO: make it false
	}


	private void SyncData()
	{
		for (int i = _shopSaveData.shopBoughtItems.Count; i < _shopReadySetProfile.Count; i++)
		{
			_shopSaveData.AddDefault();
		}
	}


	private void SetDefaultData()
	{
		for (int id = 0; id < _shopReadySetProfile.Count; id++)
		{
			_shopSaveData.AddDefault();
		}
		_shopSaveData.shopBoughtItems[0].isBought = true;

		if (_gamePrefs == null)
		{
			_gamePrefs = new GamePrefs();
		}
		Select(0);
	}
	

	private void GenerateGoods()
	{
		_shop.GenerateGoods(_shopReadySetProfile, GoodActivator);
	}
	

	private IShopItem GoodActivator(int id, IShopReadyProfile profile)
	{
		IShopItem temp = new ShopItem(profile, id);
		temp.isBought = _shopSaveData.shopBoughtItems[id].isBought;
		return temp;
	}


	private void Select(int id)
	{
		_gamePrefs.selectedCylinderId = id;
	}


	private void Purchase(int id)
	{
		_gamePrefs.coins = _shop.coins.value;
		_shopSaveData.shopBoughtItems[id].isBought = true;
		SaveShopData();
	}


#if UNITY_EDITOR
	#region HELPERS
	[ContextMenu("Reset Shop")]
	private void ResetShop()
	{
		_shopSaveData = new ShopSaveData();
		SetDefaultData();
		SaveShopData();
	}


	[ContextMenu("Add 100 Cons")]
	private void AddCoins()
	{
		_shop.coins.value += 100;
		_gamePrefs.coins = _shop.coins.value;
	}
	#endregion
#endif


}

