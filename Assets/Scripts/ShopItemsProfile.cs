using System;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu()]
public class ShopItemsProfile : RuntimeSetProfile<IShopItem<CylinderProfile>>
{


	// ui poutbox
	public Action<int> tryToSelect;
	
	// ui inbox
	public Action onChanged;
	public Action<int> onBought;


}
