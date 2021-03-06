using System.Collections.Generic;
using UnityEngine;

public class Mode : MonoBehaviour
{
    [SerializeField] protected Vector3 raycastOffset;
    
    protected List<Tile> _adjacentTiles = new List<Tile>();
    protected List<Player> _adjacentPlayers = new List<Player>();
    
    /// <summary>Sets all adjacent tiles and players on it to the proper fields</summary>
    protected void GetAdjacentTilesAndPlayers()
    {
        _adjacentTiles.Clear();
        _adjacentPlayers.Clear();
        var startPosition = PlayerManager.Instance.CurrentPlayer.attachedTile.LowestTileFromUnderneath.transform.position;
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
                    var veryTopTile = tile.HighestTileFromAbove;
                    var attachedPlayer = veryTopTile.AttachedPlayer;
                    
                    if (attachedPlayer != null)
                        _adjacentPlayers.Add(attachedPlayer);
                    
                    _adjacentTiles.Add(veryTopTile);
                    _adjacentTiles.Add(tile);
                }
            }
        }
    }
}
