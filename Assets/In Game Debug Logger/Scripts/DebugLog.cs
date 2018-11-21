using System;
using UnityEngine;



public class DebugLog : MonoBehaviour
{


	[SerializeField] private StringListProfile _log;

	[Header("Colors by type")]
	[SerializeField] private Color _logColor;
	[SerializeField] private Color _warningColor;
	[SerializeField] private Color _errorColor;

	private string _logColorHex;
	private string _warningColorHex;
	private string _errorColorHex;



	private void Awake()
	{
		_log.Clear();

		_logColorHex = ColorUtility.ToHtmlStringRGBA(_logColor);
		_warningColorHex = ColorUtility.ToHtmlStringRGBA(_warningColor);
		_errorColorHex = ColorUtility.ToHtmlStringRGBA(_errorColor);
	}


	void OnEnable()
	{
		Application.logMessageReceived += LogMessage;
	}


	private void OnDisable()
	{
		Application.logMessageReceived -= LogMessage;
	}


	private void LogMessage(string condition, string stackTrace, LogType type)
	{
		string colorHex;
		switch (type)
		{
			case LogType.Error:
				colorHex = _errorColorHex;
				break;
			case LogType.Assert:
				colorHex = _errorColorHex;
				break;
			case LogType.Warning:
				colorHex = _warningColorHex;
				break;
			case LogType.Log:
				colorHex = _logColorHex;
				break;
			case LogType.Exception:
				colorHex = _errorColorHex;
				break;
			default:
				colorHex = _logColorHex;
				break;
		}

		string message = $"<color=#{colorHex}><b>{condition}</b>\n{stackTrace}</color>";
		_log.Add(message);
	}


}
