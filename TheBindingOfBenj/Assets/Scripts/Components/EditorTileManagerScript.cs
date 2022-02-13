using GameLibrary;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorTileManagerScript : MonoBehaviour
{
    [SerializeField] private EditorTileScript _tile;
    [SerializeField, Range(1, 50)] private int _width = 15;
    [SerializeField, Range(1, 50)] private int _height = 15;
    [SerializeField] private TMP_InputField _levelName;
    [SerializeField] private GameObject _levelButtonPrefab;

    public TileType SelectedTileType { get; private set; }

    private readonly List<EditorTileScript> _tiles = new List<EditorTileScript>();


    void Start()
    {
        GenerateGrid();
        LoadLevels();
    }


    private void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
            for (int y = 0; y < _height; y++)
            {
                var tile = Instantiate(_tile, new Vector3(x, y), Quaternion.identity, transform);
                tile.name = $"Tile {x} {y}";
                _tiles.Add(tile);
            }

        Camera.main.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -1f);
    }

    public void ChangeTileSelection(int type)
    {
        SelectedTileType = (TileType)type;
    }

    public void ClearTiles()
    {
        foreach (var t in _tiles)
            t.Reset();
    }

    public void SaveLevel()
    {
        var tiles = _tiles.Select(t => t.Tile).ToList();
        var json = JsonConvert.SerializeObject(tiles);
        var path = $@"Assets\Resources\Rooms\{_levelName.text}.json";

        if (!File.Exists(path))
        {
            var room = Instantiate(_levelButtonPrefab, GameObject.Find("Content").transform);
            room.GetComponent<Button>().onClick.AddListener(() => LoadLevel(_levelName.text));
            room.GetComponentInChildren<TextMeshProUGUI>().text = _levelName.text;
        }

        File.WriteAllText(path, json);
    }

    private void LoadLevels()
    {
        var levels = Directory.GetFiles(@"Assets\Resources\Rooms\", "*.json");


        foreach (var l in levels)
        {
            var levelName = Path.GetFileNameWithoutExtension(l);
            var button = Instantiate(_levelButtonPrefab, GameObject.Find("Content").transform);
            button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelName));
            button.GetComponentInChildren<TextMeshProUGUI>().text = levelName;
        }
    }

    public void LoadLevel(string name)
    {
        var tiles = JsonConvert.DeserializeObject<List<Tile>>(File.ReadAllText($@"Assets\Resources\Rooms\{name}.json"));
        _levelName.text = name;

        foreach (var t in _tiles)
        {
            var matchTile = tiles.Find(x => x.X == t.transform.position.x && x.Y == t.transform.position.y);
            t.ChangeTile(matchTile.Type);
        }
    }
}
