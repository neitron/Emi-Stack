using System;
using System.Collections.Generic;
using UnityEngine;



public class DebugScreenUi : MonoBehaviour
{


	[SerializeField] private GameObject _messagePrefab;
	[SerializeField] private RectTransform _listRoot;
	[SerializeField] private StringListProfile _messages;


	private int _previousCount;



	private void Awake()
	{
		_previousCount = 0;
	}


	private void Update ()
	{
		if (_messages.Count != _previousCount)
		{
			UpdateScreen(_messages);
			_previousCount = _messages.Count;
		}
	}


	private void UpdateScreen(StringListProfile messages)
	{
		for(int i = _previousCount; i < messages.Count; i++)
		{
			TMPro.TextMeshProUGUI message = Instantiate(_messagePrefab, _listRoot).GetComponent<TMPro.TextMeshProUGUI>();
			message.text = messages[i];

			UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(_listRoot);
		}
	}


}
