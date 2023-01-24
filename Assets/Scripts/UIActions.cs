using System;

public static class UIActions
{
    public static Action OnResetGame;
    public static Action<int> OnChangedFlags;
    public static Action OnFirstClick;
    public static Action<GridManagerScript.EGameOver> OnGameOver;
}
