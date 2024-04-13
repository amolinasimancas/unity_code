using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public Transform player;	
	public Transform[] crates;

	//Name of the save file
	const string filename = "/saveFile.save";


	void Update()
    {
		if (Input.GetKeyDown(KeyCode.F5))
			SaveGame();

		if (Input.GetKeyDown(KeyCode.F6))
			LoadGame();
    }

	void SaveGame()
	{
		//Create a new data container
		SaveFile sf = new SaveFile();

		//Add the position of each crate to the save file
		foreach (Transform crate in crates)
			sf.cratePositions.Add(crate.position);

		//Add the player's position and rotation to the save file
		sf.playerPosition = player.position;
		sf.yawRotation = transform.rotation.y;

		//Create a file (if one doesn't exist) to store the save data
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + filename);
		
		//Write the data container to the actual file on the hard drive
		bf.Serialize(file, sf);
		
		//Close the file
		file.Close();

		//Let everyone know!
		Debug.Log("Game Saved: " + Application.persistentDataPath + filename);
	}

	void LoadGame()
	{
		//Make sure there is a save file
		if (!File.Exists(Application.persistentDataPath + filename))
		{
			Debug.LogError("No save file!");
			return;
		}

		//Open the save file
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
		
		//Read the data container from the save file
		SaveFile sf = (SaveFile)bf.Deserialize(file);

		//Close the file
		file.Close();

		//Apply the crate positions
		for (int i = 0; i < crates.Length; i++)
			crates[i].position = sf.cratePositions[i];

		//Apply the players position
		player.position = sf.playerPosition;

		//Apply the player's rotation
		Vector3 rot = transform.rotation.eulerAngles;
		rot.y = sf.yawRotation;
		player.rotation = Quaternion.Euler(rot);

		//Let everyone know!
		Debug.Log("Game Loaded: " + Application.persistentDataPath + filename);
	}
}
