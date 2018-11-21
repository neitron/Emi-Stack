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
		GetComponent<Button>().onClick.AddListener(OnClick);
	}


	public void Init(ShopScreenUi shopUi, IShopItemView item)
	{
		_shopUi = shopUi;
		_id = item.id;
		_thumbnail.sprite = item.thumbnail;

		if (item.isBought)
		{
			CahngeToBoughtView();
			return;
		}

		_costView.text = $"$ {item.cost}";
	}


	internal void CahngeToBoughtView()
	{
		_viewBought.SetActive(true);
		_viewNonBought.SetActive(false);
	}


	private void OnClick()
	{
		_shopUi.OnClickItem(_id);
	}


}
