using UnityEngine;



public class CylinderEquipment : MonoBehaviour
{


	[SerializeField] private Cylinder _target;



	private void Awake()
	{
		CylinderProfile.OnCylinderSelected += _target.Equip;
	}


}
