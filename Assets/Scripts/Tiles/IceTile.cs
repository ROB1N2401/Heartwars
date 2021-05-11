﻿public class IceTile : Tile
{ 
    public override void PlacePlayer(Player player)
    {
        if(player == null)
            return;
    
        var underneathTile = player.attachedTile.LowestTileFromUnderneath;
        var destinationTile = GetTileFromOppositeDirection(underneathTile);
        
        base.PlacePlayer(player);

        if (destinationTile == null)
        {
            player.Die();
            return;
        }

        destinationTile = destinationTile.HighestTileFromAbove;
        
        destinationTile.PlacePlayer(player);
        // if (destinationTile.TileData.IsWalkable)
        //     destinationTile.PlacePlayer(player);
        // else if(destinationTile.TileData.TileType == ETileType.Void)
        //     player.Die();
    }
}