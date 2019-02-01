using UnityEngine;




[CreateAssetMenu(fileName = "Game Settings")]
public class SettingsProfile : ScriptableObject
{


	[Header("General")]
	public float scaleSpeed;
	public Vector3 cylinderStartScale;
	public float scaleThreshold;
	public int cameraRotationSpeed;
	public Vector3 outOffScreenOffset;
	public float moveToEndPosSpeed;
	public float biasToJoinCurentCylinder;
	public int commonSeries;
	public int commonSeriesPrize;


	[Header("Perfect match case")]
	public float perfectFitDifference;
	public float perfectSpeedModificator;
	public float perfectScaleUpModificator;
	public float perfectScalingAnimDuration;
	public int perfectSeries;
	public int perfectSeriesPrize;
	public DG.Tweening.Ease perfectScalingAnimEase;


}
