using System;
using UnityEngine;

public class Placement : Mode
{
    [Serializable]
    private class TileBlueprintEntry
    {
        public GameObject blueprintPrefab;
        public ETileType tileType;
        public KeyCode keyThatSelectsTile;
    }

    [SerializeField] private TileBlueprintEntry[] blueprints;

    private TileBlueprintEntry _selectedBlueprintEntry;
    private GameObject _blueprintInstance;

    private void Awake()
    {
        _selectedBlueprintEntry = blueprints[0];
        _blueprintInstance = Instantiate(_selectedBlueprintEntry.blueprintPrefab);
        _blueprintInstance.layer = LayerMask.NameToLayer("Ignore Raycast");
        _blueprintInstance.SetActive(false);
    }

    private void OnDisable()
    {
        if(_blueprintInstance != null)
            _blueprintInstance.SetActive(false);
    }

    private void Update()
    {
        GetAdjacentTiles();

        var mousePointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(mousePointRay, out hit))
        {
            var selectedTile = hit.transform.GetComponent<Tile>();
            if(selectedTile == null)
                return;

            selectedTile = selectedTile.HighestTileFromAbove;
            
            if (_adjacentTiles.Contains(selectedTile))
            {
                DrawSelectedBlueprint(selectedTile);
                
                //todo reduce hardcode
                if (Input.GetMouseButtonDown(0) && player.HasSuchItemInInventory(_selectedBlueprintEntry.tileType))
                {
                    //todo reduce hardcode
                    player.PlaceTile(_selectedBlueprintEntry.tileType, selectedTile);
                }
            }
        }
    }
    
    /// <summary>Shows tile preview that will be placed</summary>
    /// <param name="baseTile">Tile upon which preview will be shown</param>
    private void DrawSelectedBlueprint(Tile baseTile)
    {
        foreach (var blueprintEntry in blueprints)
        {
            if (Input.GetKey(blueprintEntry.keyThatSelectsTile))
            {
                Destroy(_blueprintInstance);
                _selectedBlueprintEntry = blueprintEntry;
                _blueprintInstance = Instantiate(_selectedBlueprintEntry.blueprintPrefab);
                _blueprintInstance.layer = LayerMask.NameToLayer("Ignore Raycast");
                _blueprintInstance.SetActive(false);
            }
        }
        
        _blueprintInstance.transform.position = baseTile.transform.position + baseTile.TileAbovePositionOffset;
        _blueprintInstance.SetActive(true);
    }
}
