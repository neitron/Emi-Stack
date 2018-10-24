using UnityEngine;



public class StatsUi : MonoBehaviour
{


	[Header("Score")]
	[SerializeField] private IntProfile _score;
	[SerializeField] private TMPro.TextMeshProUGUI _scoreView;

	[Header("Best Score")]
	[SerializeField] private IntProfile _bestScore;
	[SerializeField] private TMPro.TextMeshProUGUI _bestScoreView;
	


	void Update ()
	{
		_scoreView.text = $"{_score.value}";
		_bestScoreView.text = $"{_bestScore.value}";
	}


}
