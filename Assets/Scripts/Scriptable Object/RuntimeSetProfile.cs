using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class RuntimeSetProfile<T> : ScriptableObject, IEnumerable
{


	public List<T> items = new List<T>();

	public int Count => items.Count;
	public T this[int index] => items[index];



	public void Add(T newItem)
	{
		if (!items.Contains(newItem))
		{
			items.Add(newItem);
		}
	}


	public void Remove(T item)
	{
		if (items.Contains(item))
		{
			items.Remove(item);
		}
	}


	public void Clear()
	{
		items.Clear();
	}


	public IEnumerator GetEnumerator()
	{
		return items.GetEnumerator();
	}


}




