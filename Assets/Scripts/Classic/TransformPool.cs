using System.Collections.Generic;
using UnityEngine;



internal class TransformPool
{
	

	public delegate void EntityActivator(out Transform transform);


	public Transform next => GetNext();

	private const int POOL_SIZE = 50;

	private EntityActivator _activator;
	private List<Transform> _pool;
	private int _currentIndex;
	private Transform _root;



	public TransformPool(EntityActivator activator)
	{
		if (activator == null)
		{
			Debug.LogError("Pool's entities activator is null");
			return;
		}
		_activator = activator;

		_root = new GameObject("Pool").transform;

		_pool = new List<Transform>(POOL_SIZE);
		for (int i = 0; i < POOL_SIZE; i++)
		{
			_pool.Add(CreateEntity());	
		}
	}


	private Transform CreateEntity()
	{
		Transform temp = null;

		_activator.Invoke(out temp);

		SetActive(temp, false);
		temp.parent = _root;

		return temp;
	}


	private Transform GetNext()
	{
		if (_currentIndex == _pool.Count)
		{
			_pool.Add(CreateEntity());
		}

		Transform temp = _pool[_currentIndex++];
		SetActive(temp, true);

		return temp;
	}


	public void Reset(Transform trans)
	{
		if (trans.parent != _root)
		{
			return;
		}

		_currentIndex--;
		SetActive(trans, false);
	}


	private void SetActive(Transform trans, bool isActive)
	{
		trans.gameObject.SetActive(isActive);
		trans.hideFlags = isActive ? HideFlags.None : HideFlags.HideInHierarchy;
	}


}