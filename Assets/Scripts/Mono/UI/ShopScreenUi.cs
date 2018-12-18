﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShopScreenUi : MonoBehaviour
{

	[Header("Shop things")]
	[SerializeField] private ShopItemSetProfile _goods;
	[SerializeField] private LayoutGroup _view;
	[SerializeField] private GameObject _templatePrefab;


	private List<ShopItemUi> _goodsViewButtons;



	private void Awake()
	{
		_goods.onChanged += UpdateView;
		_goods.onBought += UpdateButtonAfterBought;
	}


	private void UpdateButtonAfterBought(int id)
	{
		_goodsViewButtons[id].CahngeToBoughtView();
	}


	public void UpdateView()
	{
		_goodsViewButtons = new List<ShopItemUi>();

		_templatePrefab.SetActive(true);
		
		foreach (IShopItemView item in _goods.items)
		{
			ShopItemUi temp = Instantiate(_templatePrefab, _view.transform).GetComponent<ShopItemUi>();
			temp.Init(this, item);
			_goodsViewButtons.Add(temp);

#if UNITY_EDITOR
			temp.gameObject.name += $"\t #{item.id}";
#endif
		}

		_templatePrefab.SetActive(false);
	}


	public void OnClickItem(int id)
	{
		_goods.tryToSelect?.Invoke(id);
	}
	

}
