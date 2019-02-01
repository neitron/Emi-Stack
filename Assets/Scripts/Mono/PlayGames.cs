using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.IO;
using System.Collections;

public class PlayGames : MonoBehaviour
{

	[SerializeField] private GameEventProfiler _screenCaptureStart;
	[SerializeField] private GameEventProfiler _screenCaptureEnd;


	public static bool isSharingScreenshot { get; private set; }


	private const string sharePluginName = "com.neitronnative.shareplugin";


	private bool _isAppFocus { get; set; }



	private class ShareImageCallback : AndroidJavaProxy
	{


		private Action<int> shareHandler;

		public ShareImageCallback(Action<int> shareHandler) : base(sharePluginName + ".ShareImageCallback")
		{
			this.shareHandler = shareHandler;
		}


		public void OnShareComplete(int result)
		{
			Debug.Log($"ShareComplete: {result}");
			isSharingScreenshot = false;
			
			shareHandler?.Invoke(result);
		}


	}



	private void Start ()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.Activate();

		SignIn();
	}


	public void SignIn()
	{
		Social.localUser.Authenticate((bool success) => 
		{
			string reportMessage = "User SignIn";
			if (success)
			{
				if (Social.localUser != null)
				{
					reportMessage += $" success: {Social.localUser.userName}";
				}
				reportMessage += "success";
			}
			else
			{
				reportMessage += "failed";
			}

			Debug.Log("User SignIn" + (success ? $" success: {Social.localUser?.userName}" : " failed"));
		});
	}


	private void OnApplicationQuit()
	{
		PlayGamesPlatform.Instance.SignOut();
	}


	private void OnApplicationFocus(bool focus)
	{
		_isAppFocus = focus;
	}


	#region Achievements

	public static void UnlockAchievement(string id)
	{
		Social.ReportProgress(id, 10, success => { });
	}


	public static void IncrementAchievement(string id, int stepsToIncrement)
	{
		PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
	}


	public void ShowAchievementsUi()
	{
		Social.ShowAchievementsUI();
	}

	#endregion Achievements


	#region Leaderboard

	public static void AddScoreToLeaderboard(string leaderboardId, long score)
	{
		Social.ReportScore(score, leaderboardId, success => { });
	}


	public void ShowLeaderboardUi()
	{
		PlayGamesPlatform.Instance.SetDefaultLeaderboardForUI(GPGSIds.leaderboard_leaderboard);
		Social.ShowLeaderboardUI();
	}


	public static void LoadScores(Action<IScore[]> callback, bool isNetworkOnly)
	{
		PlayGamesPlatform.Instance.LoadScores(
			GPGSIds.leaderboard_leaderboard,
			LeaderboardStart.TopScores,
			100,
			LeaderboardCollection.Public,
			LeaderboardTimeSpan.AllTime,
			data =>
			{
				Debug.Log($"Update players score list: {(data.Valid ? "Success" : "Failed")}, Approx: {data.ApproximateCount}");
				callback?.Invoke(data.Scores);
			},
			isNetworkOnly);
	}


	public static void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
	{
		Social.LoadUsers(userIds, callback);
	}

	#endregion Leaderboard


	#region Sharing


	public void Share(string caption)
	{
		Share(caption, SharingCompleteCallback);
	}


	private void SharingCompleteCallback(int res)
	{
		Debug.Log($"COMPLETE: {res}");
	}


	public void Share(string caption, Action<int> shareComplete)
	{
		if (isSharingScreenshot)
		{
			Debug.Log("Already sharing screenshot - abort");
			return;
		}
		isSharingScreenshot = true;
		_screenCaptureStart.Raice();
		StartCoroutine(waitForEndOfFrame(caption, shareComplete));
	}


	private IEnumerator waitForEndOfFrame(string caption, Action<int> shareComplete)
	{
		yield return new WaitForEndOfFrame();
		
		string fileName = "screenshot.png";
		ScreenCapture.CaptureScreenshot(fileName, 1);
		_screenCaptureEnd.Raice();

		// Wait until Screen Capture Saving Process completed
		string path = Path.Combine(Application.persistentDataPath, fileName);
		while (IsFileUnavailable(path))
		{
			Debug.Log($"{fileName} file locked");
			yield return new WaitForSeconds(.02f);
		}
		Debug.Log($"Screen capture is saved: {path}");

		if (Application.platform == RuntimePlatform.Android)
		{
			AndroidJavaClass shareProviderClass = new AndroidJavaClass($"{sharePluginName}.ShareProvider");
			AndroidJavaClass unityContext = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = unityContext.GetStatic<AndroidJavaObject>("currentActivity");
			shareProviderClass.SetStatic("mainActivity", activity);

			AndroidJavaObject shareProvider = shareProviderClass.CallStatic<AndroidJavaObject>("getInstance");
			shareProvider.Call("ShareImage", new object[] { null, fileName, caption, new ShareImageCallback(shareComplete) });
		}
	}


	private bool IsFileUnavailable(string path)
	{
		// if file doesn't exist, return true
		if (!File.Exists(path))
			return true;

		FileInfo file = new FileInfo(path);
		FileStream stream = null;

		try
		{
			stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
		}
		catch (IOException)
		{
			//the file is unavailable because it is:
			//still being written to
			//or being processed by another thread
			//or does not exist (has already been processed)
			return true;
		}
		finally
		{
			if (stream != null)
				stream.Close();
		}

		//file is not locked
		return false;
	}


	#endregion


}
