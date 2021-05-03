using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject blueprintToShow;
    [SerializeField] private Vector3 raycastOffset;
    private Tile _occupiedTile;
    private List<Tile> _adjacentTiles = new List<Tile>();
    
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
                //todo replace hardcode
                if (Input.GetMouseButtonDown(0) && player.HasSuchItemInInventory(ETileType.Floor))
                {
                    //todo remove hardcode
                    player.PlaceTile(ETileType.Floor, selectedTile);
                }
            }
        }
    }
    
    void GetAdjacentTiles()
    {
        _adjacentTiles.Clear();
        var startPosition = player.attachedTile.LowestTileFromUnderneath.transform.position;
        for (int i = 0; i < 6; i++)
        {
            var ray = new Ray();
            RaycastHit hit;
            Vector3 direction = Vector3.zero;

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
