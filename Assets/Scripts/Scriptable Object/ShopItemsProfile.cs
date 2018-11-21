using System;
using UnityEngine;



[CreateAssetMenu()]
public class ShopItemsProfile : RuntimeSetProfile<IShopItem>
{


	// ui outbox
	public Action<int> tryToSelect;
	
	// ui inbox
	public Action onChanged;
	public Action<int> onBought;


}
