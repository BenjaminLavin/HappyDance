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
        string filePath = "Default";
        try
        {
            filePath = GetPath();
        }
        catch
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/CSV/");
            filePath = GetPath();
        }


        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine("age,sex,scoreOnDance,preDanceHappiness,postDanceHappiness");

        writer.Flush();
        //This closes the file
        writer.Close();
    }

    public void UpdateCSV ()
    {
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


    public void MassTestCSV()
    {
        try
        {
            NewCSV();
        }
        catch
        {
            Debug.Log("New CSV failed");
        }

        for(int i = 0; i < 500; i++)
        {


            UpdateCSV();
            GameObject.Find("sexdropdown").GetComponent<Dropdown>().value = Random.Range(0, 2);
            int sex = GameObject.Find("sexdropdown").GetComponent<Dropdown>().value;

            GameObject.Find("postdanceslider").GetComponent<Slider>().value = Random.Range(0, 100);
            string postDanceHappiness = GameObject.Find("postdanceslider").GetComponent<Slider>().value.ToString();
            //randomize age
            GameObject.Find("agedropdown").GetComponent<Dropdown>().value = Random.Range(0, 8);
            int age = GameObject.Find("agedropdown").GetComponent<Dropdown>().value + 2;
            MainMenu.prehappiness = Random.Range(0, 100);
            string preDanceHappiness = MainMenu.prehappiness.ToString();

            string scoreOnDance = Random.Range(0, 100).ToString();
            Debug.Log(age + "," + sex.ToString() + "," + scoreOnDance + "," + preDanceHappiness + "," + postDanceHappiness);

        }
    }
}