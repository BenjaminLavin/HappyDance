using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

	public GameObject ConfirmationDialog;
	public GameObject NewCSVDialog;
	public GameObject UpdateCSVDialog;
	public Text NewCSVDialogText;
	public Text UpdateCSVDialogText;


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
		CloseAllDialogs ();
		age = ageSlider.value;
	}

	public void HappinessChanged(){
		CloseAllDialogs ();
		postHappiness = postHappinessSlider.value;
	}


	public void SexSliderChanged(){
		CloseAllDialogs ();
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
		CloseAllDialogs ();
		full = maleButton.GetComponent<Image> ().color;
		faded = femaleButton.GetComponent<Image> ().color;
		femaleText.color = Color.gray;
		femaleButton.GetComponent<Image> ().color = faded;
		NewCSVDialogText.text = GetPath () + "\nApplication will now restart!";
		UpdateCSVDialogText.text = GetPath () + "\nApplication will now restart!";
	}

    public void Back()
    {
		CloseAllDialogs ();
        SceneManager.LoadScene("MainMenu");
    }

	public void YesOverwrite(){
		CloseAllDialogs ();
		CreateNewCSV ();
	}

	public void CloseAllDialogs(){
		ConfirmationDialog.SetActive (false);
		NewCSVDialog.SetActive (false);
		UpdateCSVDialog.SetActive (false);
	}

	private void CreateNewCSV(){
		string filePath = GetPath();

		StreamWriter writer = new StreamWriter(filePath);

		writer.WriteLine("age,sex,scoreOnDance,preDanceHappiness,postDanceHappiness");

		writer.Flush();
		//This closes the file
		writer.Close();

		WriteNewData ();

		NewCSVDialog.SetActive (true);
	}

	public void OkDialog(){
		CloseAllDialogs ();
		Back ();
	}

    public void NewCSV()
    {
		CloseAllDialogs ();
		if (System.IO.File.Exists (GetPath ())) {
			ConfirmationDialog.SetActive (true);
			return;
		} else {
			CreateNewCSV ();
		}

        
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
		CloseAllDialogs ();
		if (System.IO.File.Exists (GetPath ())) {
			WriteNewData ();
		
			UpdateCSVDialog.SetActive (true);

		} else {
			NewCSV ();
		}
    }

    private string GetPath()
    {
        //return Application.dataPath + "/CSV/" + "Dance_Results.csv";
		return "Dance_Results.csv";

    }

}