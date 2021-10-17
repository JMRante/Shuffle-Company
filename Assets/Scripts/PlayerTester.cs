using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PlayerTester : MonoBehaviour
{
    // public bool isTestModeOn = false;
    // public bool isTestRecordModeOn = false;
    // public string testRecordPath = "X:\\Base\\Game Projects\\ShuffleCompanyMeta\\Tests\\";
    // public string testRecordFile = "SCTest1.csv";

    // private Dictionary<int, Vector3> testInstructions;
    // private PlayerMove playerMover;
    // private StringBuilder testRecord;

    // private Vector3 lastLatestInputDirection;

    // void Start()
    // {
    //     testInstructions = new Dictionary<int, Vector3>();
    //     playerMover = GetComponent<PlayerMove>();
    //     lastLatestInputDirection = playerMover.LatestInputDirection;

    //     if (isTestRecordModeOn)
    //     {
    //         testRecord = new StringBuilder();
    //     }
    //     else if (isTestModeOn)
    //     {
    //         try
    //         {
    //             using (StreamReader sr = new StreamReader(testRecordPath + testRecordFile))
    //             {
    //                 string line;

    //                 while ((line = sr.ReadLine()) != null)
    //                 {
    //                     string[] lineParts = line.Split(',');
                        
    //                     if (lineParts.Length == 2)
    //                     {
    //                         Vector3 direction;

    //                         switch (Int32.Parse(lineParts[1]))
    //                         {
    //                             case 1: direction = Vector3.forward; break;
    //                             case 2: direction = Vector3.right; break;
    //                             case 3: direction = Vector3.back; break;
    //                             case 4: direction = Vector3.left; break;
    //                             default: direction = Vector3.zero; break;
    //                         }

    //                         int fixedUpdateFrame = Int32.Parse(lineParts[0]);
    //                         testInstructions.Add(fixedUpdateFrame, direction);
    //                     }
    //                 }
    //             }
    //         }
    //         catch (Exception e)
    //         {
    //             Debug.LogError("Failed to load test file " + testRecordPath + testRecordFile + ": " + e.Message);
    //         }
    //     }
    // }

    // void FixedUpdate()
    // {
    //     if (isTestRecordModeOn)
    //     {
    //         int fixedUpdateFrame = Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);

    //         if (playerMover.LatestInputDirection != lastLatestInputDirection)
    //         {
    //             int directionCode = 0;

    //             if (playerMover.LatestInputDirection == Vector3.forward)
    //                 directionCode = 1;
    //             else if (playerMover.LatestInputDirection == Vector3.right)
    //                 directionCode = 2;
    //             else if (playerMover.LatestInputDirection == Vector3.back)
    //                 directionCode = 3;
    //             else if (playerMover.LatestInputDirection == Vector3.left)
    //                 directionCode = 4;
    //             else if (playerMover.LatestInputDirection == Vector3.zero)
    //                 directionCode = 0;

    //             testRecord.AppendLine(fixedUpdateFrame + "," + directionCode);
    //         }
    //     }
    //     else if (isTestModeOn)
    //     {
    //         int fixedUpdateFrame = Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);

    //         if (testInstructions.ContainsKey(fixedUpdateFrame))
    //         {
    //             Vector3 direction = testInstructions[fixedUpdateFrame];
    //             playerMover.SetInput(direction);
    //         }
    //     }
    // }

    // void OnApplicationQuit()
    // {
    //     if (isTestRecordModeOn)
    //     {
    //         StreamWriter testRecordOutStream = new StreamWriter(testRecordPath + testRecordFile);
    //         testRecordOutStream.Write(testRecord.ToString());
    //         testRecordOutStream.Close();
    //     }
    // }
}
