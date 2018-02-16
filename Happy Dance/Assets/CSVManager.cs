using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvFile;

public class CSVManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//create a csv
	}
	
	void Submit () {
		//get applicable variables

		//update csv
		using (var csvFile = new CsvFile<Client>("clients.csv"))
		{
    	for (int i = 0 ; i < 1000000 ; i++)
    	{
        	var client = new Client() { id = i, name = "Client " + i };
        	csvFile.Append(user);
    	}
		}    

	}
}
