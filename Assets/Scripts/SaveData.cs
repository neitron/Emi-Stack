using System;



[Serializable]
public abstract class SaveData
{ 


	public readonly string KEY_NAME;
	


	public SaveData(string keyName = null)
	{
		KEY_NAME = keyName;
		if (string.IsNullOrEmpty(KEY_NAME))
		{
			KEY_NAME = $"{GetType().Name}";
		}
	}
	

}