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

	public DEManager DynamicEnvironment;

	//
	int moveArrayint = 0;
	bool dummy = true;
	bool In = true;
	int rotationcounter = 0;
	int SwirlPart = 0;
	int RightRotation = 0;
	int LeftRotations = 0;
	int part = 0;
	int interval = 25, interval2 = 25, interval3 =25;
	int returnCounter = 0;
	public bool shouldStart = false;

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

	/* SpineBase
     * SpineMid
     * Neck
     * Head
     * ShoulderLT
     * ElbowLt
     * WristLT
     * HandLt
     * ShoulderRt
     * ElbowRt
     * WristRt
     * HandRt
     * HipRt
     * HipLt
     * KneeRt
     * KneeLt
     * AnkleLt
     * FootLt
     * SpineShoulder
     * HandTip
     * Thumb
     */

	public float[,] HipsArrayR = new float[10000, 3];

	public float[,] joint_ShoulderLTArrayR = new float[10000, 3];
	public float[,] SpineArrayR = new float[10000, 3];
	public float[,] NeckArrayR = new float[10000, 3];
	public float[,] HeadArrayR = new float[10000, 3];
	public float[,] PelvisArrayR = new float[10000, 3];

	public float[,] LeftShoulderArrayR = new float[10000, 3];
	public float[,] LeftUpperArmArrayR = new float[10000, 3];
	public float[,] LeftElbowArrayR = new float[10000, 3];
	public float[,] LeftWristArrayR = new float[10000, 3];
	public float[,] LeftHandArrayR = new float[10000, 3];
	//public Transform LeftFingers;

	public float[,] RightShoulderArrayR = new float[10000, 3];
	public float[,] RightUpperArmArrayR = new float[10000, 3];
	public float[,] RightElbowArrayR = new float[10000, 3];
	public float[,] RightWristArrayR = new float[10000, 3];
	public float[,] RightHandArrayR = new float[10000, 3];


	public float[,] LeftThighArrayR = new float[10000, 3];
	public float[,] LeftKneeArrayR = new float[10000, 3];
	public float[,] LeftFootArrayR = new float[10000, 3];
	public float[,] LeftToesArrayR = new float[10000, 3];
	public float[,] RightUpperLegArrayR = new float[10000, 3];

	public float[,] RightThighArrayR = new float[10000, 3];
	public float[,] RightKneeArrayR = new float[10000, 3];
	public float[,] RightFootArrayR = new float[10000, 3];
	public float[,] RightToesArrayR = new float[10000, 3];


	public Transform Hips; public float[,] HipsArray = new float[10000, 3];

	public Transform joint_ShoulderLT; public float[,] joint_ShoulderLTArray = new float[10000, 3];
	public Transform Spine; public float[,] SpineArray = new float[10000, 3];
	public Transform Neck; public float[,] NeckArray = new float[10000, 3];
	public Transform Head; public float[,] HeadArray = new float[10000, 3];
	public Transform Pelvis; public float[,] PelvisArray = new float[10000, 3];

	public Transform LeftShoulder; public float[,] LeftShoulderArray = new float[10000, 3];
	public GameObject Shoulder_Right;

	public Transform originalElbowLt;
	public Transform originalElbowRt;
	public Quaternion rightShoulderOriginal, leftShoulderOriginal;
	public Quaternion rightElbowOriginal, leftElbowOriginal;
	public Quaternion rightwristOriginal, leftwristOriginal;


	public Transform LeftUpperArm; public float[,] LeftUpperArmArray = new float[10000, 3];
	public Transform LeftElbow; public float[,] LeftElbowArray = new float[10000, 3];
	public Transform LeftWrist; public float[,] LeftWristArray = new float[10000, 3];
	public Transform LeftHand; public float[,] LeftHandArray = new float[10000, 3];
	//public Transform LeftFingers;

	public Transform RightShoulder; public float[,] RightShoulderArray = new float[10000, 3];
	public Transform RightUpperArm; public float[,] RightUpperArmArray = new float[10000, 3];
	public Transform RightElbow; public float[,] RightElbowArray = new float[10000, 3];
	public Transform RightWrist; public float[,] RightWristArray = new float[10000, 3];
	public Transform RightHand; public float[,] RightHandArray = new float[10000, 3];
	//public Transform RightFingers;

	public Transform LeftThigh; public float[,] LeftThighArray = new float[10000, 3];
	public Transform LeftKnee; public float[,] LeftKneeArray = new float[10000, 3];
	public Transform LeftFoot; public float[,] LeftFootArray = new float[10000, 3];
	public Transform LeftToes; public float[,] LeftToesArray = new float[10000, 3];
	public Transform RightUpperLeg; public float[,] RightUpperLegArray = new float[10000, 3];

	public Transform RightThigh; public float[,] RightThighArray = new float[10000, 3];
	public Transform RightKnee; public float[,] RightKneeArray = new float[10000, 3];
	public Transform RightFoot; public float[,] RightFootArray = new float[10000, 3];
	public Transform RightToes; public float[,] RightToesArray = new float[10000, 3];

	public Transform Root;

	public Vector3 rightWristEuler, leftWristEuler;
	public Vector3 rightShoulerEuler, leftShoulderEuler;
	public Vector3 rightElbowEuler, leftElbowEuler;
	public Vector3 SpineEuler;

	public bool firstTimeDM2 = true, DM2over;
	public bool firstTimeDM3 = true, DM3over;
	public bool firstTimeDM4 = true, DM4over;

	public int DM3counter = 0;



	// A required variable if you want to rotate the model in space.
	public GameObject offsetNode;
	public GameObject PelvisObject;

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
		//GetInitialRotations();

		// Set the model to the calibration pose.
		//RotateToCalibrationPose(0, KinectManager.IsCalibrationNeeded());




	}

	int xcount;

    bool firstTimeSA = true;

	public void Update()
	{

		if (shouldStart)
		{
			moveFromArray();
		}
        else
        {
            //startingAnimation();
         
        }


	}

	// Update the avatar each frame.
	public void UpdateAvatar(uint UserID, bool IsNearMode)
	{




		//// every frame mimic the file


		//bool flipJoint = !MirroredMovement;

		//// Update Head, Neck, Spine, and Hips normally.
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.HIPS, 1, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.SPINE, 2, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.NECK, 3, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.HEAD, 4, flipJoint);

		//// Beyond this, switch the arms and legs.

		//// Left Arm --> Right Arm
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_COLLAR, !MirroredMovement ? 5 : 11, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_SHOULDER, !MirroredMovement ? 6 : 12, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ELBOW, !MirroredMovement ? 7 : 13, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_WRIST, !MirroredMovement ? 8 : 14, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HAND, !MirroredMovement ? 9 : 15, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FINGERTIP, !MirroredMovement ? 10 : 16, flipJoint);

		//// Right Arm --> Left Arm
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_COLLAR, !MirroredMovement ? 11 : 5, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_SHOULDER, !MirroredMovement ? 12 : 6, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ELBOW, !MirroredMovement ? 13 : 7, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_WRIST, !MirroredMovement ? 14 : 8, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HAND, !MirroredMovement ? 15 : 9, flipJoint);
		//TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FINGERTIP, !MirroredMovement ? 16 : 10, flipJoint);

		//if (!IsNearMode)
		//{
		//    // Left Leg --> Right Leg
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_HIP, !MirroredMovement ? 17 : 21, flipJoint);
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_KNEE, !MirroredMovement ? 18 : 22, flipJoint);
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_ANKLE, !MirroredMovement ? 19 : 23, flipJoint);
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.LEFT_FOOT, !MirroredMovement ? 20 : 24, flipJoint);

		//    // Right Leg --> Left Leg
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_HIP, !MirroredMovement ? 21 : 17, flipJoint);
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_KNEE, !MirroredMovement ? 22 : 18, flipJoint);
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_ANKLE, !MirroredMovement ? 23 : 19, flipJoint);
		//    TransformBone(UserID, KinectWrapper.SkeletonJoint.RIGHT_FOOT, !MirroredMovement ? 24 : 20, flipJoint);
		//}

		//// If the avatar is supposed to move in the space, move it.
		////if (MovesInSpace)
		//{
		//    MoveAvatar(UserID);
		//}



	}

	// Calibration pose is simply initial position with hands raised up. Rotation must be 0,0,0 to calibrate.
