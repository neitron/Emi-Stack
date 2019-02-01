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

	private const string SELECTED_CYLINDER_ID_KEY = "SELECTED_CYLINDER_ID";
	public int selectedCylinderId
	{
		get
		{
			return PlayerPrefs.GetInt(SELECTED_CYLINDER_ID_KEY, 0);
		}
		set
		{
			PlayerPrefs.SetInt(SELECTED_CYLINDER_ID_KEY, value);
		}
	}

	private const string IS_AUDIO_MUTED_KEY = "IS_AUDIO_MUTED";
	public bool isAudioMuted
	{
		get
		{
			return PlayerPrefs.GetInt(IS_AUDIO_MUTED_KEY, 1) == 1 ? true : false;
		}
		set
		{
			PlayerPrefs.SetInt(IS_AUDIO_MUTED_KEY, value ? 1 : 0);
		}
	}



	public void Reset()
	{
		bestScore = 0;
		coins = 0;
		selectedCylinderId = 0;
		PlayerPrefs.Save();
	}


}
