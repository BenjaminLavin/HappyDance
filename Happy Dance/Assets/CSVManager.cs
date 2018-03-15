using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class  CSVManager : MonoBehaviour 
{

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Play button pressed");
    }

    public void NewCSV()
    {
        //Debug.Log(postdanceslider.value);
        string filePath = GetPath();

        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine("age,sex,scoreOnDance,preDanceHappiness,postDanceHappiness");

        writer.Flush();
        //This closes the file
        writer.Close();
    }

    public void UpdateCSV ()
    {
        //int sex = (GetComponent<Dropdown>().value);
        //int sex = 1;

        int sex = GameObject.Find("sexdropdown").GetComponent<Dropdown>().value;

        string gender = "Other";


        if (sex == 0) {
            gender = "Male";
        }
        else if(sex == 1){
            gender = "Female";
        }
        else{
            gender = "Other";
        }
        
        //int age = (GetComponent<Dropdown>().value);
        //int age = 4;
        int age = GameObject.Find("agedropdown").GetComponent<Dropdown>().value + 2;

        string postDanceHappiness = GameObject.Find("postdanceslider").GetComponent<Slider>().value.ToString();

        //string preDanceHappiness = "testprehap";
        string preDanceHappiness = MainMenu.prehappiness.ToString();
        //string postDanceHappiness = "testposthap";

        //YOUR VALUE HERE
        //string scoreOnDance = Random.Range(0, 100).ToString();

        string scoreOnDance = BodySourceView.danceScore.ToString();

        //Info loadedData = DataSaver.loadData<Info>("PreDanceHappiness");

        string filePath = GetPath();

        using (StreamWriter writer = File.AppendText(filePath))
        {
            //write a new line to the CSV
            writer.WriteLine(age + "," + gender + "," + scoreOnDance + "," + preDanceHappiness + "," + postDanceHappiness);

            //write ti file
            writer.Flush();
            
            //This closes the file
            writer.Close();
        }
    }

    private string GetPath()
    {
    #if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Dance_Results.csv";
    #elif UNITY_ANDROID
        return Application.persistentDataPath+"Dance_Results.csv";
    #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Dance_Results.csv";
    #else
        return Application.dataPath +"/"+"Dance_Results.csv";
    #endif
    }

}