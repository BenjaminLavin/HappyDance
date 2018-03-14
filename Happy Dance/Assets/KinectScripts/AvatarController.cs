using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text; 

public class AvatarController : MonoBehaviour
{	
	// Enum used for bones' constraints
	public enum ConstraintAxis { X = 0, Y = 1, Z = 2 }

    //
    int moveArrayint = 0;
    bool dummy = true;

	
	// Bool that determines whether the avatar is active.
	//public bool Active = true;
	
	// Bool that has the characters (facing the player) actions become mirrored. Default true.
	public bool MirroredMovement = true;
	
	// Bool that determines whether the avatar will move or not in space.
	//public bool MovesInSpace = true;
	
	// Bool that determines whether the avatar is allowed to jump -- vertical movement
	// can cause some models to behave strangely, so use at your own discretion.
	public bool VerticalMovement = false;
	
	// Rate at which avatar will move through the scene. The rate multiplies the movement speed (.001f, i.e dividing by 1000, unity's framerate).
	public int MoveRate = 1;
	
	// Slerp smooth factor
	public float SmoothFactor = 3.0f;
	
	
	// Public variables that will get matched to bones. If empty, the kinect will simply not track it.
	// These bones can be set within the Unity interface.
	public Transform Hips; public float[,] HipsArray;

	public Transform Spine; public float[,] SpineArray = new float[1000,3];
    public Transform Neck; public float[,] NeckArray = new float[1000, 3];
    public Transform Head; public float[,] HeadArray = new float[1000,3];

    public Transform LeftShoulder; public float[,] LeftShoulderArray = new float[1000, 3];
    public Transform LeftUpperArm; public float[,] LeftUpperArmArray = new float[1000, 3];
    public Transform LeftElbow; public float[,] LeftElbowArray = new float[1000, 3];
    public Transform LeftWrist; public float[,] LeftWristArray = new float[1000, 3];
    public Transform LeftHand; public float[,] LeftHandArray = new float[1000, 3];
    //public Transform LeftFingers;

    public Transform RightShoulder; public float[,] RightShoulderArray = new float[1000, 3];
    public Transform RightUpperArm; public float[,] RightUpperArmArray = new float[1000, 3];
    public Transform RightElbow; public float[,] RightElbowArray = new float[1000, 3];
    public Transform RightWrist; public float[,] RightWristArray = new float[1000, 3];
    public Transform RightHand; public float[,] RightHandArray = new float[1000, 3];
    //public Transform RightFingers;

    public Transform LeftThigh; public float[,] LeftThighArray = new float[1000, 3];
    public Transform LeftKnee; public float[,] LeftKneeArray = new float[1000, 3];
    public Transform LeftFoot; public float[,] LeftFootArray = new float[1000, 3];
    public Transform LeftToes; public float[,] LeftToesArray = new float[1000, 3];

    public Transform RightThigh; public float[,] RightThighArray = new float[1000, 3];
    public Transform RightKnee; public float[,] RightKneeArray = new float[1000, 3];
    public Transform RightFoot; public float[,] RightFootArray = new float[1000, 3];
    public Transform RightToes; public float[,] RightToesArray = new float[1000, 3];

    public Transform Root;
	
	// A required variable if you want to rotate the model in space.
	public GameObject offsetNode;
	
	// Variable to hold all them bones. It will initialize the same size as initialRotations.
	private Transform[] bones;
	
	// Rotations of the bones when the Kinect tracking starts.
    private Quaternion[] initialRotations;
	
	// Calibration Offset Variables for Character Position.
	private bool OffsetCalibrated = false;
	private float XOffset, YOffset, ZOffset;
	private Quaternion originalRotation;
	
	// GUI Text to display the gesture messages.
	private GameObject GestureInfo;
	// GUI Texture to display the hand cursor
	private GameObject HandCursor;
	
	
    public void Start()
    {	
		GestureInfo = GameObject.Find("GestureInfo");
		HandCursor = GameObject.Find("HandCursor");
		
		// Holds our bones for later.
		bones = new Transform[25];
		
		// Initial rotations of said bones.
		initialRotations = new Quaternion[bones.Length];
		
		// Map bones to the points the Kinect tracks.
		MapBones();

		// Get initial rotations to return to later.
		GetInitialRotations();
		
		// Set the model to the calibration pose.
        RotateToCalibrationPose(0, KinectManager.IsCalibrationNeeded());

        // open the file
        //Head.Rotate(100, 100, 100);
        openPosFile();

    }

