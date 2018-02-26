using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Microsoft.VisualBasic.FileIO;


// Testing out Nolan CSV command for starting the game
public class NewPlayModeTest {

	var path = @"C:\YourFile.csv";
	
	[Test]
	public void NewPlayModeTestSimplePasses() {
        MainMenu dummyClass = new MainMenu();
        dummyClass.PlayGame(); // create csv file
		
	// test to see if the file exists
	using (var parser = new TextFieldParser(path))
	{
    	parser.TextFieldType = FieldType.Delimited;
  	parser.SetDelimiters(",");

    	string[] line;
   	while (!parser.EndOfData)
    	{
        	try {line = parser.ReadFields();}
        catch (MalformedLineException ex)
        {
            // log ex.Message
        }
    }
}
    }


}
