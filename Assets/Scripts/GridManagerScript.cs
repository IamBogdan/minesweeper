using System.Collections.Generic;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    public enum EGameOver {
        Win,
        Lose
    }

    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _cellPrefab;

    private CellScript[,] _map;
    private int _width;
    private int _height;
    private int _totalMines;
    private int __currentMines;
    private int _currentMines {
        get => __currentMines;
        set {
            __currentMines = value;
            UIActions.OnChangedFlags?.Invoke(__currentMines);
        }
    }
    private bool _isFirstClick = true;
    private bool _isGameFinished = false;

    public static GridManagerScript Instance {
        get;
        private set;
    }

    private void Awake() {
        if (Instance == null) { 
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }

        UIActions.OnResetGame += ResetGame;
        UIActions.OnOpenSettings += ClearGame;
        UIActions.OnExitSettings += Start;

        // _currentMines = _totalMines = SettingsManagerScript.TotalMines;
        
        // _width = SettingsManagerScript.MapSize.x;
        // _height = SettingsManagerScript.MapSize.y;

        // _map = new CellScript[_width, _height];
    }

    public void Start() {
        _currentMines = _totalMines = SettingsManagerScript.TotalMines;
        _width = SettingsManagerScript.MapSize.x;
        _height = SettingsManagerScript.MapSize.y;

        GenerateMap();
        GenerateMines();
    }

    public void LeftClickHandler(CellScript cell) {
        if (_isGameFinished) {
            return;
        }

        // Debug.Log("ClickHandler: " + cell.Position.x + ", " + cell.Position.y);

        if (_isFirstClick) {
            UIActions.OnFirstClick?.Invoke();
            _isFirstClick = false;
            
            if (_map[cell.Position.x, cell.Position.y].Type == CellScript.EType.Mine) {
                ReplaceMine(cell.Position.x, cell.Position.y);
            }
        }

        Reveal(cell.Position.x, cell.Position.y);
    }

    public void RightClickHandler(CellScript cell) {
        if (_isGameFinished) {
            return;
        }

        if (!cell.IsRevealed) {
            if (cell.IsFlagged) {
                _currentMines++;
            }
            else {
                _currentMines--;
            }
        }
        Debug.Log("current mines: " + _currentMines);

        cell.SwapFlag();
    }

    private void GenerateMap() {
        _map = new CellScript[_width, _height];

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                Vector2Int position = new Vector2Int(x, y);

                GameObject instance = Instantiate(_cellPrefab, new Vector3(position.x, position.y), Quaternion.identity);
                instance.name = $"Cell {x}, {y}";

                CellScript cell = instance.GetComponent<CellScript>();
                cell.Init(position, CellScript.EType.Safe);

                _map[x, y] = cell;
            }
        }
    }

    private void GenerateMines() {
        if (_totalMines > _width * _height) {
            _totalMines = _width * _height;
        }

        int i = 0;
        while (i < _totalMines) {
            int x = Random.Range(0, _width);
            int y = Random.Range(0, _height);

            if (_map[x, y].Type == CellScript.EType.Mine) {
                continue;
            }

            _map[x, y].SetMine();

            i++;
        }
    }

    private void ReplaceMine(int x, int y) {
        List<Vector2Int> free = new List<Vector2Int>();

        for (int xx = 0; xx < _width; xx++) {
            for (int yy = 0; yy < _height; yy++) {
                if (_map[xx, yy].Type == CellScript.EType.Safe) {
                    free.Add(new Vector2Int(xx, yy));
                }
            }
        }

        if (free.Count == 0) {
            Debug.Log("Warning GridManagerScript::ReplaceMine: no free cell");
            return;
        }

        Vector2Int freePos = free[Random.Range(0, free.Count)];

        _map[freePos.x, freePos.y].SetMine();
        _map[x, y].SetSafe();
    }

    private bool OutBounds(int x, int y) {
        return x < 0 || y < 0 || x >= _width || y >= _height;
    }

    private int CalcMinesNear(int x, int y) {
        if (OutBounds(x, y)) {
            return 0;
        }

        int mines = 0;
        for (int offsetX = -1; offsetX <= 1; offsetX++) {
            for (int offsetY = -1; offsetY <= 1; offsetY++) {
                if (OutBounds(x + offsetX, y + offsetY) || (offsetX == 0 && offsetY == 0)) {
                    continue;
                }

                mines += _map[x + offsetX, y + offsetY].Type == CellScript.EType.Mine ? 1 : 0;
            }
        }

        return mines;
    }

    private void Reveal(int x, int y) {
        if (OutBounds(x, y) || _map[x, y].IsRevealed) {
            return;
        }
        
        if (_map[x, y].IsFlagged) {
            _currentMines++;
            _map[x, y].SwapFlag();
        }

        int minesNear = CalcMinesNear(x, y);
        if (_map[x, y].Reveal((CellScript.EValue)minesNear) == CellScript.EType.Mine) {
            Lose();
            return;
        }
        else if (AreAllSafeRevealed()) {
            Win();
        }

        if (minesNear != 0) {
            return;
        }

        for (int offsetX = -1; offsetX <= 1; offsetX++) {
            for (int offsetY = -1; offsetY <= 1; offsetY++) {
                if (offsetX == 0 && offsetY == 0) {
                    continue;
                }
                Reveal(x + offsetX, y + offsetY);
            }
        }
    }

    private void Lose() {
        UIActions.OnGameOver?.Invoke(EGameOver.Lose);

        _isGameFinished = true;
        Debug.Log("BAM! YOU LOSE!");

        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                if (_map[x, y].IsFlagged && _map[x, y].Type == CellScript.EType.Mine) {
                    continue;
                }
                else if (_map[x, y].IsFlagged && _map[x, y].Type == CellScript.EType.Safe) { // show mistakes
                    _map[x, y].SetWrongFlag();
                    continue;
                }

                _map[x, y].RevealIfMine();
            }
        }
    }

    private void Win() {
        UIActions.OnGameOver?.Invoke(EGameOver.Win);

        _isGameFinished = true;
        Debug.Log("ALL ARE REVEALED. WIN!");

        FlagAllMines();
    }

    private bool AreAllSafeRevealed() {
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                if (_map[x, y].Type == CellScript.EType.Safe && !_map[x, y].IsRevealed) {
                    return false;
                }
            }
        }
        return true;
    }

    private void FlagAllMines() {
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                if (_map[x, y].Type == CellScript.EType.Mine && !_map[x, y].IsFlagged) {
                    _map[x, y].SwapFlag();
                }
            }
        }
        _currentMines = 0;
        UIActions.OnChangedFlags(_currentMines);
    }

    private void ResetValues() {
        _isFirstClick = true;
        _isGameFinished = false;
        _currentMines = _totalMines;
    }

    private void ResetGame() {
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                _map[x, y].Init(new Vector2Int(x, y), CellScript.EType.Safe);
            }
        }
        
        GenerateMines();
        ResetValues();
    }

    private void ClearGame() {
        if (_map == null) {
            return;
        }

        ResetValues();
        
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                _map[x, y].Destroy();
            }
        }
        _map = null;
    }
}
