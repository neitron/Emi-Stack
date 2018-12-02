using System;
using UnityEngine;



[CreateAssetMenu()]
public class ShopItemSetProfile : RuntimeSetProfile<IShopItem>
{


	// ui outbox
	public Action<int> tryToSelect;

	// ui inbox
	public Action onChanged;
	public Action<int> onBought;


}
