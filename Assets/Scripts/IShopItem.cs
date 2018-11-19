using UnityEngine;



public interface IShopItem<T> where T : ScriptableObject
{


	T profile { get; }
	int id { get; }

	void Purchase();
	void Equip();


}
