using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        PixelPerfectCamera pixelCamera = GetComponent<PixelPerfectCamera>();

        int width = SettingsScript.MapSize.x;
        int height = SettingsScript.MapSize.y;

        int scale = 2; // 1 2 4 8

        float UIPanelPixelHeight = GameObject.Find("UIPanel").GetComponent<RectTransform>().sizeDelta.y;

        camera.transform.position = new Vector3(width / 2.0f - 0.5f, (height + UIPanelPixelHeight / 16) / 2.0f - 0.5f, -10);
        // camera.orthographicSize = (height + UIPanelPixelHeight / 16) / 2;
    
        pixelCamera.refResolutionX = SettingsScript.PixelCellSize.x * width;
        pixelCamera.refResolutionY = SettingsScript.PixelCellSize.y * height + (int)UIPanelPixelHeight;

        GameObject.Find("Canvas").GetComponent<CanvasScaler>().scaleFactor = scale;
        Screen.SetResolution(pixelCamera.refResolutionX * scale, pixelCamera.refResolutionY * scale, false);
    }
}
