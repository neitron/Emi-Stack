using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu()]
public class GameEventProfiler : ScriptableObject
{


	public List<GameEventListener> listeners = new List<GameEventListener>(); 



	public void Raice()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised();
		}
	}


	public void Add(GameEventListener listener)
	{
		listeners.Add(listener);
	}


	public void Remove(GameEventListener listener)
	{
		listeners.Remove(listener);
	}


}
