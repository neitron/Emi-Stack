using System;



[Serializable]
public abstract class SaveData
{ 


	public readonly string keyName;
	


	public SaveData(string keyName = null)
	{
		this.keyName = keyName;
		if (string.IsNullOrEmpty(this.keyName))
		{
			this.keyName = $"{GetType().Name}";
		}
	}
	

}