using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Joint = Windows.Kinect.Joint;
using System;
using System.IO;
using UnityEngine.SceneManagement;


public class BodySourceView : MonoBehaviour
{
    public Material BoneMaterial;
    public GameObject BodySourceManager;

    public List<string> outputList;
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    public GameObject DancerBody;
    private Kinect.Body[] DancerBodyModel = null;

    public bool didMove = false;
    public bool moveCompleted = false;

    public int whichMove = 0;

    //public int firstSwitch, secondSwitch, thirdSwitch, FourthSwitch;
    public int lastFrame = 0;
    public int startFrame =0;
    public int tempDanceScore =0;

    public static int danceScore =100;




    public int[] moveSwitchArray = new int[4] { 10000, 10000, 10000, 10000 };

    public float[,] SpineBaseArray = new float[8000, 3];
    public float[,] SpineMidArray = new float[8000, 3];
    public float[,] SpineShoulderArray = new float[8000, 3];

    public float[,] NeckArray = new float[8000, 3];
    public float[,] HeadArray = new float[8000, 3];

    public float[,] LeftShoulderArray = new float[8000, 3];
    public float[,] LeftElbowArray = new float[8000, 3];
    public float[,] LeftWristArray = new float[8000, 3];
    public float[,] LeftHandArray = new float[8000, 3];
    public float[,] LeftThumbArray = new float[8000, 3];
    public float[,] LeftHandTipArray = new float[8000, 3];

    public float[,] RightShoulderArray = new float[8000, 3];
    public float[,] RightElbowArray = new float[8000, 3];
    public float[,] RightWristArray = new float[8000, 3];
    public float[,] RightHandArray = new float[8000, 3];
    public float[,] RightThumbArray = new float[8000, 3];
    public float[,] RightHandTipArray = new float[8000, 3];


    public float[,] LeftHipArray = new float[8000, 3];
    public float[,] LeftKneeArray = new float[8000, 3];
    public float[,] LeftFootArray = new float[8000, 3];
    public float[,] LeftAnkleArray = new float[8000, 3];

    public float[,] RightHipArray = new float[8000, 3];
    public float[,] RightKneeArray = new float[8000, 3];
    public float[,] RightFootArray = new float[8000, 3];
    public float[,] RightAnkleArray = new float[8000, 3];







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


    private void Start()
    {

        DancerBodyModel = new Kinect.Body[1];
        openPosFile();
        ulong x = 10;
        CreateBodyObjectDancer(x);
    }

    void Update()
    {

        if(Time.frameCount == lastFrame + 200)
        {
            // set our final score and exit the scene
            danceScore = tempDanceScore;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        


        if (Time.frameCount % 100 == 0)
        {
            Debug.Log(Time.frameCount);
            //Debug.Log(moveSwitchArray[2]);
        }


        // When frame matches to dance, check if move was completed
        if (moveSwitchArray[0] == Time.frameCount | moveSwitchArray[1] == Time.frameCount | moveSwitchArray[2] == Time.frameCount | lastFrame == Time.frameCount)
        {
            CheckScore();
        }

        //Body[] bodyData = BodySourceManager.GetData();
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId]);

