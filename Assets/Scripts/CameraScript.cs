using UnityEngine;
using UnityEngine.U2D;

public class CameraScript : MonoBehaviour
{
    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        PixelPerfectCamera pixelCamera = GetComponent<PixelPerfectCamera>();
        
        int width = SettingsScript.MapSize.x;
        int height = SettingsScript.MapSize.y;

        camera.transform.position = new Vector3(width / 2.0f - 0.5f, (height + SettingsScript.UIHeight) / 2.0f - 0.5f, -10);
        camera.orthographicSize = (height + SettingsScript.UIHeight) / 2;
    
        pixelCamera.refResolutionX = SettingsScript.PixelCellSize.x * width;
        pixelCamera.refResolutionY = SettingsScript.PixelCellSize.y * height + SettingsScript.PixelUIHeight;
        
        Screen.SetResolution(pixelCamera.refResolutionX * 2, pixelCamera.refResolutionY * 2, false);
    }
}
