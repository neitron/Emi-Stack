using UnityEngine;
using UnityEngine.Events;




public class GameEventListener : MonoBehaviour
{


	[SerializeField] private GameEventProfiler _event;
	[SerializeField] private UnityEvent _response;
	


	private void OnEnable()
	{
		_event.Add(this);
	}


	private void OnDisable()
	{
		_event.Remove(this);
	}


	internal void OnEventRaised()
	{
		_response?.Invoke();
	}


}
