﻿using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Tween _moveTween;
    public TileType TileType { get; set; }

    private Empty _empty;
    public Empty Empty
    {
        get => _empty;
        set
        {
            _empty = value;
            if (_empty)
            {
                _empty.Tile = this;
                transform.SetParent(_empty.transform); //Set parent sau khi nhận Empty
                if (_moveTween != null && _moveTween.IsActive()) _moveTween.Kill();
                _moveTween = transform.DOLocalMove(Vector2.zero, Config.TileMoveDuration).OnComplete(() =>
                {
                    var selectedTile = InputManager.Instance.SelectedTile;
                    var targetTile = InputManager.Instance.TargetTile;
                    if (this == selectedTile) EventManager.EndSwapTileAction(selectedTile, targetTile);
                });
            }
        }
    }
    private SpriteRenderer spriteRenderer;
    public void InitialData(TileType type, Empty empty)
    {
        TileType = type;
        Empty = empty;
        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = LoadManager.SpriteLoad($"Tile/{type}");
    }

    private void OnDisable()
    {
        _moveTween?.Kill();
    }
}
