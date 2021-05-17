using UnityEngine;

public class Destruction : Mode
{ 
    private void Update()
    {
        GetAdjacentTilesAndPlayers();

        foreach (var go in _adjacentTiles)
        {
            var outline = go.transform.gameObject.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;
        }

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue, (1 << 9), QueryTriggerInteraction.Ignore))
        {
            var tile = hit.transform.GetComponent<Tile>();
            if(tile == null)
                return;

            tile = tile.HighestTileFromAbove;
            
            if (_adjacentTiles.Contains(tile))
            {
                var outline = tile.GetComponent<Outline>();
                if(outline != null)
                    tile.GetComponent<Outline>().enabled = true;
                
                //todo reduce hardcode
                if (Input.GetMouseButtonDown(0))
                {
                    player.DestroyTopTile(tile);
                    AudioManager.InvokeDestructionSound(tile.TileData.TileType);
                }
            }
        }
    }
}