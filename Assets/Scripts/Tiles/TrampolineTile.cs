using System.Collections.Generic;
using UnityEngine;

public class TrampolineTile : Tile
{
    public override void PlacePlayer(Player player, ETransitionType transitionType = ETransitionType.Walk)
    {
        if(player == null)
            return;
        
        var path = GetPathOfTilesToTheOppositeTileWithGap(player.attachedTile, 3);
        Tile destinationTile = this;

        if (path.Count < 3)
        {
            player.Die();
            return;
        }

        //Gets last walkable tile from the path
        if (path.Count > 0)
        {
            for (var i = path.Count - 1; i > -1; i--)
            {
                var tileOnPath = path[i].HighestTileFromAbove;

                if ((tileOnPath.TileData.IsWalkable || tileOnPath.TileData.TileType == ETileType.Void) && !tileOnPath.IsPlayerOnTile)
                {
                    destinationTile = tileOnPath;
                    break;
                }
            }
        }
        else
        {
            player.Die();
            return;
        }

        if (destinationTile == this)
        {
            base.PlacePlayer(player);
            return;
        }

        var animator = player.GetComponent<PlayerTransition>();
        if(animator != null)
            animator.DirectTransition(destinationTile.PositionForPlayer);
        
        destinationTile.PlacePlayer(player); 
    }

    /// <summary>Gets all the tiles on the opposite direction of a given tile direction</summary>
    /// <param name="neighbourOppositeTile">Tile for which opposite direction will be taken</param>
    /// <param name="gap">Length of the path (in Tiles)</param>
    /// <returns>Returns list of tiles of the path</returns>
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
            if (Physics.Raycast(rayToNextTile, out hit, 1f))
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
}
