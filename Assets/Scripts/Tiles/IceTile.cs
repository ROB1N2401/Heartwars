public class IceTile : Tile
{
    public override void PlacePlayer(Player player, ETransitionType transitionType = ETransitionType.Walk)
    {
        if (player == null)
            return;

        var animator = player.GetComponent<PlayerTransition>();
        var underneathTile = player.attachedTile.LowestTileFromUnderneath;
        var destinationTile = GetTileFromOppositeDirection(underneathTile);
        
        if(animator) 
            animator.DirectTransition(PositionForPlayer);

        //todo debug
        print($"Ice tile opposite tile {destinationTile}");
        print($"under: {_neighbourTiles.underTile} above: {_neighbourTiles.aboveTile} neighbour above: {LowestTileFromUnderneath._neighbourTiles.aboveTile}");
        
        if (destinationTile == null)
        {
            player.Die();
            return;
        }

        destinationTile = destinationTile.HighestTileFromAbove;
        
        //todo debug
        print($"Ice tile opposite tile {destinationTile}");

        if ((destinationTile.TileData.IsWalkable || destinationTile.TileData.TileType == ETileType.Void) && !destinationTile.IsPlayerOnTile)
            destinationTile.PlacePlayer(player);
        else
            base.PlacePlayer(player);
    }
}