using UnityEngine;

internal class GameState
{


	public enum State { Home, Session, End }

	private State _state;
	public State state
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			Input.ResetInputAxes();
			Debug.Log(state);
		}
	}


}
