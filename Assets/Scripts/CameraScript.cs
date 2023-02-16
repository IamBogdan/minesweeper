using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    private Camera _camera;
    private PixelPerfectCamera _pixelCamera;
    private float _UIPanelPixelHeight;

    private void Awake() {
        _camera = GetComponent<Camera>();
        _pixelCamera = GetComponent<PixelPerfectCamera>();
        _UIPanelPixelHeight = GameObject.Find("UIPanel").GetComponent<RectTransform>().sizeDelta.y;

        GameObject.Find("Canvas").GetComponent<CanvasScaler>().scaleFactor = SettingsManagerScript.CameraScale;

        UIActions.OnOpenSettings += SetSettingsCamera;
        UIActions.OnExitSettings += SetCamera;

        SetCamera();
    }

    private void SetSettingsCamera() {
        int width = SettingsManagerScript.SettingsWindowSize.x;
        int height = SettingsManagerScript.SettingsWindowSize.y;

        _camera.transform.position = new Vector3(width / (16 + 2.0f - 0.5f), (height / 16 + _UIPanelPixelHeight / 16) / 2.0f - 0.5f, -10);

        _pixelCamera.refResolutionX = width;
        _pixelCamera.refResolutionY = height;

        Screen.SetResolution(_pixelCamera.refResolutionX * SettingsManagerScript.CameraScale, _pixelCamera.refResolutionY * SettingsManagerScript.CameraScale, false);
    }

    private void SetCamera() {
        int width = SettingsManagerScript.MapSize.x;
        int height = SettingsManagerScript.MapSize.y;

        _camera.transform.position = new Vector3(width / 2.0f - 0.5f, (height + _UIPanelPixelHeight / 16) / 2.0f - 0.5f, -10);

        _pixelCamera.refResolutionX = CellScript.PixelCellSize.x * width;
        _pixelCamera.refResolutionY = CellScript.PixelCellSize.y * height + (int)_UIPanelPixelHeight;

        Screen.SetResolution(_pixelCamera.refResolutionX * SettingsManagerScript.CameraScale, _pixelCamera.refResolutionY * SettingsManagerScript.CameraScale, false);
    }

    private void OnApplicationQuit() {
        PlayerPrefs.DeleteKey("Screenmanager Resolution Height");
        PlayerPrefs.DeleteKey("Screenmanager Resolution Width");
    }
}
