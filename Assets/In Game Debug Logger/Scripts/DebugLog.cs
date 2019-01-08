using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using UnityEngine;



public class DebugLog : MonoBehaviour
{


	private struct NonFormatedLogMessage
	{


		public string condition { get; set; }
		public string stackTrace { get; set; }
		public string hexColor { get; set; }
		public LogType type { get; set; }



		public NonFormatedLogMessage(string condition, string stackTrace, string hexColor, LogType type)
		{
			this.condition = condition;
			this.stackTrace = stackTrace;
			this.hexColor = hexColor;
			this.type = type;
		}


	}



	[SerializeField] private StringListProfile _log;

	[Header("Colors by type")]
	[SerializeField] private Color _logColor;
	[SerializeField] private Color _warningColor;
	[SerializeField] private Color _errorColor;

	[Header("Report credits")]
	[SerializeField] private string _email;
	[SerializeField] private string _password;

	private string _logColorHex;
	private string _warningColorHex;
	private string _errorColorHex;
	private List<NonFormatedLogMessage> _nonFormatedLog;



	private void Awake()
	{
		_log.Clear();
		_nonFormatedLog = new List<NonFormatedLogMessage>();

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
		_nonFormatedLog.Add(new NonFormatedLogMessage(condition, stackTrace, colorHex, type));
	}


	public void SendReportByMail()
	{
		MailMessage mail = new MailMessage(_email, _email);
		mail.IsBodyHtml = true;
		mail.Subject = $"EmiStack Report {Application.version} : {DateTime.Now.ToString(DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt"))}";

		mail.Body +=
			$"SystemInfo.deviceName: {SystemInfo.deviceName}\n" +
			$"SystemInfo.deviceModel: {SystemInfo.deviceModel}\n" +
			$"SystemInfo.deviceType: {SystemInfo.deviceType}\n" +
			$"SystemInfo.operatingSystemFamily: {SystemInfo.operatingSystemFamily}\n" +
			$"SystemInfo.operatingSystem: {SystemInfo.operatingSystem}\n=============================================================================\n\n";

		foreach (NonFormatedLogMessage log in _nonFormatedLog)
		{
			mail.Body += $"<h3 style=\"color:#{log.hexColor};\">[{log.type.ToString()}] - {log.condition}</h3>";
			mail.Body += $"<p style=\"color:#{log.hexColor};\">{log.stackTrace}</p>\n__________________________________________________________\n";
		}

		SmtpClient smtpServer = new SmtpClient()
		{
			Host = "smtp.gmail.com",
			Port = 587,
			Credentials = new System.Net.NetworkCredential(_email, _password),
			EnableSsl = true
		};
		ServicePointManager.ServerCertificateValidationCallback =
			(s, certificate, chain, sslPolicyError) => { return true; };
		smtpServer.SendAsync(mail, "");
		smtpServer.SendCompleted += (sender, e) => { Debug.Log($"Send Mail Completed {e.Cancelled}/{e.Error}"); };
	}


}
