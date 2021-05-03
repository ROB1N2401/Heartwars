using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Vector3 raycastOffset;
    
    private List<Tile> _adjacentTiles = new List<Tile>();

    void Update()
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

    void GetAdjacentTiles()
    {
        _adjacentTiles.Clear();
        var startPosition = player.attachedTile.LowestTileFromUnderneath.transform.position;
        for (int i = 0; i < 6; i++)
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray();
            Vector3 direction = new Vector3();

            switch (i)
            { 
                case 0:
                    direction = new Vector3(1, 0, 0);
                    break;
                case 1:
                    direction = new Vector3(1, 0, -1);
                    break;
                case 2:
                    direction = new Vector3(-1,0, -1);
                    break;
                case 3:
                    direction = new Vector3(-1,0, 0);
                    break;
                case 4:
                    direction = new Vector3(-1,0, 1);
                    break;
                case 5:
                    direction = new Vector3(1, 0, 1);
                    break;
                default:
                    Debug.LogError("Failed to assign a direction");
                    break;
            }

            //todo reduce hardcode
            ray = new Ray(startPosition + raycastOffset, direction);

            //todo reduce hardcode
            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                var tile = hit.transform.GetComponent<Tile>();
                if (tile != null)
                {
                    _adjacentTiles.Add(tile.HighestTileFromAbove);
                    _adjacentTiles.Add(tile);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        var rayDown = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit;
        Tile attachedTile = null;
        if (Physics.Raycast(rayDown, out hit))
            attachedTile = hit.transform.GetComponent<Tile>();
        if (attachedTile != null)
            attachedTile = attachedTile.LowestTileFromUnderneath;
            
        
        if (!isActiveAndEnabled)
            return;
        for (int i = 0; i < 6; i++)
        {
            Vector3 direction = new Vector3();

            switch (i)
            {
                case 0:
                    direction = new Vector3(1, 0, 0);
                    break;
                case 1:
                    direction = new Vector3(1, 0, -1);
                    break;
                case 2:
                    direction = new Vector3(-1, 0, -1);
                    break;
                case 3:
                    direction = new Vector3(-1, 0, 0);
                    break;
                case 4:
                    direction = new Vector3(-1, 0, 1);
                    break;
                case 5:
                    direction = new Vector3(1, 0, 1);
                    break;
                default:
                    Debug.LogError("Failed to assign a direction");
                    break;
            }

            Debug.DrawRay(attachedTile.transform.position + raycastOffset, direction);
        }
    }
}