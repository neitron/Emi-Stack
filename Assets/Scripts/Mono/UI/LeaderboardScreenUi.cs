﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;



public class LeaderboardScreenUi : MonoBehaviour
{


	[SerializeField] private LayoutGroup _view;
	[SerializeField] private GameObject _templatePrefab;
	[SerializeField] private GameObject _loadingIndicator;
	[SerializeField] private GameObject _noDataTitle;


	private Dictionary<string, LeaderboardPlayerUi> _playerScoreViews;



	public void UpdateView()
	{
		_noDataTitle.gameObject.SetActive(false);
		_loadingIndicator.SetActive(true);
		PlayGames.LoadScores(UpdateView);
	}


	private void UpdateView(IScore[] scores)
	{
		Debug.Log($"Scores have been loaded: {scores.Length}");

		if (scores.Length == 0)
		{
			_noDataTitle.gameObject.SetActive(true);
		}

		Queue<LeaderboardPlayerUi> viewStack =
			new Queue<LeaderboardPlayerUi>(_view.GetComponentsInChildren<LeaderboardPlayerUi>());
		viewStack.Enqueue(null);

		_playerScoreViews = new Dictionary<string, LeaderboardPlayerUi>();

		_templatePrefab.SetActive(false);
		foreach (IScore scoreEntity in scores)
		{
			LeaderboardPlayerUi temp;
			if (viewStack.Peek() == null)
			{
				temp = Instantiate(_templatePrefab, _view.transform).GetComponent<LeaderboardPlayerUi>();
			}
			else
			{
				temp = viewStack.Dequeue();
			}

			_playerScoreViews.Add(scoreEntity.userID, temp);
		}

		string[] ids = new string[_playerScoreViews.Keys.Count];
		_playerScoreViews.Keys.CopyTo(ids, 0);
		PlayGames.LoadUsers(ids, userProfiles =>
		{
			for (int i = 0; i < ids.Length; i++)
			{
				_playerScoreViews[userProfiles[i].id].Init(scores[i], userProfiles[i]);
			}
		});

		_loadingIndicator.SetActive(false);
	}


}