                startFrame = Time.frameCount;
                RefreshBodyObjectDancer(body, DancerBody);




            }



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
            jointObj.name = jt.ToString();
            jointObj.transform.parent = body.transform;
        }

        return body;
    }

    private GameObject CreateBodyObjectDancer(ulong id)
    {
        DancerBody = new GameObject("Body:" + id);

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.SetVertexCount(2);
            lr.material = BoneMaterial;
            lr.SetWidth(0.05f, 0.05f);

            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.name = jt.ToString();
            jointObj.transform.parent = DancerBody.transform;
        }

        return DancerBody;
    }







    int movearrayint = 0;

    private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {


        outputList.Add(Time.frameCount.ToString());

        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;

            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];

            }

            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            //Transform jointObjDancer = DancerBody.transform.Find(jt.ToString());

            jointObj.localPosition = GetVector3FromJoint(sourceJoint);
            //jointObjDancer.localPosition = GetVector3FromJoint(sourceJoint);


            // write the coordinates to a file

            // get coordinates
            float x = jointObj.localPosition.x;
            float y = jointObj.localPosition.y;
            float z = jointObj.localPosition.z;

            // text to be printed
            string outputline = sourceJoint.JointType.ToString() + ": " + x.ToString() + "," + y.ToString() + "," + z.ToString();
            outputList.Add(outputline);




            // lines between the joints
            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            //LineRenderer lr2 = jointObjDancer.GetComponent<LineRenderer>();



            if (targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.localPosition);


                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));


                lr.SetColors(GetColorForState(sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));

            }
            else
            {
                lr.enabled = false;
            }
        }

        // check if current body position matches anything
        moveMatch(body, bodyObject);


    }

    private void RefreshBodyObjectDancer(Kinect.Body body, GameObject bodyObject)
    {


        for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
        {
            Kinect.Joint sourceJoint = body.Joints[jt];
            Kinect.Joint? targetJoint = null;
            Kinect.JointType? targetJointType = null;



            if (_BoneMap.ContainsKey(jt))
            {
                targetJoint = body.Joints[_BoneMap[jt]];
                targetJointType = _BoneMap[jt];


            }

            Transform jointObjDancer = DancerBody.transform.Find(jt.ToString());



            jointObjDancer.localPosition = GetVector3FromFile(jt.ToString());

            //Debug.Log(jointObjDancer.localPosition);

            LineRenderer lr2 = jointObjDancer.GetComponent<LineRenderer>();


            if (targetJoint.HasValue)
            {
                lr2.SetPosition(0, jointObjDancer.localPosition);
                lr2.SetPosition(1, GetVector3FromFile(targetJointType.ToString()));
                lr2.SetColors(GetColorForStateDancer(sourceJoint.TrackingState), GetColorForStateDancer(targetJoint.Value.TrackingState));
            }
            else
            {
                lr2.enabled = false;
            }

        }
        movearrayint++;

    }

    private static Color GetColorForState(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.green;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }

    private static Color GetColorForStateDancer(Kinect.TrackingState state)
    {
        switch (state)
        {
            case Kinect.TrackingState.Tracked:
                return Color.blue;

            case Kinect.TrackingState.Inferred:
                return Color.red;

            default:
                return Color.black;
        }
    }







    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    private Vector3 GetVector3FromFile(string joint)
    {

        if (joint == "SpineBase")
        {
            return new Vector3(SpineBaseArray[movearrayint, 0], SpineBaseArray[movearrayint, 1], SpineBaseArray[movearrayint, 2]);
        }

        if (joint == "SpineMid")
        {
            return new Vector3(SpineMidArray[movearrayint, 0], SpineMidArray[movearrayint, 1], SpineMidArray[movearrayint, 2]);
        }
        if (joint == "Head")
        {
            return new Vector3(HeadArray[movearrayint, 0], HeadArray[movearrayint, 1], HeadArray[movearrayint, 2]);
        }
        if (joint == "Neck")
        {
            return new Vector3(NeckArray[movearrayint, 0], NeckArray[movearrayint, 1], NeckArray[movearrayint, 2]);
        }
        if (joint == "ShoulderLeft")
        {
            return new Vector3(LeftShoulderArray[movearrayint, 0], LeftShoulderArray[movearrayint, 1], LeftShoulderArray[movearrayint, 2]);
        }
        if (joint == "ShoulderRight")
        {
            return new Vector3(RightShoulderArray[movearrayint, 0], RightShoulderArray[movearrayint, 1], RightShoulderArray[movearrayint, 2]);
        }
        if (joint == "ElbowLeft")
        {
            return new Vector3(LeftElbowArray[movearrayint, 0], LeftElbowArray[movearrayint, 1], LeftElbowArray[movearrayint, 2]);
        }
        if (joint == "ElbowRight")
        {
            return new Vector3(RightElbowArray[movearrayint, 0], RightElbowArray[movearrayint, 1], RightElbowArray[movearrayint, 2]);
        }

        if (joint == "HandRight")
        {
            return new Vector3(RightHandArray[movearrayint, 0], RightHandArray[movearrayint, 1], RightHandArray[movearrayint, 2]);
        }
        if (joint == "HandLeft")
        {
            return new Vector3(LeftHandArray[movearrayint, 0], LeftHandArray[movearrayint, 1], LeftHandArray[movearrayint, 2]);
        }

        if (joint == "WristRight")
        {
            return new Vector3(RightWristArray[movearrayint, 0], RightWristArray[movearrayint, 1], RightWristArray[movearrayint, 2]);
        }
        if (joint == "WristLeft")
        {
            return new Vector3(LeftWristArray[movearrayint, 0], LeftWristArray[movearrayint, 1], LeftWristArray[movearrayint, 2]);
        }






        if (joint == "HipLeft")
        {
            return new Vector3(LeftHipArray[movearrayint, 0], LeftHipArray[movearrayint, 1], LeftHipArray[movearrayint, 2]);
        }
        if (joint == "HipRight")
        {
            return new Vector3(RightHipArray[movearrayint, 0], RightHipArray[movearrayint, 1], RightHipArray[movearrayint, 2]);
        }

        if (joint == "KneeRight")
        {
            return new Vector3(RightKneeArray[movearrayint, 0], RightKneeArray[movearrayint, 1], RightKneeArray[movearrayint, 2]);
        }

        if (joint == "KneeLeft")
        {
            return new Vector3(LeftKneeArray[movearrayint, 0], LeftKneeArray[movearrayint, 1], LeftKneeArray[movearrayint, 2]);
        }


        if (joint == "AnkleRight")
        {
            return new Vector3(RightAnkleArray[movearrayint, 0], RightAnkleArray[movearrayint, 1], RightAnkleArray[movearrayint, 2]);
        }
        if (joint == "AnkleLeft")
        {
            return new Vector3(LeftAnkleArray[movearrayint, 0], LeftAnkleArray[movearrayint, 1], LeftAnkleArray[movearrayint, 2]);
        }
        if (joint == "FootLeft")
        {
            return new Vector3(LeftFootArray[movearrayint, 0], LeftFootArray[movearrayint, 1], LeftFootArray[movearrayint, 2]);
        }
        if (joint == "FootRight")
        {
            return new Vector3(RightFootArray[movearrayint, 0], RightFootArray[movearrayint, 1], RightFootArray[movearrayint, 2]);
        }
        if (joint == "FootRight")
        {
            return new Vector3(RightFootArray[movearrayint, 0], RightFootArray[movearrayint, 1], RightFootArray[movearrayint, 2]);
        }
        if (joint == "HandTipLeft")
        {
            return new Vector3(RightHandTipArray[movearrayint, 0], RightHandTipArray[movearrayint, 1], RightHandTipArray[movearrayint, 2]);
        }
        if (joint == "HandTipRight")
        {
            return new Vector3(RightHandTipArray[movearrayint, 0], RightHandTipArray[movearrayint, 1], RightHandTipArray[movearrayint, 2]);
        }
        if (joint == "ThumbLeft")
        {
            return new Vector3(LeftThumbArray[movearrayint, 0], LeftThumbArray[movearrayint, 1], LeftThumbArray[movearrayint, 2]);
        }
        if (joint == "ThumbRight")
        {
            return new Vector3(RightThumbArray[movearrayint, 0], RightThumbArray[movearrayint, 1], RightThumbArray[movearrayint, 2]);
        }
        if (joint == "SpineShoulder")
        {
            return new Vector3(SpineShoulderArray[movearrayint, 0], SpineShoulderArray[movearrayint, 1], SpineShoulderArray[movearrayint, 2]);
        }





        return new Vector3(10, 10, 10);
    }


    private void PrintOutput()
    {

        //print to the file
        using (System.IO.StreamWriter file =
        new System.IO.StreamWriter(@"C:\Users\Justin\Documents\Coordinates.txt", true))
        {
            foreach (var outputline in outputList)
            {
                file.WriteLine(outputline);
            }
        }
    }

    public void openPosFile()
    {



        //int fNum = Time.frameCount;

        //Read Body Data From File
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Justin\Documents\RoughDance.txt");



        int fNum = 0;
        int nextNum = 0;
        int counter = 0;


        foreach (string line in lines)
        {



            if (line.Contains("*"))
            {
                lastFrame = fNum;
            }

            if (line.Contains("#"))
            {
                Debug.Log(fNum);
                moveSwitchArray[counter] = fNum;
               
                //Debug.Log(moveSwitchArray[fNum].ToString() + " _ " + fNum.ToString());
                counter++;
                //CheckScore();
                //whichMove++;
                continue;
            }

            if (nextNum % 27 == 0)
            {
                fNum++;
            }

            //moveSwitchArray[fNum] = 0;

            var list = line.Split(':');

            // skip the frame count lines
            if (list.GetLength(0) == 1) { continue; }

            string text = list[0];




            // match coordinates to fake body
            var coordList = list[1].Split(',');

            //jointObj.transform.localScale = new Vector3(float.Parse(coordList[0]), float.Parse(coordList[1]), float.Parse(coordList[2]));

            if (text == "SpineBase")
            {

                SpineBaseArray[fNum, 0] = float.Parse(coordList[0]);
                SpineBaseArray[fNum, 1] = float.Parse(coordList[1]);
                SpineBaseArray[fNum, 2] = float.Parse(coordList[2]);


            }

            if (text == "SpineMid")
            {

                SpineMidArray[fNum, 0] = float.Parse(coordList[0]);
                SpineMidArray[fNum, 1] = float.Parse(coordList[1]);
                SpineMidArray[fNum, 2] = float.Parse(coordList[2]);

            }

            if (text == "SpineShoulder")
            {

                SpineShoulderArray[fNum, 0] = float.Parse(coordList[0]);
                SpineShoulderArray[fNum, 1] = float.Parse(coordList[1]);
                SpineShoulderArray[fNum, 2] = float.Parse(coordList[2]);

            }


            if (text == "Head")
            {
                HeadArray[fNum, 0] = float.Parse(coordList[0]);
                HeadArray[fNum, 1] = float.Parse(coordList[1]);
                HeadArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "Neck")
            {
                NeckArray[fNum, 0] = float.Parse(coordList[0]);
                NeckArray[fNum, 1] = float.Parse(coordList[1]);
                NeckArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "ShoulderLeft")
            {
                LeftShoulderArray[fNum, 0] = float.Parse(coordList[0]);
                LeftShoulderArray[fNum, 1] = float.Parse(coordList[1]);
                LeftShoulderArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "ShoulderRight")
            {
                RightShoulderArray[fNum, 0] = float.Parse(coordList[0]);
                RightShoulderArray[fNum, 1] = float.Parse(coordList[1]);
                RightShoulderArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "ElbowLeft")
            {
                LeftElbowArray[fNum, 0] = float.Parse(coordList[0]);
                LeftElbowArray[fNum, 1] = float.Parse(coordList[1]);
                LeftElbowArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "ElbowRight")
            {
                RightElbowArray[fNum, 0] = float.Parse(coordList[0]);
                RightElbowArray[fNum, 1] = float.Parse(coordList[1]);
                RightElbowArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "KneeLeft")
            {
                LeftKneeArray[fNum, 0] = float.Parse(coordList[0]);
                LeftKneeArray[fNum, 1] = float.Parse(coordList[1]);
                LeftKneeArray[fNum, 2] = float.Parse(coordList[2]);

            }
            if (text == "KneeRight")
            {
                RightKneeArray[fNum, 0] = float.Parse(coordList[0]);
                RightKneeArray[fNum, 1] = float.Parse(coordList[1]);
                RightKneeArray[fNum, 2] = float.Parse(coordList[2]);
            }

            if (text == "AnkleRight")
            {
                RightAnkleArray[fNum, 0] = float.Parse(coordList[0]);
                RightAnkleArray[fNum, 1] = float.Parse(coordList[1]);
                RightAnkleArray[fNum, 2] = float.Parse(coordList[2]);
            }

            if (text == "AnkleLeft")
            {
                LeftAnkleArray[fNum, 0] = float.Parse(coordList[0]);
                LeftAnkleArray[fNum, 1] = float.Parse(coordList[1]);
                LeftAnkleArray[fNum, 2] = float.Parse(coordList[2]);
            }

            if (text == "HipLeft")
            {
                LeftHipArray[fNum, 0] = float.Parse(coordList[0]);
                LeftHipArray[fNum, 1] = float.Parse(coordList[1]);
                LeftHipArray[fNum, 2] = float.Parse(coordList[2]);
            }

            if (text == "HipRight")
            {
                RightHipArray[fNum, 0] = float.Parse(coordList[0]);
                RightHipArray[fNum, 1] = float.Parse(coordList[1]);
                RightHipArray[fNum, 2] = float.Parse(coordList[2]);
            }


            if (text == "WristRight")
            {
                RightWristArray[fNum, 0] = float.Parse(coordList[0]);
                RightWristArray[fNum, 1] = float.Parse(coordList[1]);
                RightWristArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "WristLeft")
            {
                LeftWristArray[fNum, 0] = float.Parse(coordList[0]);
                LeftWristArray[fNum, 1] = float.Parse(coordList[1]);
                LeftWristArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "HandLeft")
            {
                LeftHandArray[fNum, 0] = float.Parse(coordList[0]);
                LeftHandArray[fNum, 1] = float.Parse(coordList[1]);
                LeftHandArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "HandRight")
            {
                RightHandArray[fNum, 0] = float.Parse(coordList[0]);
                RightHandArray[fNum, 1] = float.Parse(coordList[1]);
                RightHandArray[fNum, 2] = float.Parse(coordList[2]);
            }

            if (text == "HandTipLeft")
            {
                LeftHandTipArray[fNum, 0] = float.Parse(coordList[0]);
                LeftHandTipArray[fNum, 1] = float.Parse(coordList[1]);
                LeftHandTipArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "HandTipRight")
            {
                RightHandTipArray[fNum, 0] = float.Parse(coordList[0]);
                RightHandTipArray[fNum, 1] = float.Parse(coordList[1]);
                RightHandTipArray[fNum, 2] = float.Parse(coordList[2]);
            }

            if (text == "ThumbRight")
            {
                RightThumbArray[fNum, 0] = float.Parse(coordList[0]);
                RightThumbArray[fNum, 1] = float.Parse(coordList[1]);
                RightThumbArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "ThumbLeft")
            {
                LeftThumbArray[fNum, 0] = float.Parse(coordList[0]);
                LeftThumbArray[fNum, 1] = float.Parse(coordList[1]);
                LeftThumbArray[fNum, 2] = float.Parse(coordList[2]);
            }


            if (text == "FootRight")
            {
                RightFootArray[fNum, 0] = float.Parse(coordList[0]);
                RightFootArray[fNum, 1] = float.Parse(coordList[1]);
                RightFootArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if (text == "FootLeft")
            {
                LeftFootArray[fNum, 0] = float.Parse(coordList[0]);
                LeftFootArray[fNum, 1] = float.Parse(coordList[1]);
                LeftFootArray[fNum, 2] = float.Parse(coordList[2]);
            }





            nextNum++;



        }
        //Console.WriteLine(HeadArray);

    }


    // every ### in the text file, function called seeing if the previous move was done and move onto next move
    private void CheckScore()
    {

        Debug.Log(whichMove);

        string outputline = "This is a test";

        //print to the file

        if (whichMove == 0)
        {
            if (didMove)
            {
                outputline = "Clapping Hands Completed";
            }

            outputline = "Clapping Hands Failed";
        }
        if (whichMove == 1)
        {
            if (didMove)
            {
                outputline = "Swirling Hands Completed";
            }

            outputline = "Swirling Hands Failed";
        }
        if (whichMove == 2)
        {
            if (didMove)
            {
                outputline = "Shimmy Up Completed";
            }

            outputline = "Shimmy Up Failed";
        }
        if (whichMove == 3)
        {
            if (didMove)
            {
                outputline = "Aligator Hands Completed";
            }

            outputline = "Aligator Hands Failed";
        }

        else
        {

            //return;
        }


        using (System.IO.StreamWriter file =
        new System.IO.StreamWriter(@"C:\Users\Justin\Documents\Score.txt", true))
        {
            file.WriteLine(outputline);
        }

        //System.IO.File.WriteAllLines(@"C:\Users\Justin\Documents\Score.txt", outputline


        didMove = false;
        moveCompleted = false;
        whichMove++;


    }

    private void moveMatch(Kinect.Body body, GameObject bodyObject)
    {

        if (whichMove == 0 && moveCompleted == false)
        {
            Debug.Log("got here?");

            Transform jointObj = bodyObject.transform.Find("ShoulderRight");
            Kinect.Joint sourceJoint = body.Joints[Kinect.JointType.ShoulderRight];
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            Transform jointObjCompare = bodyObject.transform.Find("HandRight");
            Kinect.Joint sourceJoint2 = body.Joints[Kinect.JointType.HandRight];
            jointObjCompare.localPosition = GetVector3FromJoint(sourceJoint2);

            float y1 = jointObj.localPosition.y;
            float y2 = jointObjCompare.localPosition.y;

            if (y2 > y1)
            {
                didMove = true;
                tempDanceScore++;
                moveCompleted = true;
            }

        }
        if (whichMove == 1 && moveCompleted == false)
        {
            Transform jointObj = bodyObject.transform.Find("ShoulderRight");
            Kinect.Joint sourceJoint = body.Joints[Kinect.JointType.ShoulderRight];
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            Transform jointObjCompare = bodyObject.transform.Find("HandRight");
            Kinect.Joint sourceJoint2 = body.Joints[Kinect.JointType.HandRight];
            jointObjCompare.localPosition = GetVector3FromJoint(sourceJoint2);

            float y1 = jointObj.localPosition.y;
            float y2 = jointObjCompare.localPosition.y;

            if (y2 > y1)
            {
                didMove = true;
                tempDanceScore++;
                moveCompleted = true;
            }
        }
        if (whichMove == 2 && moveCompleted == false)
        {
            Transform jointObj = bodyObject.transform.Find("HandLeft");
            Kinect.Joint sourceJoint = body.Joints[Kinect.JointType.HandLeft];
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            Transform jointObjCompare = bodyObject.transform.Find("HandRight");
            Kinect.Joint sourceJoint2 = body.Joints[Kinect.JointType.HandRight];
            jointObjCompare.localPosition = GetVector3FromJoint(sourceJoint2);

            float y1 = jointObj.localPosition.y;
            float y2 = jointObjCompare.localPosition.y;

            if (y2 > y1)
            {
                didMove = true;
                tempDanceScore++;
                moveCompleted = true;
            }
        }
        if (whichMove == 3 && moveCompleted == false)
        {

            Transform jointObj3 = bodyObject.transform.Find("HipLeft");
            Kinect.Joint sourceJoint3 = body.Joints[Kinect.JointType.HipLeft];
            jointObj3.localPosition = GetVector3FromJoint(sourceJoint3);


            Transform jointObj = bodyObject.transform.Find("HandLeft");
            Kinect.Joint sourceJoint = body.Joints[Kinect.JointType.HandLeft];
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            Transform jointObjCompare = bodyObject.transform.Find("HandRight");
            Kinect.Joint sourceJoint2 = body.Joints[Kinect.JointType.HandRight];
            jointObjCompare.localPosition = GetVector3FromJoint(sourceJoint2);

            float y1 = jointObj.localPosition.y;
            float y2 = jointObjCompare.localPosition.y;

            float x1 = jointObj.localPosition.x; // Handleft
            float x2 = jointObj3.localPosition.x; // shoulder
            float x3 = jointObjCompare.localPosition.x; //HandRight
            
            if ( y2 - y1 < 0.10 && x1 > x2 && x3>x2) //Rough estimate for now
            {
                didMove = true;
                tempDanceScore++;
                moveCompleted = true;
            }
        }

    }








}
