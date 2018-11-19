using System;
using UnityEngine;



[CreateAssetMenu()]
internal class IntProfile : ScriptableObject
{


	[SerializeField] private int _value;

	public int value
	{
		get
		{
			return _value;
		}
		set
		{
			if (value == _value)
			{
				return;
			}

			_value = value;
			OnChanged?.Invoke(_value);
		}
	}

	public event Action<int> OnChanged;


}
