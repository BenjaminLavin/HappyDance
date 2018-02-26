using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;


// Testing out Nolan CSV command for starting the game
public class NewPlayModeTest {

	[Test]
	public void NewPlayModeTestSimplePasses() {
        MainMenu class1 = new MainMenu();
        class1.PlayGame();
    }


}
