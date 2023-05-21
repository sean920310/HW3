using System;
using UnityEngine;
using System.IO;

public class ReplayManager : MonoBehaviour
{
    public Transform[] transforms;

    public bool recording { get; private set; } = false;
    public bool replaying { get; private set; } = false;

    public Action OnStartedRecording;
    public Action OnStoppedRecording;
    public Action OnStartedReplaying;
    public Action OnStoppedReplaying;

    private MemoryStream memoryStream = null;
    private BinaryWriter binaryWriter = null;
    private BinaryReader binaryReader = null;

    private bool recordingInitialized;

    private int currentRecordingFrames = 0;
    public int maxRecordingFrames = 360;

    public void Start()
    {
        //transforms = FindObjectsOfType<Transform>();
        //print(transforms.Length);
    }
    public void FixedUpdate()
    {
        if (recording)
        {
            UpdateRecording();
        }
        else if (replaying)
        {
            UpdateReplaying();
        }
    }

    public void StartStopRecording()
    {
        if (!recording)
        {
            StartRecording();
        }
        else
        {
            StopRecording();
        }
    }

    private void ResetReplayFrame()
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        binaryWriter.Seek(0, SeekOrigin.Begin);
    }
    private void StartRecording()
    {
        if (!recordingInitialized)
        {
            InitializeRecording();
        }
        else
        {
            memoryStream.SetLength(0);
        }

        recording = true;
        ResetReplayFrame();
        if (OnStartedRecording != null)
        {
            OnStartedRecording();
        }
    }

    private void InitializeRecording()
    {
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
        binaryReader = new BinaryReader(memoryStream);
        recordingInitialized = true;
    }

    private void UpdateRecording()
    {
        if (maxRecordingFrames != 0 && currentRecordingFrames > maxRecordingFrames)
        {
            StopRecording();
            currentRecordingFrames = 0;
            return;
        }

        SaveTransforms(transforms);
        ++currentRecordingFrames;
    }

    private void StopRecording()
    {
        recording = false;
        if (OnStoppedRecording != null)
        {
            OnStoppedRecording();
        }
    }

    public void StartStopReplaying()
    {
        if (!replaying)
        {
            StartReplaying();
        }
        else
        {
            StopReplaying();
        }
    }

    private void StartReplaying()
    {
        ResetReplayFrame();
        replaying = true;
        if (OnStartedReplaying != null)
        {
            OnStartedReplaying();
        }
    }

    private void UpdateReplaying()
    {
        if (memoryStream.Position >= memoryStream.Length)
        {
            StopReplaying();
            return;
        }
        LoadTransforms(transforms);
    }

    private void StopReplaying()
    {
        replaying = false;
        if (OnStoppedReplaying != null)
        {
            OnStoppedReplaying();
        }
    }
    private void SaveTransform(Transform transform)
    {
        binaryWriter.Write(transform.localPosition.x);
        binaryWriter.Write(transform.localPosition.y);
        binaryWriter.Write(transform.localPosition.z);

        if (transform.tag == "Ball")
        {
            binaryWriter.Write(transform.rotation.x);
            binaryWriter.Write(transform.rotation.y);
            binaryWriter.Write(transform.rotation.z);
        }

        binaryWriter.Write(transform.localScale.x);
        binaryWriter.Write(transform.localScale.y);
        binaryWriter.Write(transform.localScale.z);

        if(transform.GetComponent<Animator>())
            binaryWriter.Write(transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash);
    }
    private void SaveTransforms(Transform[] transforms)
    {
        foreach (Transform transform in transforms)
        {
            SaveTransform(transform);
        }
    }
    private void LoadTransform(Transform transform)
    {
        float x = binaryReader.ReadSingle();
        float y = binaryReader.ReadSingle();
        float z = binaryReader.ReadSingle();
        transform.localPosition = new Vector3(x, y, z);

        if (transform.tag == "Ball")
        {
            x = binaryReader.ReadSingle();
            y = binaryReader.ReadSingle();
            z = binaryReader.ReadSingle();
            transform.rotation = Quaternion.Euler(x, y, z);
        }

        x = binaryReader.ReadSingle();
        y = binaryReader.ReadSingle();
        z = binaryReader.ReadSingle();
        transform.localScale = new Vector3(x, y, z);

        if (transform.GetComponent<Animator>())
        {
            int hash = binaryReader.ReadInt32();
            if (transform.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).shortNameHash != (hash))
                transform.GetComponent<Animator>().Play(hash, 0, 0.0f);
        }
    }
    private void LoadTransforms(Transform[] transforms)
    {
        foreach (Transform transform in transforms)
        {
            LoadTransform(transform);
        }
    }
}
