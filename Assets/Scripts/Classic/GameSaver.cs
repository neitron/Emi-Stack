using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public class GameSaver
{
	

	public void Save<T>(T data) where T : SaveData, new()
	{
		BinaryFormatter bf = new BinaryFormatter();
		string fullPath = Application.persistentDataPath + $"/{data.KEY_NAME}.save";
		using (FileStream fstream = File.Create(fullPath))
		{
			bf.Serialize(fstream, data);
		}
	}


	public void SaveAsJson(object data, bool isPretty = false)
	{
		string jsonData = JsonUtility.ToJson(data, isPretty);
		
		string fullPath = Application.persistentDataPath + $"/{data.GetType().Name}.json";
		File.WriteAllText(fullPath, jsonData);
	}


	public bool Load<T>(out T data, string keyName = null) where T : SaveData, new()
	{
		data = new T();

		if (string.IsNullOrEmpty(keyName))
		{
			keyName = data.KEY_NAME;
		}

		string fullPath = Application.persistentDataPath + $"/{keyName}.save";
		if (!File.Exists(fullPath))
		{
			Debug.Log($"There isn't a game save: {fullPath}");
			return false;
		}

		BinaryFormatter bf = new BinaryFormatter();
		using (FileStream fstream = File.Open(fullPath, FileMode.Open))
		{
			if (fstream.Length == 0)
			{
				return false;
			}
			data = (T)bf.Deserialize(fstream);
		}

		return true;
	}
	

}