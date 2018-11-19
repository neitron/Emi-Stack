using System;
using UnityEngine;

public class GamePrefs
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

	private const string COINS_KEY = "COINS";
	public int coins
	{
		get
		{
			return PlayerPrefs.GetInt(COINS_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(COINS_KEY, value);
		}
	}


	public void Reset()
	{
		bestScore = 0;
		coins = 0;
		PlayerPrefs.Save();
	}


}
