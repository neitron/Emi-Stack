using System;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Button))]
public class ShopItemUi : MonoBehaviour
{


	[SerializeField] private TMPro.TextMeshProUGUI _costView;
	[SerializeField] private Image _thumbnail;
	[SerializeField] private GameObject _viewNonBought;
	[SerializeField] private GameObject _viewBought;


	private ShopScreenUi _shopUi;
	private int _id;



	private void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnShopItemClick);
	}


	public void Init(ShopScreenUi shopUi, IShopItemWithThumb item)
	{
		_shopUi = shopUi;

		_id = item.id;

		_thumbnail.sprite = item.thumbnail;

		if (item.isBought)
		{
			OpenToSelect();
			return;
		}

		_costView.text = $"$ {item.cost}";
	}


	internal void OpenToSelect()
	{
		_viewBought.SetActive(true);
		_viewNonBought.SetActive(false);
	}


	private void OnShopItemClick()
	{
		_shopUi.OnClickItem(_id);
	}


}
