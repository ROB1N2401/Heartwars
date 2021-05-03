using UnityEngine;

public class Destruction : Mode
{ 
    private void Update()
    {
        GetAdjacentTiles();

        foreach (var go in _adjacentTiles)
        {
            var outline = go.transform.gameObject.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000f, (1 << 9), QueryTriggerInteraction.Ignore))
        {
            var tile = hit.transform.GetComponent<Tile>();
            if(tile == null)
                return;
            
            if (_adjacentTiles.Contains(tile))
            {
                var outline = tile.GetComponent<Outline>();
                if(outline != null)
                    tile.GetComponent<Outline>().enabled = true;
                
                if (Input.GetMouseButtonDown(0))
                    player.DestroyTopTile(tile);
            }
        }
    }
}