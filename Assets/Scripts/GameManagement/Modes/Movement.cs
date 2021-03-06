using UnityEngine;

public class Movement : Mode
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

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var tile = hit.transform.GetComponent<Tile>();
            if(tile == null)
                return;
            var outline = tile.GetComponent<Outline>();

            if (_adjacentTiles.Contains(tile) &&
                !TileTransition.Instance.IsTransitioning && 
                !PlayerManager.Instance.CurrentPlayer.GetComponent<PlayerTransition>().IsTransitionTime)
            {
               if(outline != null)
                   outline.enabled = true;
               
               //todo reduce hardcode
               if (Input.GetMouseButtonDown(0))
                    PlayerManager.Instance.CurrentPlayer.MoveTo(tile);
            } 
        }
    }
}
