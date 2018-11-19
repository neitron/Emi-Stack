using UnityEngine;



public class StatsUi : MonoBehaviour
{


	[Header("Score")]
	[SerializeField] private IntProfile _score;
	[SerializeField] private TMPro.TextMeshProUGUI _scoreView;

	[Header("Best Score")]
	[SerializeField] private IntProfile _bestScore;
	[SerializeField] private TMPro.TextMeshProUGUI _bestScoreView;


	[Header("Coins")]
	[SerializeField] private IntProfile _coins;
	[SerializeField] private TMPro.TextMeshProUGUI _coinsView;



	private void Start()
	{
		_score.OnChanged += (newValue) => { _scoreView.text = $"{newValue}"; };

		_bestScoreView.text = $"{_bestScore.value}";
		_bestScore.OnChanged += (newValue) => { _bestScoreView.text = $"{newValue}"; };

		_coinsView.text = $"{_coins.value}";
		_coins.OnChanged += (newValue) => { _coinsView.text = $"{newValue}"; };
	}


}