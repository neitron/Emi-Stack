using UnityEngine;
using UnityEngine.UI;



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
			Text message = Instantiate(_messagePrefab, _listRoot).GetComponent<Text>();
			message.text = messages[i];

			LayoutRebuilder.ForceRebuildLayoutImmediate(_listRoot);
		}
	}


}
