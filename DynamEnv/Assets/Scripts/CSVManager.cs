using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class  CSVManager : MonoBehaviour 
{

    public void Back()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Back button pressed");
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
        float sex = 1;
        try
        {
            sex = GameObject.Find("sexslider").GetComponent<Slider>().value;
        }
        catch
        {
            sex = 2;
        }
        string gender = "Other";

        if (sex < 0.5) {
            gender = "Male";
        }
        else{
            gender = "Female";
        }
        string age = "4";
        try
        {
            age = (GameObject.Find("ageslider").GetComponent<Slider>().value + 2).ToString();
        }
        catch
        {
            age = "ERROR";
        }
  
        string postDanceHappiness = GameObject.Find("postdanceslider").GetComponent<Slider>().value.ToString();
        //string agestringtwo = GameObject.Find("ageslider").GetComponent<Slider>().value.ToString();
        //string genderstring = GameObject.Find("sexslider").GetComponent<Slider>().value.ToString();

        //string preDanceHappiness = "testprehap";
        string preDanceHappiness = MainMenu.prehappiness.ToString();
        //string postDanceHappiness = "testposthap";

        string scoreOnDance = Random.Range(0, 100).ToString();


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