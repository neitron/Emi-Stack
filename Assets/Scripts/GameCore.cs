using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



internal partial class GameCore : MonoBehaviour
{


	[SerializeField] private GameObject _firstLayer;
	[SerializeField] private Vector3 _layerOffset;
	[SerializeField] private float _scaleThreshold;
	[SerializeField] private float _scaleSpeed;
	[SerializeField] private IntProfile _layersScoreProfile;
	[SerializeField] private IntProfile _bestScore;
	[SerializeField, ColorUsage(false, true)] private Color _firstColor;
	[SerializeField, ColorUsage(false, true)] private Color _errorColor;
	[SerializeField, ColorUsage(false, true)] private Color _successColor;
	[SerializeField] private ParticleSystem _splashFx;
	[SerializeField] private AudioSource _splashAudioFx;


	private enum TouchState { Released, Up, Down }
	private TouchState _touchState;

	private delegate void UpdateCallback();
	private Dictionary<GameState.State, UpdateCallback> _gameStateCallbacks;
	private GameState _gameState;
	private GamePrefs _gamePrefs;

	private Transform _mainCam;
	private Transform _currentLayer;
	private List<Transform> _pole;
	private TransformPool _layersPool;
	private Vector3 _scaleVector;
	private Vector3 _mainCamStartPosition;
	private Vector3 _splashFxStartPosition;
	private float _prevLayerScale;



	private void Awake()
	{
		_gameState = new GameState();
		_gamePrefs = new GamePrefs();
	}


	private void Start ()
	{
		_layersPool = new TransformPool((out Transform trans) => 
			{
				trans = Instantiate(_firstLayer).transform;
				ResetLayer(trans);
			} );

		_mainCam = Camera.main.transform;
		_mainCamStartPosition = _mainCam.position;
		_splashFxStartPosition = _splashFx.transform.position;
		_scaleVector = new Vector3(1.0f, 0.0f, 1.0f) * _scaleSpeed;

		_gameStateCallbacks = new Dictionary<GameState.State, UpdateCallback>();
		_gameStateCallbacks.Add(GameState.State.Home, HomeUpdate);
		_gameStateCallbacks.Add(GameState.State.Session, SessionUpdate);

		_bestScore.value = _gamePrefs.bestScore;
		_layersScoreProfile.value = 0;

		_firstLayer.GetComponent<Cylinder>().ChangeEmissionColor(Color.black, 0.0f);
		
		_pole = new List<Transform>();
	}
	

	private void Reset()
	{
		_mainCam.DOMove(_mainCamStartPosition, 0.3f);
		_splashFx.transform.position = _splashFxStartPosition;

		foreach (Transform trans in _pole)
		{
			ResetLayer(trans);
			_layersPool.Reset(trans);
		}
		_pole.Clear();

		_currentLayer = _firstLayer.transform;
		_prevLayerScale = _currentLayer.localScale.x;
		_layersScoreProfile.value = 1;

		_firstLayer.GetComponent<Cylinder>().ChangeEmissionColor(_firstColor, 0.0f);
		_splashAudioFx.Play();
	}


	private void ResetLayer(Transform trans)
	{
		trans.GetComponent<Cylinder>().ChangeEmissionColor(Color.black, 0.0f);
		trans.localScale = Vector3.up;
	}


	private void Update ()
	{
		if (_gameStateCallbacks.ContainsKey(_gameState.state))
		{
			_gameStateCallbacks[_gameState.state]?.Invoke();
		}
	}


	private void HomeUpdate()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Reset();
			_gameState.state = GameState.State.Session;
		}
	}


	private void SessionUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			ChangeTouchState(TouchState.Down);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			ChangeTouchState(TouchState.Up);
		}
	}


	private void TouchDown()
	{
		_currentLayer = _layersPool.next;
		_currentLayer.position = _layerOffset * _layersScoreProfile.value;

		_pole.Add(_currentLayer);

		StartCoroutine(ScaleUpLayer());
	}


	private IEnumerator ScaleUpLayer()
	{
		var wait = new WaitForEndOfFrame();
		while (_currentLayer.localScale.x / _prevLayerScale < _scaleThreshold)
		{
			_currentLayer.localScale += _scaleVector * Time.deltaTime;
			yield return wait;
		}
		EndGame();
		yield return null;
	}


	private void TouchUp()
	{
		StopAllCoroutines();

		if (_currentLayer.localScale.x > _prevLayerScale)
		{
			EndGame();
			return;
		}

		_currentLayer.GetComponent<Cylinder>().ChangeEmissionColor(_successColor, 0.2f);
		_splashFx.transform.position += _layerOffset;
		_splashFx.Play();

		_splashAudioFx.Play();

		_prevLayerScale = _currentLayer.localScale.x;
		_mainCam.DOMove(_mainCam.position + _layerOffset, 0.1f);
		_layersScoreProfile.value++;
	}
	

	private void EndGame()
	{
		_gameState.state = GameState.State.End;

		_currentLayer.GetComponent<Cylinder>().ChangeEmissionColor(_errorColor, 0.5f, ShopwPole);

		if (_layersScoreProfile.value > _bestScore.value)
		{
			_bestScore.value = _layersScoreProfile.value;
			_gamePrefs.bestScore = _bestScore.value;
		}
	}


	private void ShopwPole()
	{
		ResetLayer(_currentLayer);
		_layersPool.Reset(_currentLayer);
		_pole.Remove(_currentLayer);

		Vector3 newPos = (_currentLayer.position - _firstLayer.transform.position);
		newPos.z -= newPos.magnitude / Mathf.Tan(Mathf.Deg2Rad * (Camera.main.fieldOfView * 0.5f + _mainCam.rotation.eulerAngles.x));
		newPos.y -= 0.5f;

		_mainCam.DOMove(_firstLayer.transform.position + newPos, 0.3f);

		_gameState.state = GameState.State.Home;
	}


	private void ChangeTouchState(TouchState state)
	{
		_touchState = state;
		//Debug.Log(state);

		switch (_touchState)
		{
			case TouchState.Down:
				TouchDown();
				break;
			case TouchState.Up:
				TouchUp();
				break;
		}
	}


}
