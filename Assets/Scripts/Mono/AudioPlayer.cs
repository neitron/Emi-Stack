using UnityEngine;
using UnityEngine.Audio;



public class AudioPlayer : MonoBehaviour
{

	[SerializeField] AudioMixer _audioMixer;
	[SerializeField] IntProfile _perfectSeries;
	[SerializeField] SettingsProfile _gameSettings;
	[SerializeField] private GameEventProfiler _soundOff;
	[SerializeField] private GameEventProfiler _soundOn;


	private GamePrefs _prefs;
	private float _masterVolume;



	private void Start()
	{
		_perfectSeries.OnChanged += PitchAccordingToPerfectSeries;
		_prefs = new GamePrefs();

		if (_prefs.isAudioMuted)
		{
			_soundOff.Raice();
		}
		else
		{
			_soundOn.Raice();
		}
		
	}


	public void SwitchSound(bool isOn)
	{
		if (!isOn)
		{
			_audioMixer.GetFloat("MasterVolume", out _masterVolume);
		}
		
		_audioMixer.SetFloat("MasterVolume", isOn ? _masterVolume : -80.0f);

		_prefs.isAudioMuted = !isOn;
	}


	private void PitchAccordingToPerfectSeries(int value)
	{
		float pitch = value * (1.0f / (float)(_gameSettings.perfectSeries - 1)) + 1.0f;
		_audioMixer.SetFloat("PerfectPitch", pitch);
	}


}
