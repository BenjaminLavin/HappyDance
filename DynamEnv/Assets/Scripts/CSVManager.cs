using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class  CSVManager : MonoBehaviour 
{
	public Text maleText;
	public Text femaleText;
	public Button maleButton;
	public Button femaleButton;

	public Slider sexSlider;
	public Slider ageSlider;
	public Slider postHappinessSlider;


	private string gender = "Male";
	private float age = 2;
	public float postHappiness = 5;


	private Color full;
	private Color faded;

	public void MalePressed(){
		sexSlider.value = 0;
	}

	public void FemalePressed(){
		sexSlider.value = 1;
	}

	public void AgeSliderChanged(){
		age = ageSlider.value;
	}

	public void HappinessChanged(){
		postHappiness = postHappinessSlider.value;
	}


	public void SexSliderChanged(){
		switch ((int)sexSlider.value) {
		case 0:
			maleText.color = Color.white;
			maleButton.GetComponent<Image> ().color = full;
			femaleText.color = Color.gray;
			femaleButton.GetComponent<Image> ().color = faded;
			gender = "Male";
			break;
		case 1:
			maleText.color = Color.gray;
			maleButton.GetComponent<Image> ().color = faded;
			femaleText.color = Color.white;
			femaleButton.GetComponent<Image> ().color = full;
			gender = "Female";
			break;
		default:
			break;
		};
	}

	void Start(){
		full = maleButton.GetComponent<Image> ().color;
		faded = femaleButton.GetComponent<Image> ().color;
		femaleText.color = Color.gray;
		femaleButton.GetComponent<Image> ().color = faded;
	}

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NewCSV()
    {

		if (System.IO.File.Exists (GetPath ())) {
			if (!EditorUtility.DisplayDialog ("FILE ALREADY EXISTS",
				"Creating a new CSV will overwrite the current file. Continue?", "Yes", "No")){
				return;
			}
		}

        string filePath = GetPath();

        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine("age,sex,scoreOnDance,preDanceHappiness,postDanceHappiness");

        writer.Flush();
        //This closes the file
        writer.Close();

		WriteNewData ();

		EditorUtility.DisplayDialog ("New CSV Created",
			GetPath() + "\n\nApplication will now restart!", "Ok");

		Back ();
    }

	private void WriteNewData(){
		string postDanceHappinessString = postHappiness.ToString();
		string preDanceHappiness = MainMenu.prehappiness.ToString();
		string scoreOnDance = BodySourceView.danceScore.ToString();

		string filePath = GetPath();

		using (StreamWriter writer = File.AppendText(filePath))
		{
			//write a new line to the CSV
			writer.WriteLine(age + "," + gender + "," + scoreOnDance + "," + preDanceHappiness + "," + postDanceHappinessString);

			//write ti file
			writer.Flush();

			//This closes the file
			writer.Close();
		}
	}

    public void UpdateCSV ()
	{
		if (System.IO.File.Exists (GetPath ())) {
			WriteNewData ();
		

			EditorUtility.DisplayDialog ("CSV Updated",
				GetPath () + "\n\nApplication will now restart!", "Ok");
		

			Back ();
		} else {
			NewCSV ();
		}
    }

    private string GetPath()
    {
        return Application.dataPath + "/CSV/" + "Dance_Results.csv";
    }

}