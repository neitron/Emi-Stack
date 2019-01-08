using UnityEngine;
using DG.Tweening;



[RequireComponent(typeof(RectTransform))]
public class ScreensUi : MonoBehaviour
{


	[SerializeField] private float _screenSwitchingDuration;



	private RectTransform _rectTransform;



	private void Start()
	{
		_rectTransform = (RectTransform)transform;
	}

	
	public void MoveToShop()
	{
		_rectTransform.DOAnchorPosX(-1080, _screenSwitchingDuration);
	}


	public void MoveToLeaderboard()
	{
		_rectTransform.DOAnchorPosX(-2160, _screenSwitchingDuration);
	}


	public void MoveToMain()
	{
		_rectTransform.DOAnchorPosX(0, _screenSwitchingDuration);
	}


}