//	public void RotateToCalibrationPose(uint userId, bool needCalibration)
//	{
//		// Reset the rest of the model to the original position.
//		RotateToInitialPosition();
//
//		if (needCalibration)
//		{
//			if (offsetNode != null)
//			{
//				// Set the offset's rotation to 0.
//				offsetNode.transform.rotation = Quaternion.Euler(Vector3.zero);
//			}
//
//			// Right Elbow
//			if (RightElbow != null)
//				RightElbow.rotation = Quaternion.Euler(0, -90, 90) *
//					initialRotations[(int)KinectWrapper.SkeletonJoint.RIGHT_ELBOW];
//
//			// Left Elbow
//			if (LeftElbow != null)
//				LeftElbow.rotation = Quaternion.Euler(0, 90, -90) *
//					initialRotations[(int)KinectWrapper.SkeletonJoint.LEFT_ELBOW];
//
//			if (offsetNode != null)
//			{
//				// Restore the offset's rotation
//				offsetNode.transform.rotation = originalRotation;
//			}
//		}
//
//		if (userId != 0)
//		{
//			// clear gesture info
//			KinectManager.Instance.ClearGestures(userId);
//
//			if (GestureInfo != null)
//				GestureInfo.GetComponent<GUIText>().text = string.Empty;
//		}
//	}

	// Invoked on the successful calibration of a player.
	public void SuccessfulCalibration(uint userId)
	{
		// reset the models position
		if (offsetNode != null)
		{
			offsetNode.transform.rotation = originalRotation;
		}

		// re-calibrate the position offset
		OffsetCalibrated = false;

		if (GestureInfo != null)
			GestureInfo.GetComponent<GUIText>().text = "SweepLeft, SweepRight, Zoom or Click.";
	}

	// Invoked when a gesture is in progress 
	// The gesture must be added first, with KinectManager.Instance.DetectGesture()
