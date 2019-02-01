using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;



public class LeaderboardPlayerUi : MonoBehaviour
{


	[SerializeField] private Text _nameView; // Because of Unicode needs
	[SerializeField] private TMPro.TextMeshProUGUI _scoreView;
	[SerializeField] private TMPro.TextMeshProUGUI _rankView;

	[SerializeField] private Image _avatarView;



	internal void Init(IScore scoreEntity, IUserProfile userProfile)
	{
		_rankView.text = scoreEntity.rank.ToString();
		_nameView.text = userProfile.userName;
		_scoreView.text = scoreEntity.formattedValue;
		// Pick a random, saturated and not-too-dark color
		_avatarView.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.55f, 0.55f, 0.84f, 0.84f);

		gameObject.SetActive(true);
		
		StartCoroutine(LoadProfileImage(userProfile));

		
		Debug.Log($"player: {scoreEntity.userID}_{scoreEntity.formattedValue}");
	}


	private IEnumerator LoadProfileImage(IUserProfile userProfile)
	{
		float waitTime = 0.1f;
		float timeLimit = 10.0f;
		float totalTime = 0.0f;

		WaitForSeconds wait = new WaitForSeconds(waitTime);

		while (userProfile.image == null && totalTime < timeLimit)
		{
			Debug.Log($"Loading Image {totalTime}");
			totalTime += waitTime;
			yield return wait;
		}

		if (userProfile.image != null)
		{
			Debug.Log($"Image was loaded");
			Texture2D tex = userProfile.image;
			_avatarView.sprite = Sprite.Create(userProfile.image, new Rect(0, 0, tex.width, tex.height), new Vector3(0.5f, 0.5f));
			_avatarView.color = Color.white;
		}
		else
		{
			Debug.Log($"Loading proccess failed - to long");
		}

		yield return null;
	}


}
