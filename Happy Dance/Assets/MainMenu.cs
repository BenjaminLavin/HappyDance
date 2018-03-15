using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

//public static string prehappiness = 5;
//public static string score = 50;

public class MainMenu : MonoBehaviour {

    //Info saveData = new Info();
    //saveData.DanceScore = 99;
    //saveData.prehappiness = 40;
    //static int prehappiness = 50;
    public static float prehappiness = 5;
    public void PlayGame ()
    {
        prehappiness = (GameObject.Find("predancehappiness").GetComponent<Slider>().value);
        //DataSaver.saveData(saveData, "PreDanceHappiness");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Play button pressed");
    }

    public void QuitGame()
    {
        Debug.Log("quit Game");
        Application.Quit();
    }
}


/*
[Serializable]
public class Info
{
    public int DanceScore = 50;
    public float prehappiness = 0;
}

public class DataSaver
{
    //Save Data
    public static void saveData<T>(T dataToSave, string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Convert To Json then to bytes
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        }
        //Debug.Log(path);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    //Load Data
    public static T loadData<T>(string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return default(T);
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return default(T);
        }

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            Debug.Log("Loaded Data from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);

        //Convert to Object
        object resultValue = JsonUtility.FromJson<T>(jsonData);
        return (T)Convert.ChangeType(resultValue, typeof(T));
    }

    public static bool deleteData(string dataFileName)
    {
        bool success = false;

        //Load Data
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return false;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }
}
*/