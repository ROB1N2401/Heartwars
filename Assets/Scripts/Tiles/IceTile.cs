public class IceTile : Tile
{ 
    public override void PlacePlayer(Player player, ETransitionType transitionType = ETransitionType.Walk)
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

        if(destinationTile.TileData.IsWalkable|| destinationTile.TileData.TileType == ETileType.Void)
            destinationTile.PlacePlayer(player);
    }
}