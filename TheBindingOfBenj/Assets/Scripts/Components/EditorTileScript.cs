using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EditorTileScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _entraceMask;
    [SerializeField] private List<Sprite> _sprites;

    private SpriteRenderer _spriteRenderer;
    private EditorTileManagerScript _tileManager;

    public Tile Tile { get; set; }

    private void Start()
    {
        Tile = new Tile
        {
            Type = TileType.Empty,
            X = transform.position.x,
            Y = transform.position.y
        };

        var isEntrance = (Tile.X == 7 || Tile.Y == 7) && (Tile.X == 0 || Tile.X == 14 || Tile.Y == 0 || Tile.Y == 14);
        _entraceMask.SetActive(isEntrance);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tileManager = transform.parent.GetComponent<EditorTileManagerScript>();
    }

    public void Reset()
    {
        ChangeTile(TileType.Empty);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);

        if (Mouse.current.leftButton.isPressed)
            OnPointerDown(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _highlight.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ChangeTile(_tileManager.SelectedTileType);
    }

    public void ChangeTile(TileType type)
    {
        _spriteRenderer.sprite = _sprites[(int)type];
        Tile.Type = type;
    }
}
