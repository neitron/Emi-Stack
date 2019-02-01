using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class SplashFx : MonoBehaviour
{


	[SerializeField] private IntProfile _perfectSeries;


	private ParticleSystem _splashSystem;



	private void Start ()
	{
		_splashSystem = GetComponent<ParticleSystem>();
	}


	public void PlayAccordingToSeries()
	{
		ParticleSystem.Burst burst = _splashSystem.emission.GetBurst(0);
		burst.cycleCount = _perfectSeries.value;
		_splashSystem.emission.SetBurst(0, burst);

		_splashSystem.Play();
	}


}