    int xcount;

    public void Update()
    {

        if (Time.frameCount % 60 == 0)
        {
            moveFromArray();
        }
    }

    // Update the avatar each frame.
    public void UpdateAvatar(uint UserID, bool IsNearMode)
    {

        


        // every frame mimic the file


        bool flipJoint = !MirroredMovement;
		
		// Update Head, Neck, Spine, and Hips normally.
		TransformBone(UserID, KinectWrapper.SkeletonJoint.HIPS, 1, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.SPINE, 2, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.NECK, 3, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.HEAD, 4, flipJoint);
		
		// Beyond this, switch the arms and legs.
		
		// Left Arm --> Right Arm
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_COLLAR, !MirroredMovement ? 5 : 11, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_SHOULDER, !MirroredMovement ? 6 : 12, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ELBOW, !MirroredMovement ? 7 : 13, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_WRIST, !MirroredMovement ? 8 : 14, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HAND, !MirroredMovement ? 9 : 15, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FINGERTIP, !MirroredMovement ? 10 : 16, flipJoint);
		
		// Right Arm --> Left Arm
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_COLLAR, !MirroredMovement ? 11 : 5, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_SHOULDER, !MirroredMovement ? 12 : 6, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ELBOW, !MirroredMovement ? 13 : 7, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_WRIST, !MirroredMovement ? 14 : 8, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HAND, !MirroredMovement ? 15 : 9, flipJoint);
		TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FINGERTIP, !MirroredMovement ? 16 : 10, flipJoint);
		
		if(!IsNearMode)
		{
			// Left Leg --> Right Leg
			TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HIP, !MirroredMovement ? 17 : 21, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_KNEE, !MirroredMovement ? 18 : 22, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ANKLE, !MirroredMovement ? 19 : 23, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FOOT, !MirroredMovement ? 20 : 24, flipJoint);
			
			// Right Leg --> Left Leg
			TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HIP, !MirroredMovement ? 21 : 17, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_KNEE, !MirroredMovement ? 22 : 18, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ANKLE, !MirroredMovement ? 23 : 19, flipJoint);
			TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FOOT, !MirroredMovement ? 24 : 20, flipJoint);	
		}
		
		// If the avatar is supposed to move in the space, move it.
		//if (MovesInSpace)
		{
			MoveAvatar(UserID);
		}

        

    }
	
	// Calibration pose is simply initial position with hands raised up. Rotation must be 0,0,0 to calibrate.
    public void RotateToCalibrationPose(uint userId, bool needCalibration)
    {	
		// Reset the rest of the model to the original position.
        RotateToInitialPosition();
		
		if(needCalibration)
		{
			if(offsetNode != null)
			{
				// Set the offset's rotation to 0.
				offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
			}
			
			// Right Elbow
			if(RightElbow != null)
	        	RightElbow.rotation = Quaternion.Euler(0, -90, 90) * 
					initialRotations[(int)KinectWrapper.SkeletonJoint.RIGHT_ELBOW];
			
			// Left Elbow
			if(LeftElbow != null)
	        	LeftElbow.rotation = Quaternion.Euler(0, 90, -90) * 
					initialRotations[(int)KinectWrapper.SkeletonJoint.LEFT_ELBOW];

			if(offsetNode != null)
			{
				// Restore the offset's rotation
				offsetNode.transform.rotation = originalRotation;
			}
		}
		
		if(userId != 0)
		{
			// clear gesture info
			KinectManager.Instance.ClearGestures(userId);
			
			if(GestureInfo != null)
				GestureInfo.GetComponent<GUIText>().text = string.Empty;
		}
    }
	
	// Invoked on the successful calibration of a player.
	public void SuccessfulCalibration(uint userId)
	{
		// reset the models position
		if(offsetNode != null)
		{
			offsetNode.transform.rotation = originalRotation;
		}
		
		// re-calibrate the position offset
		OffsetCalibrated = false;
		
		if(GestureInfo != null)
			GestureInfo.GetComponent<GUIText>().text = "SweepLeft, SweepRight, Zoom or Click.";
	}
	
	// Invoked when a gesture is in progress 
	// The gesture must be added first, with KinectManager.Instance.DetectGesture()
	public void GestureInProgress(uint userId, KinectWrapper.Gestures gesture, float progress, 
		KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
	{
		//GestureInfo.guiText.text = string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100));
		if((gesture == KinectWrapper.Gestures.RightHandCursor || gesture == KinectWrapper.Gestures.LeftHandCursor) && progress > 0.5f)
		{
			if(HandCursor != null)
				HandCursor.transform.position = Vector3.Lerp(HandCursor.transform.position, screenPos, 3 * Time.deltaTime);

//			string sGestureText = string.Format("{0} - ({1:F1}, {2:F1} {3:F1})", gesture, screenPos.x, screenPos.y, progress * 100);
//			Debug.Log(sGestureText);
		}
		else if(gesture == KinectWrapper.Gestures.Click && progress > 0.3f)
		{
			string sGestureText = string.Format ("{0} {1:F1}% complete", gesture, progress * 100);
			if(GestureInfo != null)
				GestureInfo.GetComponent<GUIText>().text = sGestureText;
		}		
		else if((gesture == KinectWrapper.Gestures.ZoomOut || gesture == KinectWrapper.Gestures.ZoomIn) && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
			if(GestureInfo != null)
				GestureInfo.GetComponent<GUIText>().text = sGestureText;
		}
		else if(gesture == KinectWrapper.Gestures.Wheel && progress > 0.5f)
		{
			string sGestureText = string.Format ("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
			if(GestureInfo != null)
				GestureInfo.GetComponent<GUIText>().text = sGestureText;
		}
	}
	
	// Invoked when a gesture is complete.
	// Return true, if the gesture must be detected again, false otherwise
	public bool GestureComplete(uint userId, KinectWrapper.Gestures gesture,
		KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
	{
		string sGestureText = gesture + " detected";
		if(gesture == KinectWrapper.Gestures.Click)
			sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);
		
		if(GestureInfo != null)
			GestureInfo.GetComponent<GUIText>().text = sGestureText;
		
		return true;
	}
	
	// Apply the rotations tracked by kinect to the joints.
    void TransformBone(uint userId, KinectWrapper.SkeletonJoint joint, int boneIndex, bool flip)
    {
		Transform boneTransform = bones[boneIndex];
		if(boneTransform == null)
			return;
		
		// Grab the bone we're moving.
		int iJoint = (int)joint;
		if(iJoint < 0)
			return;
		
		// Get Kinect joint orientation
		Quaternion jointRotation = KinectManager.Instance.GetJointOrientation(userId, iJoint, flip);
		if(jointRotation == Quaternion.identity)
			return;
		
		// Apply the new rotation.
        Quaternion newRotation = jointRotation * initialRotations[boneIndex];
		
		//If an offset node is specified, combine the transform with its
		//orientation to essentially make the skeleton relative to the node
		if (offsetNode != null)
		{
			// Grab the total rotation by adding the Euler and offset's Euler.
			Vector3 totalRotation = newRotation.eulerAngles + offsetNode.transform.rotation.eulerAngles;
			// Grab our new rotation.
			newRotation = Quaternion.Euler(totalRotation);
		}
		
		// Smoothly transition to our new rotation.
        boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, Time.deltaTime * SmoothFactor);
	}
	
	// Moves the avatar in 3D space - pulls the tracked position of the spine and applies it to root.
	// Only pulls positional, not rotational.
	void MoveAvatar(uint UserID)
	{
		if(Root == null || Root.parent == null)
			return;
		if(!KinectManager.Instance.IsJointTracked(UserID, (int)KinectWrapper.SkeletonJoint.HIPS))
			return;
		
        // Get the position of the body and store it.
		Vector3 trans = KinectManager.Instance.GetUserPosition(UserID);
		
		// If this is the first time we're moving the avatar, set the offset. Otherwise ignore it.
		if (!OffsetCalibrated)
		{
			OffsetCalibrated = true;
			
			XOffset = !MirroredMovement ? trans.x * MoveRate : -trans.x * MoveRate;
			YOffset = trans.y * MoveRate;
			ZOffset = -trans.z * MoveRate;
		}
	
		float xPos;
		float yPos;
		float zPos;
		
		// If movement is mirrored, reverse it.
		if(!MirroredMovement)
		//	xPos = trans.x * MoveRate - XOffset;
			xPos = 0;
		else
		//	xPos = -trans.x * MoveRate - XOffset;
			xPos = 0;
		
		//yPos = trans.y * MoveRate - YOffset;
		//zPos = -trans.z * MoveRate - ZOffset;
		yPos = YOffset;
		zPos = -0.1f;
		
		// If we are tracking vertical movement, update the y. Otherwise leave it alone.
		Vector3 targetPos = new Vector3(xPos, VerticalMovement ? yPos : 0f, zPos);
		Root.parent.localPosition = Vector3.Lerp(Root.parent.localPosition, targetPos, 3 * Time.deltaTime);
	}
	
	// If the bones to be mapped have been declared, map that bone to the model.
	void MapBones()
	{
		// If they're not empty, pull in the values from Unity and assign them to the array.
		if(Hips != null)
			bones[1] = Hips;
		if(Spine != null)
			bones[2] = Spine;
		if(Neck != null)
			bones[3] = Neck;
		if(Head != null)
			bones[4] = Head;
		
		if(LeftShoulder != null)
			bones[5] = LeftShoulder;
		if(LeftUpperArm != null)
			bones[6] = LeftUpperArm;
		if(LeftElbow != null)
			bones[7] = LeftElbow;
		if(LeftWrist != null)
			bones[8] = LeftWrist;
		if(LeftHand != null)
			bones[9] = LeftHand;
//		if(LeftFingers != null)
//			bones[10] = LeftFingers;
		
		if(RightShoulder != null)
			bones[11] = RightShoulder;
		if(RightUpperArm != null)
			bones[12] = RightUpperArm;
		if(RightElbow != null)
			bones[13] = RightElbow;
		if(RightWrist != null)
			bones[14] = RightWrist;
		if(RightHand != null)
			bones[15] = RightHand;
//		if(RightFingers != null)
//			bones[16] = RightFingers;
		
		if(LeftThigh != null)
			bones[17] = LeftThigh;
		if(LeftKnee != null)
			bones[18] = LeftKnee;
		if(LeftFoot != null)
			bones[19] = LeftFoot;
		if(LeftToes != null)
			bones[20] = LeftToes;
		
		if(RightThigh != null)
			bones[21] = RightThigh;
		if(RightKnee != null)
			bones[22] = RightKnee;
		if(RightFoot != null)
			bones[23] = RightFoot;
		if(RightToes!= null)
			bones[24] = RightToes;
	}
	
	// Capture the initial rotations of the model.
	void GetInitialRotations()
	{
		if(offsetNode != null)
		{
			// Store the original offset's rotation.
			originalRotation = offsetNode.transform.rotation;
			// Set the offset's rotation to 0.
			offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		
		for (int i = 0; i < bones.Length; i++)
		{
			if (bones[i] != null)
			{
				initialRotations[i] = bones[i].rotation;
			}
		}

		if(offsetNode != null)
		{
			// Restore the offset's rotation
			offsetNode.transform.rotation = originalRotation;
		}
	}

	// Set bones to initial position.
    public void RotateToInitialPosition()
    {	
		if(bones == null)
			return;
		
		if(offsetNode != null)
		{
			// Set the offset's rotation to 0.
			offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
		}
		
		// For each bone that was defined, reset to initial position.
		for (int i = 0; i < bones.Length; i++)
		{
			if (bones[i] != null)
			{
				bones[i].rotation = initialRotations[i];
			}
		}

		if(Root != null && Root.parent != null)
		{
			Root.parent.localPosition = Vector3.zero;
		}

		if(offsetNode != null)
		{
			// Restore the offset's rotation
			offsetNode.transform.rotation = originalRotation;
		}
    }

    // move the files from the array
    public void moveFromArray()
    {

        //if (dummy == true) {
        //    foreach (int num in SpineArray)
        //    {
        //        Debug.Log(num);
        //        dummy = false;
        //    }
        //}


        //Root.transform.Translate(SpineArray[moveArrayint, 0], SpineArray[moveArrayint, 1], SpineArray[moveArrayint, 2]);

     Root.position = new Vector3(SpineArray[moveArrayint, 0], SpineArray[moveArrayint, 1], SpineArray[moveArrayint, 2]);
     //Spine.position = new Vector3(SpineArray[moveArrayint, 0], SpineArray[moveArrayint, 1], SpineArray[moveArrayint, 2]);

     //Head.position = new Vector3(HeadArray[moveArrayint, 0], HeadArray[moveArrayint, 1], HeadArray[moveArrayint, 2]);
     //Neck.position = new Vector3(NeckArray[moveArrayint, 0], NeckArray[moveArrayint, 1], NeckArray[moveArrayint, 2]);

        //LeftShoulder.transform.Translate(LeftShoulderArray[moveArrayint, 0], LeftShoulderArray[moveArrayint, 1], LeftShoulderArray[moveArrayint, 2]);
        //RightShoulder.transform.Translate(RightShoulderArray[moveArrayint, 0], RightShoulderArray[moveArrayint, 1], RightShoulderArray[moveArrayint, 2]);

     LeftElbow.position = new Vector3(LeftElbowArray[moveArrayint, 0], LeftElbowArray[moveArrayint, 1], LeftElbowArray[moveArrayint, 2]);
     RightElbow.position = new Vector3(RightElbowArray[moveArrayint, 0], RightElbowArray[moveArrayint, 1], RightElbowArray[moveArrayint, 2]);

     RightHand.position = new Vector3(RightHandArray[moveArrayint, 0], RightHandArray[moveArrayint, 1], RightHandArray[moveArrayint, 2]);
     LeftHand.position = new Vector3(LeftHandArray[moveArrayint, 0], LeftHandArray[moveArrayint, 1], LeftHandArray[moveArrayint, 2]);

     //RightWrist.position = new Vector3(RightWristArray[moveArrayint, 0], RightWristArray[moveArrayint, 1], RightWristArray[moveArrayint, 2]);
     //LeftWrist.position = new Vector3(LeftWristArray[moveArrayint, 0], LeftWristArray[moveArrayint, 1], LeftWristArray[moveArrayint, 2]);

     //LeftFoot.position = new Vector3(LeftFootArray[moveArrayint, 0], LeftFootArray[moveArrayint, 1], LeftFootArray[moveArrayint, 2]);
     //RightFoot.position = new Vector3(RightFootArray[moveArrayint, 0], RightFootArray[moveArrayint, 1], RightFootArray[moveArrayint, 2]);

     //RightKnee.position = new Vector3(RightKneeArray[moveArrayint, 0], RightKneeArray[moveArrayint, 1], RightKneeArray[moveArrayint, 2]);
     //LeftKnee.position = new Vector3(LeftKneeArray[moveArrayint, 0], LeftKneeArray[moveArrayint, 1], LeftKneeArray[moveArrayint, 2]);


        //public Transform LeftShoulder;
        //public Transform LeftUpperArm;
        //public Transform LeftWrist;
        //public Transform LeftWrist;
        //public Transform LeftHand;
        ////public Transform LeftFingers;

        ////public Transform RightShoulder;
        //public Transform RightUpperArm;
        //public Transform RightElbow;
        //public Transform RightWrist;
        //public Transform RightHand;
        ////public Transform RightFingers;

        //public Transform LeftThigh;
        //public Transform LeftKnee;
        //public Transform LeftFoot;
        //public Transform LeftToes;

        //public Transform RightThigh;
        //public Transform RightKnee;
        //public Transform RightFoot;
        //public Transform RightToes;
        moveArrayint++;

    }

    //
    public void openPosFile()
    {

        //int fNum = Time.frameCount;

        //Read Body Data From File
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Justin\Documents\AligatorTest.txt");

        int fNum = 0;
        int nextNum =0;


        foreach (string line in lines)
        {

            if (nextNum % 27 == 0)
            {
                fNum++;
            }

            var list = line.Split(':');

            // skip the frame count lines
            if (list.GetLength(0) == 1) { continue; }

            string text = list[0];

            


            // match coordinates to fake body
            var coordList = list[1].Split(',');

           

            if (text == "SpineBase") {

                SpineArray[fNum, 0] = float.Parse(coordList[0]);
                SpineArray[fNum, 1] = float.Parse(coordList[1]);
                SpineArray[fNum, 2] = float.Parse(coordList[2]);
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
            if( text == "KneeLeft")
            {
                LeftKneeArray[fNum, 0] = float.Parse(coordList[0]);
                LeftKneeArray[fNum, 1] = float.Parse(coordList[1]);
                LeftKneeArray[fNum, 2] = float.Parse(coordList[2]);


                //Debug.Log(LeftKneeArray[fNum, 0]);
                //Debug.Log(LeftKneeArray[fNum, 1]);
                //Debug.Log(LeftKneeArray[fNum, 2]);
            }
            if (text == "KneeRight")
            {
                RightKneeArray[fNum, 0] = float.Parse(coordList[0]);
                RightKneeArray[fNum, 1] = float.Parse(coordList[1]);
                RightKneeArray[fNum, 2] = float.Parse(coordList[2]);
            }
            if( text == "WristRight")
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
       

    }

   //public string whichArray(string text)
   // {


   //     return 
   // }

	
}

