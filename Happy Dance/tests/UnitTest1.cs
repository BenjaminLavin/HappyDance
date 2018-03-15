using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {


        int[] moveSwitchArray = new int[2];
        float[,] SpineBaseArray = new float[3, 3];
        float[,] LeftThumbArray = new float[3, 3];

        // check the file is read and data is stored
        // note no need to check for file errors as we create the file 
        [TestMethod]
        public void openFileTest()
        {

        
               
                // may need to change file location
                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Justin\Documents\DanceTest.txt");
                int lastFrame = 0;
                int fNum = 0;
                int nextNum = 0;
                int counter =0;

            //////////////////////////////////////// Looking for values ////////////
               int expectedLastFrame = 2;
               double[,] expectedSpineBaseArray = new double[2, 3] { { -1.96957, -2.13667, 24.58157 }, { -1.28472, -2.510269, 24.35644 } };
               double[,] expectedLeftThumbArray = new double[2, 3] { { -4.071733, -2.627344, 22.46417 }, { -2.622149, -2.419197, 22.92727 } };
            /////////////////////////////////////////////////////////////////////////




            //Debug.WriteLine(lines.Length);
           
            foreach (string line in lines)
                {

                    
                    // indication the dance is done
                    if (line.Contains("*"))
                    {
                        lastFrame = fNum;
                        continue;
                    }

                    // signaling end of dance move, record the frame
                    if (line.Contains("#"))
                    {
                        moveSwitchArray[counter] = fNum;
                        counter++;
                        continue;
                    }

                    // read all 27 body parts then increment array lsit
                    if (nextNum % 24 == 0 && nextNum != 0)
                    {

                    Debug.WriteLine(fNum);
                    fNum++;
                    }


                    var list = line.Split(':');

                    // skip the frame count lines
                    if (list.GetLength(0) == 1) { continue; }

                    // get the body part
                    string text = list[0];

                    // match coordinates to fake body
                    var coordList = list[1].Split(',');


                    if (text == "SpineBase")
                    {
                        

                        SpineBaseArray[fNum, 0] = float.Parse(coordList[0]);
                        SpineBaseArray[fNum, 1] = float.Parse(coordList[1]);
                        SpineBaseArray[fNum, 2] = float.Parse(coordList[2]);
                    }
                    if (text == "ThumbLeft")
                    {
                    //Debug.WriteLine(fNum);
                        LeftThumbArray[fNum, 0] = float.Parse(coordList[0]);
                        LeftThumbArray[fNum, 1] = float.Parse(coordList[1]);
                        LeftThumbArray[fNum, 2] = float.Parse(coordList[2]);
                    }

                    nextNum++;


                }

                // Check the data
                Assert.AreEqual(expectedLastFrame, lastFrame, 0.001, "Correct Last Frame");

                Assert.AreEqual(expectedLeftThumbArray[1, 0], LeftThumbArray[1, 0],  0.001, "Correct X");
                Assert.AreEqual(expectedLeftThumbArray[1, 1], LeftThumbArray[1, 1], 0.001, "Correct Y");
                Assert.AreEqual(expectedLeftThumbArray[1, 2], LeftThumbArray[1, 2], 0.001, "Correct Z");

                Assert.AreEqual(expectedSpineBaseArray[1, 0], SpineBaseArray[1, 0], 0.001, "Correct X");
                Assert.AreEqual(expectedSpineBaseArray[1, 1], SpineBaseArray[1, 1], 0.001, "Correct Y");
                Assert.AreEqual(expectedSpineBaseArray[1, 2], SpineBaseArray[1, 2], 0.001, "Correct Z");


        }
        }
    }

