using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;




public class Playback : MonoBehaviour {
    public Material BoneMaterial;
    public GameObject BodySourceManager;


    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    private bool createBody = true;


    private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
    {
        { Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
        { Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
        { Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
        { Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },

        { Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
        { Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
        { Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
        { Kinect.JointType.HipRight, Kinect.JointType.SpineBase },

        { Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
        { Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
        { Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
        { Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
        { Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
        { Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
        { Kinect.JointType.HandRight, Kinect.JointType.WristRight },
        { Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
        { Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
        { Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },

        { Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
        { Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
        { Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
        { Kinect.JointType.Neck, Kinect.JointType.Head },
    };



    private List<Kinect.JointType> __joints = new List<Kinect.JointType>
    {
        Kinect.JointType.HandLeft,
        Kinect.JointType.HandRight,
    };

    // Use this for initialization
    void Start() {

     



    }
	
	// Update is called once per frame
	void Update () {

        // init the fake body
        if (createBody)
        {



            GameObject bodyObject = CreateBodyObject(6);
            createBody = false;

        }

	}


    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);




        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)

        {



            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

             jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            //Read Body Data From File
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Justin\Documents\AligatorTest.txt");

           

            foreach (string line in lines)
            {

               

                var list = line.Split(':');

                // matching variable name
                //print(list[0] + jt.ToString());
                string text = list[0] + jt.ToString();

             



                if (list[0] == jt.ToString())
                {

            

                    // match coordinates to fake body
                    var coordList = list[1].Split(',');

                    using (System.IO.StreamWriter file =
   new System.IO.StreamWriter(@"C:\Users\Justin\Documents\Test.txt", true))
                    {
                        file.WriteLine(coordList[0]+" " + coordList[1]);
                    }

                    jointObj.transform.localScale = new Vector3(float.Parse(coordList[0]), float.Parse(coordList[1]), float.Parse(coordList[2]));

                }


            }




            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;

    }


    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.red;

            case Kinect.TrackingState.Inferred:
                return Color.white;

            default:
                return Color.black;
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
