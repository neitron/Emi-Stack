using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(Text))]
public class AppVersionViewUi : MonoBehaviour
{
	

	void Start ()
	{
		GetComponent<Text>().text = $"{Application.version}"; 	
	}
	

}
