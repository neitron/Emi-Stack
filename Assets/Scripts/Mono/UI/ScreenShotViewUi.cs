using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ScreenShotViewUi : MonoBehaviour
{


	[SerializeField] private IntProfile _score;
	[SerializeField] private TextMeshProUGUI _scoreView;



	public void UpdateView ()
	{
		_scoreView.text = $"{_score.value} LAYERS";
	}


}