//	public void GestureInProgress(uint userId, KinectWrapper.Gestures gesture, float progress,
//		KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
//	{
//		//GestureInfo.guiText.text = string.Format("{0} Progress: {1:F1}%", gesture, (progress * 100));
//		if ((gesture == KinectWrapper.Gestures.RightHandCursor || gesture == KinectWrapper.Gestures.LeftHandCursor) && progress > 0.5f)
//		{
//			if (HandCursor != null)
//				HandCursor.transform.position = Vector3.Lerp(HandCursor.transform.position, screenPos, 3 * Time.deltaTime);
//
//			//			string sGestureText = string.Format("{0} - ({1:F1}, {2:F1} {3:F1})", gesture, screenPos.x, screenPos.y, progress * 100);
//			//			Debug.Log(sGestureText);
//		}
//		else if (gesture == KinectWrapper.Gestures.Click && progress > 0.3f)
//		{
//			string sGestureText = string.Format("{0} {1:F1}% complete", gesture, progress * 100);
//			if (GestureInfo != null)
//				GestureInfo.GetComponent<GUIText>().text = sGestureText;
//		}
//		else if ((gesture == KinectWrapper.Gestures.ZoomOut || gesture == KinectWrapper.Gestures.ZoomIn) && progress > 0.5f)
//		{
//			string sGestureText = string.Format("{0} detected, zoom={1:F1}%", gesture, screenPos.z * 100);
//			if (GestureInfo != null)
//				GestureInfo.GetComponent<GUIText>().text = sGestureText;
//		}
//		else if (gesture == KinectWrapper.Gestures.Wheel && progress > 0.5f)
//		{
//			string sGestureText = string.Format("{0} detected, angle={1:F1} deg", gesture, screenPos.z);
//			if (GestureInfo != null)
//				GestureInfo.GetComponent<GUIText>().text = sGestureText;
//		}
//	}
//
//	// Invoked when a gesture is complete.
//	// Return true, if the gesture must be detected again, false otherwise
//	public bool GestureComplete(uint userId, KinectWrapper.Gestures gesture,
//		KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
//	{
//		string sGestureText = gesture + " detected";
//		if (gesture == KinectWrapper.Gestures.Click)
//			sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);
//
//		if (GestureInfo != null)
//			GestureInfo.GetComponent<GUIText>().text = sGestureText;
//
//		return true;
//	}
//
//	// Apply the rotations tracked by kinect to the joints.
//	void TransformBone(uint userId, KinectWrapper.SkeletonJoint joint, int boneIndex, bool flip)
//	{
//		Transform boneTransform = bones[boneIndex];
//		if (boneTransform == null)
//			return;
//
//		// Grab the bone we're moving.
//		int iJoint = (int)joint;
//		if (iJoint < 0)
//			return;
//
//		// Get Kinect joint orientation
//		Quaternion jointRotation = KinectManager.Instance.GetJointOrientation(userId, iJoint, flip);
//		if (jointRotation == Quaternion.identity)
//			return;
//
//		// Apply the new rotation.
//		Quaternion newRotation = jointRotation * initialRotations[boneIndex];
//
//		//If an offset node is specified, combine the transform with its
//		//orientation to essentially make the skeleton relative to the node
//		if (offsetNode != null)
//		{
//			// Grab the total rotation by adding the Euler and offset's Euler.
//			Vector3 totalRotation = newRotation.eulerAngles + offsetNode.transform.rotation.eulerAngles;
//			// Grab our new rotation.
//			newRotation = Quaternion.Euler(totalRotation);
//		}
//
//		// Smoothly transition to our new rotation.
//		boneTransform.rotation = Quaternion.Slerp(boneTransform.rotation, newRotation, Time.deltaTime * SmoothFactor);
//	}
//
//	// Moves the avatar in 3D space - pulls the tracked position of the spine and applies it to root.
//	// Only pulls positional, not rotational.
//	void MoveAvatar(uint UserID)
//	{
//		if (Root == null || Root.parent == null)
//			return;
//		if (!KinectManager.Instance.IsJointTracked(UserID, (int)KinectWrapper.SkeletonJoint.HIPS))
//			return;
//
//		// Get the position of the body and store it.
//		Vector3 trans = KinectManager.Instance.GetUserPosition(UserID);
//
//		// If this is the first time we're moving the avatar, set the offset. Otherwise ignore it.
//		if (!OffsetCalibrated)
//		{
//			OffsetCalibrated = true;
//
//			XOffset = !MirroredMovement ? trans.x * MoveRate : -trans.x * MoveRate;
//			YOffset = trans.y * MoveRate;
//			ZOffset = -trans.z * MoveRate;
//		}
//
//		float xPos;
//		float yPos;
//		float zPos;
//
//		// If movement is mirrored, reverse it.
//		if (!MirroredMovement)
//			//	xPos = trans.x * MoveRate - XOffset;
//			xPos = 0;
//		else
//			//	xPos = -trans.x * MoveRate - XOffset;
//			xPos = 0;
//
//		//yPos = trans.y * MoveRate - YOffset;
//		//zPos = -trans.z * MoveRate - ZOffset;
//		yPos = YOffset;
//		zPos = -0.1f;
//
//		// If we are tracking vertical movement, update the y. Otherwise leave it alone.
//		Vector3 targetPos = new Vector3(xPos, VerticalMovement ? yPos : 0f, zPos);
//		Root.parent.localPosition = Vector3.Lerp(Root.parent.localPosition, targetPos, 3 * Time.deltaTime);
//	}

	// If the bones to be mapped have been declared, map that bone to the model.
	void MapBones()
	{


		if (Pelvis != null)
		{
			bones[1] = Pelvis;
			Debug.Log("Pelvis");
		}
		// If they're not empty, pull in the values from Unity and assign them to the array.
		if (Hips != null)
		{
			bones[1] = Hips;
			Debug.Log("Hips");
		}

		if (Spine != null)
		{
			bones[2] = Spine;
			Debug.Log("Spine");
		}

		if (Neck != null)
		{
			bones[3] = Neck;
			Debug.Log("Neck");
		}

		if (Head != null)
		{
			bones[4] = Head;
			Debug.Log("Head");
		}

		if (RightUpperLeg != null)
		{
			Debug.Log("UpperLeg");
		}

		//Can't Find
		if (LeftShoulder != null)
		{
			bones[5] = LeftShoulder;
			Debug.Log("LeftShoulder");
		}

		if (LeftUpperArm != null)
		{
			bones[6] = LeftUpperArm;
			Debug.Log("LeftUpperArm");
		}
		if (LeftElbow != null)
		{
			bones[7] = LeftElbow;
			Debug.Log("LeftElbow");
		}

		//Can't Find
		if (LeftWrist != null) {
			bones[8] = LeftWrist;
			Debug.Log("LeftWrist");
		}

		if (LeftHand != null)
		{
			bones[9] = LeftHand;
			Debug.Log("LeftHand");
			//		if(LeftFingers != null)
			//			bones[10] = LeftFingers;
		}

		//Can't Find
		if (RightShoulder != null)
		{
			bones[11] = RightShoulder;
			Debug.Log("RightShoulder");
		}
		if (RightUpperArm != null)
		{
			bones[12] = RightUpperArm;
			Debug.Log("RightUpperArm");
		}
		if (RightElbow != null)
		{
			bones[13] = RightElbow;
			Debug.Log("RightElbow");
		}
		if (RightWrist != null)
		{
			bones[14] = RightWrist;
			Debug.Log("RightWrist");
		}
		if (RightHand != null)
		{
			bones[15] = RightHand;
			Debug.Log("RightHand");
			//		if(RightFingers != null)
			//			bones[16] = RightFingers;
		}

		if (LeftThigh != null)
		{
			bones[17] = LeftThigh;
			Debug.Log("LeftThigh");
		}
		if (LeftKnee != null)
		{
			bones[18] = LeftKnee;
			Debug.Log("LeftKnee");
		}
		if (LeftFoot != null)
		{
			bones[19] = LeftFoot;
			Debug.Log("LeftFoot");
		}
		if (LeftToes != null)
		{
			bones[20] = LeftToes;
			Debug.Log("LeftToes");
		}

		if (RightThigh != null)
		{
			bones[21] = RightThigh;
			Debug.Log("RightThigh");
		}
		if (RightKnee != null)
		{
			bones[22] = RightKnee;
			Debug.Log("RightKnee");
		}
		if (RightFoot != null)
		{
			bones[23] = RightFoot;
			Debug.Log("RightFoot");
		}
		if (RightToes != null)
		{
			bones[24] = RightToes;
			Debug.Log("RightToes");
		}

	}

	// Capture the initial rotations of the model.
	void GetInitialRotations()
	{
		if (offsetNode != null)
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

		if (offsetNode != null)
		{
			// Restore the offset's rotation
			offsetNode.transform.rotation = originalRotation;
		}
	}

	// Set bones to initial position.
	public void RotateToInitialPosition()
	{
		if (bones == null)
			return;

		if (offsetNode != null)
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

		if (Root != null && Root.parent != null)
		{
			Root.parent.localPosition = Vector3.zero;
		}

		if (offsetNode != null)
		{
			// Restore the offset's rotation
			offsetNode.transform.rotation = originalRotation;
		}
	}

	// move the files from the array

	bool firstRun = false;
	double originalElbowRtY, originalElbowLtY;
	double originalElbowRtZ, originalElbowLtZ;
	double originalElbowRtX, originalElbowLtX;

	double originalHandRtY, originalHandLtY;
	double originalHandRtZ, originalHandLtZ;
	double originalHandRtX, originalHandLtX;

	double originalKneeRtY, originalKneeLtY;
	double originalKneeRtZ, originalKneeLtZ;
	double originalKneeRtX, originalKneeLtX;

	double originalFootRtY, originalFootLtY;
	double originalFootRtZ, originalFootLtZ;
	double originalFootRtX, originalFootLtX;

	double originalThighRtY, originalThighLtY;
	double originalThighRtZ, originalThighLtZ;
	double originalThighRtX, originalThighLtX;


	double originalWristRtY, originalWristLtY;
	double originalWristRtZ, originalWristLtZ;
	double originalWristRtX, originalWristLtX;

	double rightShoulderOriginalx, rightShoulderOriginaly, rightShoulderOriginalz, rightShoulderOriginalw;

	double originalRootZ, originalRootY, originalRootX;


	public void moveFromArray()
	{


		// get the original avatar
		if (firstRun == false)
		{
			originalElbowLtY = LeftElbow.position.y;
			originalElbowRtY = RightElbow.position.y;
			originalElbowLtZ = LeftElbow.position.z;
			originalElbowRtZ = RightElbow.position.z;

			originalHandLtY = LeftHand.position.y;
			originalHandRtY = RightHand.position.y;
			originalHandLtZ = LeftHand.position.z;
			originalHandRtZ = RightHand.position.z;

			originalKneeLtY = LeftKnee.position.y;
			originalKneeRtY = RightKnee.position.y;
			originalKneeLtZ = LeftKnee.position.z;
			originalKneeRtZ = RightKnee.position.z;

			originalFootLtY = LeftFoot.position.y;
			originalFootRtY = RightFoot.position.y;
			originalFootLtZ = LeftFoot.position.z;
			originalFootRtZ = RightFoot.position.z;

			originalThighLtY = LeftThigh.position.y;
			originalThighRtY = RightThigh.position.y;
			originalThighLtZ = LeftThigh.position.z;
			originalThighRtZ = RightThigh.position.z;


			originalWristLtY = LeftWrist.position.y;
			originalWristRtY = RightWrist.position.y;
			originalWristLtZ = LeftWrist.position.z;
			originalWristRtZ = RightWrist.position.z;


			originalRootZ = Root.position.z;
			originalRootY = Root.position.y;
			originalRootZ = Root.position.z;

			//rightShoulderOriginal = RightShoulder.rotation;
			//rightElbowOriginal = RightElbow.rotation;
			//rightwristOriginal = RightWrist.rotation;


			//rightShoulderOriginalx = RightShoulder.rotation.x;
			//rightShoulderOriginaly = RightShoulder.rotation.y;
			//rightShoulderOriginalz = RightShoulder.rotation.z;
			//rightShoulderOriginalw = RightShoulder.rotation.w;

			rightWristEuler = RightWrist.eulerAngles;
			leftWristEuler = LeftWrist.eulerAngles;

			leftShoulderEuler = LeftShoulder.eulerAngles;
			rightShoulerEuler = RightShoulder.eulerAngles;

			rightElbowEuler = RightElbow.eulerAngles;
			leftElbowEuler = LeftElbow.eulerAngles;
			SpineEuler = Spine.eulerAngles;





			firstRun = true;
		}


		if (dummy == true)
		{
			foreach (int num in SpineArray)
			{
				//Debug.Log(num + "  " + moveArrayint);
				dummy = false;
			}
		}




		if (SpineArray[moveArrayint, 0] == 0)
		{
			//Debug.Log("A" + moveArrayint);
		}


		// Uncheck
		//Root.position = new Vector3(SpineArray[moveArrayint, 0] + (float)originalRootX, SpineArray[moveArrayint, 1] + (float)originalRootY, (float)originalRootZ);


		//Spine.position = new Vector3(SpineArray[moveArrayint, 0], SpineArray[moveArrayint, 1], 20);

		// Ignore Head and Neck
		//Head.position = new Vector3(HeadArray[moveArrayint, 0], HeadArray[moveArrayint, 1],HeadArray[moveArrayint, 2]);
		//Neck.position = new Vector3(NeckArray[moveArrayint, 0], NeckArray[moveArrayint, 1], 20);


		//RightUpperArm.position = new Vector3(RightShoulderArray[moveArrayint, 0], RightShoulderArray[moveArrayint, 1], 20);
		//LeftUpperArm.position = new Vector3(LeftShoulderArray[moveArrayint, 0], LeftShoulderArray[moveArrayint, 1], 20);

		//Hips.position = new Vector3(HipsArray[moveArrayint, 0], HipsArray[moveArrayint, 1], 20);

		//bones[1].position = new Vector3(HipsArray[moveArrayint, 0] +1, HipsArray[moveArrayint, 1]+2, 20);
		//joint_ShoulderLT.position = new Vector3(10, 10, 10);

		// Affects the Shirt

		//LeftShoulder.position = new Vector3(LeftShoulderArray[moveArrayint, 0], LeftShoulderArray[moveArrayint, 1]+7, 20);
		//RightShoulder.position = new Vector3(RightShoulderArray[moveArrayint, 0], RightShoulderArray[moveArrayint, 1]+7, 20);


		//LeftElbow.position = new Vector3(RightElbowArray[moveArrayint, 0] + (float) originalElbowRtX, RightElbowArray[moveArrayint, 1] + (float)originalElbowRtY, (float)originalRootZ);
		//RightElbow.position = new Vector3(LeftElbowArray[moveArrayint, 0]+ (float) originalElbowLtX, LeftElbowArray[moveArrayint, 1] + (float)originalElbowLtY, (float)originalElbowLtZ);

		// 1652 amount of frames in first dance, 1202 amount of frames in second dance




		if (moveArrayint < 1011)
		{
			actDanceMoveOne();
		}

		else if (moveArrayint >= 1011 & !DM2over)
		{
			actDanceMoveTwo();
			actDanceMoveTwo();
		}
		else
		{
			if (DM3counter < 5)
			{
				actDanceMoveThree();
			}

			else if (!DM3over & DM3counter > 4)
			{
				resetDanceMoveThree();
			}
			else
			{
				actDanceMoveFour();
				actDanceMoveFour();
			}

		}


		//430 For third Dance Move
		// For two Iterations run Dance Move 3







		//rotationcounter++;







		// Elbow Y = 90, Z= 270
		//RightElbow.Rotate(0, 90, 270)

		// Uncheck
		//RightFoot.position = new Vector3(RightKneeArray[moveArrayint, 0] + (float)originalFootLtX, (float)originalFootLtY - 1, (float)originalThighLtZ);
		//LeftFoot.position = new Vector3(LeftKneeArray[moveArrayint, 0] + (float)originalFootRtX, (float)originalFootRtY - 1, (float)originalThighLtZ);

		//LeftKnee.position = new Vector3(RightKneeArray[moveArrayint, 0]+ (float)originalKneeLtX,  (float)originalKneeLtY, (float)originalThighLtZ);
		//RightKnee.position = new Vector3(LeftKneeArray[moveArrayint, 0]+ (float)originalKneeRtX, (float)originalKneeRtY, (float)originalThighLtZ);

		// Uncheck
		//RightThigh.position = new Vector3(RightKneeArray[moveArrayint, 0] + (float)originalThighLtX, (float)originalThighLtY - 2, (float)originalThighLtZ);
		//LeftThigh.position = new Vector3(LeftKneeArray[moveArrayint, 0] + (float)originalThighRtX, (float)originalThighRtY - 2, (float)originalThighLtZ);

		moveArrayint++;

	}


	public void printAvatarOriginal()
	{

		String outputline;
		float x, y, z;
		int t = 0;



		for (int i = 0; i <= 24; i++)
		{

			if (bones[i] != null)
			{
				x = bones[i].position.x;
				y = bones[i].position.y;
				z = bones[i].position.z;

				outputline = i.ToString() + " " + bones[i].ToString() + " " + "x=" + x.ToString() + " " + "y=" + y.ToString() + " " + "z=" + z.ToString();

				using (System.IO.StreamWriter file =
					new System.IO.StreamWriter(@"C:\Users\Justin\Documents\Avatar.txt", true))
				{
					file.WriteLine(outputline);
				}
			}
		}

		//foreach (Transform bone in bones)
		//{



		//    if (bone != null)
		//    {

		//        x = bone.position.x;
		//        y = bone.position.y;
		//        z = bone.position.z;

		//        outputline =  t.ToString() + " " + bone.ToString() + " " + "x="  + x.ToString() + " " + "y=" + y.ToString() + " " + "z=" + z.ToString();

		//        using (System.IO.StreamWriter file =
		//         new System.IO.StreamWriter(@"C:\Users\Justin\Documents\Avatar.txt", true))
		//        {
		//            file.WriteLine(outputline);
		//        }
		//        //Debug.Log(bone.position.x.ToString());
		//        t++;
		//    }
		//}


	}





	//
	public void openRotationFile()
	{

		//int fNum = Time.frameCount;

		//Read Body Data From File
		string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Justin\Documents\FullRotations.txt");

		int fNum = 0;
		int nextNum = 0;



		foreach (string line in lines)
		{



			if (line.Contains("#"))
			{
				continue;
			}


			var list = line.Split(':');

			// skip the frame count lines
			if (list.GetLength(0) == 1) { continue; }

			string text = list[0];



			// match coordinates to fake body
			var coordList = list[1].Split(',');

			//jointObj.transform.localScale = new Vector3(float.Parse(coordList[0]), float.Parse(coordList[1]), float.Parse(coordList[2]));

			if (text == "SpineBase")
			{

				SpineArrayR[fNum, 0] = float.Parse(coordList[0]);
				SpineArrayR[fNum, 1] = float.Parse(coordList[1]);
				SpineArrayR[fNum, 2] = float.Parse(coordList[2]);


			}
			if (text == "Head")
			{
				HeadArrayR[fNum, 0] = float.Parse(coordList[0]);
				HeadArrayR[fNum, 1] = float.Parse(coordList[1]);
				HeadArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "Neck")
			{
				NeckArrayR[fNum, 0] = float.Parse(coordList[0]);
				NeckArrayR[fNum, 1] = float.Parse(coordList[1]);
				NeckArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "ShoulderLeft")
			{
				LeftShoulderArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftShoulderArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftShoulderArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "ShoulderRight")
			{
				RightShoulderArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightShoulderArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightShoulderArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			// TODO: merge left and right ip
			if (text == "HipLeft")
			{

				LeftThighArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftThighArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftThighArrayR[fNum, 2] = float.Parse(coordList[2]);
			}

			if (text == "HipRight")
			{

				RightThighArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightThighArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightThighArrayR[fNum, 2] = float.Parse(coordList[2]);
			}


			if (text == "ElbowLeft")
			{
				LeftElbowArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftElbowArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftElbowArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "ElbowRight")
			{
				RightElbowArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightElbowArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightElbowArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "KneeLeft")
			{
				LeftKneeArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftKneeArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftKneeArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "KneeRight")
			{
				RightKneeArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightKneeArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightKneeArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "WristRight")
			{
				RightWristArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightWristArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightWristArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "WristLeft")
			{
				LeftWristArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftWristArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftWristArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "HandLeft")
			{
				LeftHandArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftHandArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftHandArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "HandRight")
			{
				RightHandArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightHandArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightHandArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "FootRight")
			{
				RightFootArrayR[fNum, 0] = float.Parse(coordList[0]);
				RightFootArrayR[fNum, 1] = float.Parse(coordList[1]);
				RightFootArrayR[fNum, 2] = float.Parse(coordList[2]);
			}
			if (text == "FootLeft")
			{
				LeftFootArrayR[fNum, 0] = float.Parse(coordList[0]);
				LeftFootArrayR[fNum, 1] = float.Parse(coordList[1]);
				LeftFootArrayR[fNum, 2] = float.Parse(coordList[2]);
			}


			nextNum++;

			if (nextNum % 26 == 0 && nextNum != 0) {

				fNum++;
			}


		}
		//Console.WriteLine(HeadArray);

	}

	public void openPosFile()
	{

		//int fNum = Time.frameCount;

		//Read Body Data From File
		// Main Dance File RoughDance2.txt
		string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Justin\Documents\Crocodile_hands.txt");

		int fNum = 0;
		int nextNum = 0;



		foreach (string line in lines)
		{



			if (line.Contains("#"))
			{
				continue;
			}


			var list = line.Split(':');

			// skip the frame count lines
			if (list.GetLength(0) == 1) { continue; }

			string text = list[0];




			// match coordinates to fake body
			var coordList = list[1].Split(',');

			//jointObj.transform.localScale = new Vector3(float.Parse(coordList[0]), float.Parse(coordList[1]), float.Parse(coordList[2]));

			if (text == "SpineBase")
			{

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
			// TODO: merge left and right ip
			if (text == "HipLeft")
			{

				LeftThighArray[fNum, 0] = float.Parse(coordList[0]);
				LeftThighArray[fNum, 1] = float.Parse(coordList[1]);
				LeftThighArray[fNum, 2] = float.Parse(coordList[2]);
			}

			if (text == "HipRight")
			{

				RightThighArray[fNum, 0] = float.Parse(coordList[0]);
				RightThighArray[fNum, 1] = float.Parse(coordList[1]);
				RightThighArray[fNum, 2] = float.Parse(coordList[2]);
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

			if (nextNum % 26 == 0 && nextNum != 0)
			{

				fNum++;
			}


		}
		//Console.WriteLine(HeadArray);

	}









	// calculate the euclid distance from the original avatar structure to the kinect mapped point
	// Inward to Outward
	// call for the outward pos

	/* Hand -> Elbow
     * Elbow -> Shoulder
     * Leg -> Knee
     * Knee -> Hips
     */

	public Vector3 scaleDistancesX(Vector3 outwardKinectJointPos, Vector3 inwardKinectJointPos, string Type)

	{
		// first
		Vector3 outwardAvatarJoint, inwardAvatarJoint, newVector3;

		//Vector3 outwardAvatarJointPos, inwardAvatarJointPos;
		double distanceBetweenAvatarJoints, distanceBetweenKinectJoints;

		double newDistance;
		double changeValue;

		// arm scaling Left Hand
		// arm. LT = bones[9], elbow. LT = bones[7]
		if (bones[9].ToString() == Type)
		{


			outwardAvatarJoint = GetVector3(bones[9].ToString()); // hand
			inwardAvatarJoint = GetVector3(bones[7].ToString()); // elbow

			distanceBetweenAvatarJoints = distance2D(outwardAvatarJoint.x, inwardAvatarJoint.x, outwardAvatarJoint.y, inwardAvatarJoint.y);
			distanceBetweenKinectJoints = distance2D(outwardKinectJointPos.x, inwardKinectJointPos.x, outwardKinectJointPos.y, inwardKinectJointPos.y);


			// shrink arm, arm is streched
			if (distanceBetweenKinectJoints > distanceBetweenAvatarJoints)
			{
				changeValue = distanceBetweenKinectJoints - distanceBetweenAvatarJoints;
				newDistance = distanceBetweenKinectJoints - changeValue;
				newVector3 = newClosestVector3(inwardAvatarJoint, outwardAvatarJoint, newDistance);
			}
			else
			{
				newVector3 = outwardKinectJointPos;
			}




		}

		// no clause
		else
		{
			newVector3 = outwardKinectJointPos;
		}

		return newVector3;
	}

	public double distance2D(double x2, double x1, double y2, double y1)
	{
		double distance = 0;

		//xd = x2-x1 yd = y2-y1 
		// Distance = sqrt(xd * xd + yd * yd)

		distance = Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));

		return distance;
	}

	// get the Avatars original Vector 3s
	public Vector3 GetVector3(String Type)
	{
		Vector3 abc = new Vector3();

		// Left Elbow
		if (Type == bones[7].ToString())
		{
			double x = -0.9019861, y = 1.248315, z = 20.01748;

			abc.x = (float)x;
			abc.y = (float)y;
			abc.z = (float)z;


		}
		// Left Hand
		if (Type == bones[9].ToString())
		{
			double x = -0.8280886, y = 1.24729, z = 20.01938;

			abc.x = (float)x;
			abc.y = (float)y;
			abc.z = (float)z;
		}

		// Right Hand
		if (Type == bones[15].ToString())
		{
			double x = -1.171986, y = 1.24870, z = 20.01558;

			abc.x = (float)x;
			abc.y = (float)y;
			abc.z = (float)z;
		}
		// Right Elbow
		if (Type == bones[13].ToString())
		{
			double x = -1.098059, y = 1.24912, z = 20.01531;

			abc.x = (float)x;
			abc.y = (float)y;
			abc.z = (float)z;
		}




		return abc;
	}


	// return the new closest point on the avatar range of motion, if kinect range exceeds that.

	/* For example,
    * Ax, Elbow of Avatar
    * Bx, Hand of Kinect
    * Cx, new Hand of Kinect
    * R, newDistance
    */

	public Vector3 newClosestVector3(Vector3 A, Vector3 B, double r)
	{
		Vector3 returnPoint = new Vector3();

		double Ax = A.x;
		double Bx = B.x;
		double Ay = A.y;
		double By = B.y;


		returnPoint.x = (float)(Ax + r * (Bx - Ax) / (Math.Sqrt(Math.Pow((Bx - Ax), 2) + Math.Pow((By - Ay), 2))));
		returnPoint.y = (float)(Ay + r * (By - Ay) / (Math.Sqrt(Math.Pow((Bx - Ax), 2) + Math.Pow((By - Ay), 2))));
		returnPoint.z = 20;


		return returnPoint;
	}


	public void actDanceMoveOne()
	{
		if (moveArrayint == 1)
		{
			interval = 10;
			RightShoulder.Rotate(0, 110, 0); // move shoulder forward
			RightElbow.Rotate(25, 0, -90); // Elbow up
			RightWrist.Rotate(-90, 0, 0); // Wrist Inwards

			LeftShoulder.Rotate(0, 110, 0); // move shoulder forward
			LeftElbow.Rotate(25, 0, -90); // Elbow up
			LeftWrist.Rotate(-90, 0, 0); // Wrist Inwards
		}
		if(moveArrayint > 990 & In == true)
		{

			Debug.Log("Entering Return Loop");

			if (returnCounter < 10)
			{
				RightWrist.Rotate(90/10, 0, 0); // Wrist Inwards
				RightElbow.Rotate(-25/10, 0, 90/10); // Elbow up
				RightShoulder.Rotate(0, -110/10 , 0); // move shoulder forward



				LeftShoulder.Rotate(0, -110/10 , 0); // move shoulder forward
				LeftElbow.Rotate(-25/10 , 0, 90/10 ); // Elbow up
				LeftWrist.Rotate(90/10, 0, -0); // Wrist Inwards
				returnCounter++;

			}
			if(returnCounter == 10)
			{
				//Debug.Log("Why");

				RightWrist.eulerAngles = rightWristEuler;
				RightShoulder.eulerAngles = rightWristEuler;
				RightElbow.eulerAngles = rightElbowEuler;

				LeftWrist.eulerAngles = leftWristEuler;
				LeftShoulder.eulerAngles = leftWristEuler;
				LeftElbow.eulerAngles = leftElbowEuler;




				returnCounter++;
			}
			return;
		}


		if (In)
		{
			//Debug.Log("In");
			RightShoulder.Rotate(0, -90 / interval, 0);
			LeftShoulder.Rotate(0, -90 / interval, 0);

		}
		else
		{
			//Debug.Log("OUt");
			RightShoulder.Rotate(0, 90 / interval, 0);
			LeftShoulder.Rotate(0, 90 / interval, 0);
		}

		if (rotationcounter == interval)
		{
			if (In) {
				Debug.Log ("Clap");
			} else {
				Debug.Log ("Arms out");
				DynamicEnvironment.DidMove (DanceMove.Clap);

			}
			In = !In;
			rotationcounter = 0;
		}

		//
		//Root.transform.Translate(SpineArray[moveArrayint, 0], SpineArray[moveArrayint, 1], SpineArray[moveArrayint, 2]);

		//// Uncheck
		//RightFoot.position = new Vector3(RightKneeArray[moveArrayint, 0] + (float)originalFootLtX, (float)originalFootLtY - 1, (float)originalThighLtZ);
		//LeftFoot.position = new Vector3(LeftKneeArray[moveArrayint, 0] + (float)originalFootRtX, (float)originalFootRtY - 1, (float)originalThighLtZ);

		//// Uncheck
		//RightThigh.position = new Vector3(RightKneeArray[moveArrayint, 0] + (float)originalThighLtX, (float)originalThighLtY - 2, (float)originalThighLtZ);
		//LeftThigh.position = new Vector3(LeftKneeArray[moveArrayint, 0] + (float)originalThighRtX, (float)originalThighRtY - 2, (float)originalThighLtZ);
		rotationcounter++;
	}


	int DM2counter = 0;

	public void actDanceMoveTwo()
	{
		// (y) R elbow (-), L elbow (+) in and out
		// (z) up and down
		interval = 15 ;

		if (firstTimeDM2)
		{
			Debug.Log("Dance Move two");

			returnCounter = 0; // Reset for next exit loop
			rotationcounter = 0; // Reset for next Dance

			if (DM2counter < 10)
			{
				Debug.Log("DM2 Counter");

				RightShoulder.Rotate(0, 8f, 0); // move shoulder forward
				RightElbow.Rotate(0, 5.5f, -1.5f); // Elbow up
				RightWrist.Rotate(-90 / 10, 0, 0); // Wrist Inwards

				LeftShoulder.Rotate(0, 8f, 0); // move shoulder forward
				LeftElbow.Rotate(0, 9.5f, 6); // Elbow up
				LeftWrist.Rotate(-9f, 0, 0); // Wrist Inwards
				DM2counter++;
			}
			else
			{
				firstTimeDM2 = false;
			}
			return;
		}


		// Reset the position
		if (moveArrayint > (1011 + 1652) & part == 0)
		{

			Debug.Log("Entering Return Loop");

			if (returnCounter < 10)
			{

				RightShoulder.Rotate(0, -75 / 10, 0); // move shoulder forward
				RightElbow.Rotate(0, -75 / 10, 25 / 10); // Elbow up
				RightWrist.Rotate(90 / 10, 0, 0); // Wrist Inwards

				LeftShoulder.Rotate(0, -75 / 10, 0); // move shoulder forward
				LeftElbow.Rotate(0, -105 / 10, -25 / 10); // Elbow up
				LeftWrist.Rotate(90 / 10, 0, 0); // Wrist Inwards

				returnCounter++;

			}
			if (returnCounter == 10)
			{
				Debug.Log("Dance2Over");

				RightWrist.eulerAngles = rightWristEuler;
				RightShoulder.eulerAngles = rightShoulerEuler;
				RightElbow.eulerAngles = rightElbowEuler;

				LeftWrist.eulerAngles = leftWristEuler;
				LeftShoulder.eulerAngles = leftShoulderEuler;
				LeftElbow.eulerAngles = leftElbowEuler;
				DM2over = true;

				returnCounter++;
			}
			return;
		}


		// Do sideways
		if (part >= 2 & part < 4)
		{
			// Move elbow up
			if (part == 2)
			{

				if (SwirlPart < 2)
				{
					RightElbow.Rotate(0, 40 / interval, -35 / interval);
				}
				else
				{
					RightShoulder.Rotate(0, -45 / (interval * 2), 0);
					LeftShoulder.Rotate(0, 45 / (interval * 2), 0);
				}
			}
			// shoulder sway
			if (part == 3)
			{


				if (SwirlPart < 2)
				{
					RightShoulder.Rotate(0, 45 / (interval * 2), 0);
					LeftShoulder.Rotate(0, -45 / (interval * 2), 0);
				}

				else
				{
					RightElbow.Rotate(0, -40 / interval, 35 / interval);
				}

			}


		}

		// Do other sideways
		// parts 6 & 7
		if (part > 5 & part < 8)
		{
			Debug.Log("? got Here");

			if (part == 6)
			{
				if (SwirlPart < 2)
				{
					RightElbow.Rotate(-1, 1, 40 / 40);
					LeftElbow.Rotate(2, 0, -4);
				}
				else
				{
					RightShoulder.Rotate(0, 45 / (interval * 2), 0);
					LeftShoulder.Rotate(0, -45 / (interval * 2), 0);
				}

			}
			if (part == 7)
			{
				if (SwirlPart < 2)
				{
					RightShoulder.Rotate(0, -45 / (interval * 2), 0);
					LeftShoulder.Rotate(0, 45 / (interval * 2), 0);
				}
				else
				{
					RightElbow.Rotate(1, -1, -40 / 40);
					LeftElbow.Rotate(-2, 0, 4);
				}
			}

		}

		// backwards swirl
		// parts 4 & 5
		if (part > 3 & part < 6)
		{

			//Left Arm out, Right arm in
			if (SwirlPart == 0)
			{
				RightElbow.Rotate(0, 45 / interval, 0);
				LeftElbow.Rotate(0, -45 / interval, 0);
			}

			// Left Arm Up, Right Arm Down
			if (SwirlPart == 1)
			{
				RightElbow.Rotate(0, 0, 45 / interval);
				LeftElbow.Rotate(0, 0, -45 / interval);
			}

			// Left Arm In, Right Arm Out
			if (SwirlPart == 2)
			{
				RightElbow.Rotate(0, -45 / interval, 0);
				LeftElbow.Rotate(0, 45 / interval, 0);
			}

			// Left Arm Down, right Arm up
			if (SwirlPart == 3)
			{
				RightElbow.Rotate(0, 0, -45 / interval);
				LeftElbow.Rotate(0, 0, 45 / interval);
			}


		}





		// Left Arm starts Down and in
		if (part < 2)
		{
			//Left Arm up, Right Arm Down
			if (SwirlPart == 0)
			{

				RightElbow.Rotate(0, 0, 45 / interval);
				LeftElbow.Rotate(0, 0, -45 / interval);
			}

			// Left Arm out, Right Arm In
			if (SwirlPart == 1)
			{
				RightElbow.Rotate(0, 45 / interval, 0);
				LeftElbow.Rotate(0, -45 / interval, 0);
			}

			// Left Arm Down, Right Arm Up
			if (SwirlPart == 2)
			{
				RightElbow.Rotate(0, 0, -45 / interval);
				LeftElbow.Rotate(0, 0, 45 / interval);
			}

			// Left Arm in, right Arm out
			if (SwirlPart == 3)
			{
				RightElbow.Rotate(0, -45 / interval, 0);
				LeftElbow.Rotate(0, 45 / interval, 0);
			}




		}


		if (rotationcounter % interval == 0 && rotationcounter != 0)
		{
			rotationcounter = 0;
			SwirlPart++;

			if (SwirlPart % 4 == 0)
			{
				part++;
				part = part % 8;
			}

			SwirlPart = SwirlPart % 4;


		}
		rotationcounter++;

	}
	public void actDanceMoveThree()
	{


		if (firstTimeDM3)
		{
			interval = 15;
			returnCounter = 0;

			RightShoulder.Rotate(-20, 90, -90);
			LeftShoulder.Rotate(-20, 90, -90);
			firstTimeDM3 = false;


		}
		// Dip
		if (part == 0)
		{
			Spine.Rotate(0, 30 / interval, 0);
			Hips.Rotate(0, 0, 10 / interval);
		}
		// Dip Back
		if (part == 1)
		{
			Spine.Rotate(0, -30 / interval, 0);
			Hips.Rotate(0, 0, -10 / interval);
		}
		if (part == 2 || part == 3)
		{
			Spine.Rotate(0, 40 / interval, 0);
			Hips.Rotate(0, 0, 10 / interval);
		}
		if (part == 4 || part == 5)
		{
			Spine.Rotate(0, -65 / (interval), -55 / interval2);

		}
		if (part == 6 || part == 7)
		{
			Spine.Rotate(0, 65 / (interval), 55 / interval2);
		}

		if (part == 8 || part == 9)
		{
			Spine.Rotate(0, -40 / interval, 0);
			Hips.Rotate(0, 0, -10 / interval);
		}


		if (rotationcounter % interval == 0 & rotationcounter != 0)
		{
			Debug.Log(part + ":" + interval);
			part++;

			part = part % 10;
			if (part == 0)
			{
				interval = -interval;
				DM3counter++;

			}
		}
		rotationcounter++;
	}



	public void actDanceMoveFour()
	{
		if (firstTimeDM4)
		{
			Spine.Rotate(55, 0, 0);
			RightShoulder.Rotate(0, 110, 0);
			LeftShoulder.Rotate(0, 100, 20);
			firstTimeDM4 = false;
		}

		interval = 10;

		// Triple Clap
		if (part < 12)
		{

			if (In)
			{
				RightShoulder.Rotate(0, 0, (float)-2.0);
				LeftShoulder.Rotate(0, 0, (float)1);
			}
			else
			{
				RightShoulder.Rotate(0, 0, (float)2.0);
				LeftShoulder.Rotate(0, 0, (float)-1);
			}
		}

		if (part == 12 || part == 13)

		{
			Spine.Rotate((float)-55 / 20, 0, 0);
			//RightShoulder.Rotate(0, (float) -110/20, 0);
			LeftShoulder.Rotate(0, (float)-100 / 20, 0);

		}
		if (part == 14 || part == 15)
		{
			Spine.Rotate((float)-55 / 20, 0, 0);
			//RightShoulder.Rotate(0, 0, 0);
			LeftShoulder.Rotate(0, 5, 0);
		}

		if (part > 15 & part < 28)
		{
			if (In)
			{
				RightShoulder.Rotate(0, 0, (float)-2.0);
				LeftShoulder.Rotate(0, 0, (float)1);
			}
			else
			{
				RightShoulder.Rotate(0, 0, (float)2.0);
				LeftShoulder.Rotate(0, 0, (float)-1);
			}
		}
		if (part == 28 || part == 29)
		{


			Spine.Rotate((float) 55.50 / 10, 0, 0);


		}

		if (rotationcounter % interval == 0 & rotationcounter != 0)
		{
			part++;
			if (part % 2 == 0)
			{
				In = !In;
			}
			part = part % 30;
		}
		rotationcounter++;
	}

	public void resetDanceMoveThree()
	{
		if (returnCounter < 10)
		{
			RightShoulder.Rotate(20 / 10, 90 / 10, 90 / 10);
			LeftShoulder.Rotate(20 / 10, 90 / 10, 90 / 10);
			returnCounter++;
		}
		else
		{
			RightShoulder.eulerAngles = rightShoulerEuler;
			LeftShoulder.eulerAngles = leftShoulderEuler;
			Spine.eulerAngles = SpineEuler;
			DM3over = true;
		}


	}

    int saLeft =0, saRight=0;

    // Make sure the cart is doing something at the start
    public void startingAnimation()
    {


        if (firstTimeSA)
        {

            RightShoulder.Rotate(0, 0, 65);
            LeftShoulder.Rotate(0, 0, 65);
            firstTimeSA = false;
        }

        // turns clockwise
        if (saLeft < 12)
        {
            Debug.Log("1");
            Root.Rotate(0, 0, (float) 1/2);
            saLeft++;
        }

        // turns counter clockwise
        else if (saLeft>=12 & saRight < 24)
        {
            Debug.Log("2");
            Root.Rotate(0, 0, (float) -1/2);
            saRight++;
        }

        else if(saLeft>=12 & saRight >= 23)
        {
            Debug.Log("3");
            Root.Rotate(0, 0, (float)1 / 2);
            saLeft++;
        }

        else if ( saLeft >=24 & saRight >= 23)
        {
            Debug.Log("4");
            saRight = 0;
            saLeft = 0;
        }

       
     

    }


}
