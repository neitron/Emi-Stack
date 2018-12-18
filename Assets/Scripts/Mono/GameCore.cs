using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GameCore : MonoBehaviour
{


	[SerializeField] private Transform _mainCam;

	[Header("Profiles")]
	[SerializeField] private SettingsProfile _gameSettings;
	[SerializeField] private IntProfile _layersScoreProfile;
	[SerializeField] private IntProfile _bestScore;
	[SerializeField] private IntProfile _coins;
	[SerializeField] private GameEventProfiler _putting;
	[SerializeField] private GameEventProfiler _perfectMatch;
	[SerializeField] private GameEventProfiler _errorMatch;
	[SerializeField] private GameEventProfiler _endGame;

	[Header("Prototype")]
	[SerializeField] private GameObject _firstLayer;
	
	[Header("Color Schema")]
	[SerializeField, ColorUsage(false, true)] private Color _firstColor;
	[SerializeField, ColorUsage(false, true)] private Color _errorColor;
	[SerializeField, ColorUsage(false, true)] private Color _successColor;
	[SerializeField, ColorUsage(false, true)] private Color _perfectColor;

	[Header("FX")]
	[SerializeField] private Transform _visualFx;



	private enum TouchState { Released, Up, Down }
	private TouchState _touchState;

	private delegate void UpdateCallback();
	private Dictionary<GameState.State, UpdateCallback> _gameStateCallbacks;
	private GameState _gameState;
	private GamePrefs _gamePrefs;

	private Transform _currentLayer;
	private List<Transform> _stack;
	private TransformPool _layersPool;
	private Vector3 _scaleVector;
	private Vector3 _mainCamStartPosition;
	private Quaternion _mainCamStartRotation;
	private Vector3 _splashFxStartPosition;
	private Vector3 _layerOffset;
	private Vector3 _endLayerPosition;
	private Vector3 _perfectMatchScaleUpFactor;
	private float _prevLayerScale;
	private bool _isPrevPerfect;
	private Animation _graundingAnimation;



	private void Awake()
	{
		_gameState = new GameState();
		_gamePrefs = new GamePrefs();
		
		LoadUiScene();
	}

	
	[System.Obsolete("Temp solution")] private void LoadUiScene()
	{
#if !UNITY_EDITOR
		UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Additive);
#endif
	}


	private void Start ()
	{
		_gameStateCallbacks = new Dictionary<GameState.State, UpdateCallback>();
		_gameStateCallbacks.Add(GameState.State.Home, HomeUpdate);
		_gameStateCallbacks.Add(GameState.State.Session, SessionUpdate);

		LoadPrefs();

		_mainCamStartPosition = _mainCam.position;
		_mainCamStartRotation = _mainCam.rotation;

		_stack = new List<Transform>();

		_layerOffset = new Vector3(0, _gameSettings.cylinderStartScale.y * 0.5f, 0.0f);
		_scaleVector = new Vector3(_gameSettings.scaleSpeed, 0.0f, _gameSettings.scaleSpeed);
		_perfectMatchScaleUpFactor = new Vector3(_gameSettings.perfectScaleUpModificator, 0.0f, _gameSettings.perfectScaleUpModificator);

		_firstLayer.transform.localScale = new Vector3(1.0f, _gameSettings.cylinderStartScale.y, 1.0f);
		InitPool();

		_layersScoreProfile.value = 0;

		_splashFxStartPosition = _visualFx.transform.position;

		// Debug console test log
		Debug.Log("Test Log");
		Debug.LogWarning("Test Warning");
		Debug.LogError("Tets Error");
	}


	private void LoadPrefs()
	{
		_bestScore.value = _gamePrefs.bestScore;
		_coins.value = _gamePrefs.coins;
	}


	private void InitPool()
	{
		_firstLayer.GetComponent<Cylinder>().ChangeEmissionColor(Color.black, 0.0f);
		_layersPool = new TransformPool((out Transform trans) =>
		{
			trans = Instantiate(_firstLayer).transform;
			ResetLayer(trans);
		});
	}


	private void Reset()
	{
		_mainCam.DOMove(_mainCamStartPosition, 0.3f);
		_visualFx.transform.position = _splashFxStartPosition;

		foreach (Transform trans in _stack)
		{
			ResetLayer(trans);
			_layersPool.Reset(trans);
		}
		_stack.Clear();

		_currentLayer = _firstLayer.transform;
		_prevLayerScale = _currentLayer.localScale.x;
		_layersScoreProfile.value = 1;

		_firstLayer.GetComponent<Cylinder>().ChangeEmissionColor(_firstColor, 0.0f);
		_putting.Raice();
	}


	private void ResetLayer(Transform trans)
	{
		trans.GetComponent<Cylinder>().ChangeEmissionColor(Color.black, 0.0f);
		trans.localScale = _gameSettings.cylinderStartScale;
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
		// Home behaviour
	}


	public void StartGame()
	{
		Reset();
		_gameState.state = GameState.State.Session;
	}


	private void SessionUpdate()
	{
		// Session's behaviour
		_mainCam.Rotate(Vector3.up, _gameSettings.cameraRotationSpeed * Time.deltaTime);
	}


	private void TouchDown()
	{
		_currentLayer = _layersPool.next;
		_endLayerPosition = _layerOffset * _layersScoreProfile.value;
		_currentLayer.position = _endLayerPosition + _gameSettings.outOffScreenOffset;

		_stack.Add(_currentLayer);

		float perfectFactor = 1.0f;
		if (_isPrevPerfect)
		{
			perfectFactor = _gameSettings.perfectSpeedModificator;
			_isPrevPerfect = false;
		}

		StartCoroutine(ScaleUpLayer(perfectFactor));
	}


	private IEnumerator ScaleUpLayer(float scaleFactor)
	{
		var wait = new WaitForEndOfFrame();

		// Falling
		Vector3 fallDownStep = (_endLayerPosition - _currentLayer.position) * _gameSettings.moveToEndPosSpeed;
		while (_currentLayer.position.y - _endLayerPosition.y > _gameSettings.biasToJoinCurentCylinder)
		{
			_currentLayer.position += fallDownStep * Time.deltaTime;
			yield return wait;
		}
		_currentLayer.position = _endLayerPosition;

		// Grounding Animation
		Animation graundingAnimation = _currentLayer.GetComponent<Animation>();
		graundingAnimation.Play();
		do
		{
			yield return wait;
		} while (graundingAnimation.isPlaying);

		// Grow Up
		while (_currentLayer.localScale.x / _prevLayerScale < _gameSettings.scaleThreshold)
		{
			_currentLayer.localScale += _scaleVector * scaleFactor * Time.deltaTime;
			yield return wait;
		}
		EndGame();
		yield return null;
	}


	private void TouchUp()
	{
		StopAllCoroutines();
		
		if (_currentLayer.position != _endLayerPosition || _currentLayer.localScale.x > _prevLayerScale)
		{
			EndGame();
			return;
		}

		if (_prevLayerScale - _currentLayer.localScale.x < _gameSettings.perfectFitDifference)
		{
			_currentLayer.localScale = new Vector3(_prevLayerScale, _gameSettings.cylinderStartScale.y, _prevLayerScale);
			PerfectMatch();
		}
		else
		{
			_prevLayerScale = _currentLayer.localScale.x;
		}

		_currentLayer.GetComponent<Cylinder>().ChangeEmissionColor(_successColor, 0.2f);

		_putting.Raice();
		_visualFx.transform.position += _layerOffset;

		_mainCam.DOMove(_mainCam.position + _layerOffset, 0.1f);
		_layersScoreProfile.value++;
	}


	private void PerfectMatch()
	{
		_coins.value++;
		_gamePrefs.coins = _coins.value;

		_currentLayer.GetComponent<Cylinder>().ChangeEmissionColor(_perfectColor, 0.5f);

		_isPrevPerfect = true;

		_perfectMatch.Raice();

		int lastInd = _stack.Count - 1;
		for (int i = lastInd; i >=0; i--)
		{
			Vector3 endScale = _stack[i].localScale + _perfectMatchScaleUpFactor;

			if(endScale.x > _firstLayer.transform.localScale.x)
			{
				endScale = _firstLayer.transform.localScale;
				
			} 
			else if (endScale.x == _firstLayer.transform.localScale.x)
			{
				break;
			}
			
			_stack[i].DOScale(endScale, _gameSettings.perfectScalingAnimDuration)
				.SetEase(_gameSettings.perfectScalingAnimEase).SetDelay((lastInd - i) * 0.02f);

			if (i == lastInd)
			{
				_prevLayerScale = endScale.x;
			}
		}

	}


	private void EndGame()
	{
		_gameState.state = GameState.State.End;
		
		_currentLayer.GetComponent<Cylinder>().ChangeEmissionColor(_errorColor, 0.5f, ShopwPole);
		_errorMatch.Raice();

		if (_layersScoreProfile.value > _bestScore.value)
		{
			_bestScore.value = _layersScoreProfile.value;
			_gamePrefs.bestScore = _bestScore.value;
		}
	}


	private void ShopwPole()
	{
		_gameState.state = GameState.State.Home;

		_mainCam.DORotateQuaternion(_mainCamStartRotation, 0.3f);

		ResetLayer(_currentLayer);
		_layersPool.Reset(_currentLayer);
		_stack.Remove(_currentLayer);

		if(_stack.Count < 3)
		{
			_endGame.Raice();
			return;
		}

		Vector3 newPos = (_currentLayer.position - _firstLayer.transform.position);
		newPos.z -= newPos.magnitude / Mathf.Tan(Mathf.Deg2Rad * (Camera.main.fieldOfView * 0.5f + Camera.main.transform.localRotation.eulerAngles.x));
		newPos.y += 0.5f;

		_mainCam.DOMove(_firstLayer.transform.position + newPos, 0.3f).OnComplete(_endGame.Raice);
	}


	private void ChangeTouchState(TouchState state)
	{
		// Debug.Log(state);
		_touchState = state;

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


	public void OnPointerDown()
	{
		if(_gameState.state == GameState.State.Session)
			ChangeTouchState(TouchState.Down);
	}


	public void OnPointerUp()
	{
		if (_gameState.state == GameState.State.Session)
			ChangeTouchState(TouchState.Up);
	}


}
