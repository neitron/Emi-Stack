using UnityEngine;

internal class GamePrefs
{


	private const string BEST_SCORE_KEY = "BEST_SCORE";
	public int bestScore
	{
		get
		{
			return PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(BEST_SCORE_KEY, value);
		}
	}


}
