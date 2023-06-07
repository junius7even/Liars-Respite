using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraSwitch : MonoBehaviour
{
    public Camera MainCamera;
    public Camera MemoryCamera;
    public PostProcessVolume postProcessVolume;

    private bool isMainCameraActive = true;
    private AudioListener mainAudioListener;
    private AudioListener memoryAudioListener;

    void Start()
    {
        mainAudioListener = MainCamera.GetComponent<AudioListener>();
        memoryAudioListener = MemoryCamera.GetComponent<AudioListener>();

        // Enable the audio listener of the main camera at the start
        mainAudioListener.enabled = true;
        memoryAudioListener.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        isMainCameraActive = !isMainCameraActive;

        MainCamera.enabled = isMainCameraActive;
        MainCamera.GetComponent<AudioListener>().enabled = isMainCameraActive;

        MemoryCamera.enabled = !isMainCameraActive;
        MemoryCamera.GetComponent<AudioListener>().enabled = !isMainCameraActive;

        postProcessVolume.enabled = !isMainCameraActive;
    }
}
