using UnityEngine;

public static class SettingsScript
{
    public static Vector2Int MapSize = new Vector2Int(11, 15);
    public static int TotalMines = 10;
    
    public static Vector2Int PixelCellSize = new Vector2Int(16, 16);
    public static int UIHeight = 2;
    public static int PixelUIHeight = PixelCellSize.y * UIHeight;
}