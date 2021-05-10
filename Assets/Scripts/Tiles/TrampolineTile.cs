using System.Collections.Generic;
using UnityEngine;

public class TrampolineTile : Tile
{
    public override void PlacePlayer(Player player)
    {
        if(player == null)
            return;
        
        var path = GetPathOfTilesToTheOppositeTileWithGap(player.attachedTile, 3);
        var destinationTile = path[path.Count - 1].HighestTileFromAbove;

        //Gets last walkable tile from the path
        for (var i = path.Count - 1; i > -1; i--)
        {
            if (destinationTile.TileData.IsWalkable)
            {
                destinationTile = path[i].HighestTileFromAbove;
                break;
            }
        }
        
        //todo debug
        _pos1 = LowestTileFromUnderneath.transform.position;
        _pos2 = path[path.Count - 1].transform.position;
        foreach(var tile in path)
            Debug.Log(tile.HighestTileFromAbove);
        
        if(destinationTile.TileData.TileType == ETileType.Void)
            player.Die();
        else
            destinationTile.PlacePlayer(player); 
    }

    private List<Tile> GetPathOfTilesToTheOppositeTileWithGap(Tile neighbourOppositeTile, int gap)
    {
        if (neighbourOppositeTile == null)
            return null;
        neighbourOppositeTile = neighbourOppositeTile.LowestTileFromUnderneath;
        gap = Mathf.Max(0, gap);
        
        var path = new List<Tile>();
        var directionOfPath = neighbourOppositeTile.transform.position - LowestTileFromUnderneath.transform.position;
        directionOfPath = -directionOfPath.normalized;
        var rayToNextTile = new Ray(LowestTileFromUnderneath.transform.position, directionOfPath);
        RaycastHit hit;

        for (int i = 0; i < gap; i++)
        {
            if (Physics.Raycast(rayToNextTile, out hit))
            {
                var baseTileOnThePath = hit.transform.GetComponent<Tile>().LowestTileFromUnderneath;
                if(baseTileOnThePath != null)
                    path.Add(baseTileOnThePath);
                
                rayToNextTile = new Ray(baseTileOnThePath.transform.position, directionOfPath);
            }
            else
                break;
        }

        return path;
    }


    //todo debug
    private Vector3 _pos1, _pos2;
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_pos1, _pos2);
    }
}
