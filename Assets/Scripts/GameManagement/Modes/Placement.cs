using System;
using UnityEngine;

public class Placement : Mode
{
    private TileBlueprintEntry _selectedBlueprintEntry;
    private GameObject _blueprintInstance;

    private void OnDisable()
    {
        if(_blueprintInstance != null)
            _blueprintInstance.SetActive(false);
    }

    private void Update()
    {
        GetAdjacentTilesAndPlayers();

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
                if (Input.GetMouseButtonDown(0) && PlayerManager.Instance.CurrentPlayer.HasSuchItemInInventory(_selectedBlueprintEntry.tileType))
                {
                    //todo reduce hardcode
                    PlayerManager.Instance.CurrentPlayer.PlaceTile(_selectedBlueprintEntry.tileType, selectedTile);
                }
            }
        }
    }
    
    /// <summary>Shows tile preview that will be placed</summary>
    /// <param name="baseTile">Tile upon which preview will be shown</param>
    private void DrawSelectedBlueprint(Tile baseTile)
    {
        if(_blueprintInstance != null)
        {
        _blueprintInstance.transform.position = baseTile.transform.position + baseTile.TileAbovePositionOffset;
        _blueprintInstance.SetActive(true);
        }
    }

    public void ReselectBlueprint(int id_in)
    {
        if (_blueprintInstance != null)
            Destroy(_blueprintInstance);

        _selectedBlueprintEntry = TilesManager.Instance.Blueprints[id_in];
        _blueprintInstance = Instantiate(_selectedBlueprintEntry.blueprintPrefab);
        _blueprintInstance.layer = LayerMask.NameToLayer("Ignore Raycast");
        _blueprintInstance.SetActive(false);
    }
}
