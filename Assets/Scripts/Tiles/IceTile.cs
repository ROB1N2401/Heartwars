public class IceTile : Tile
{
    public override void PlacePlayer(Player player, ETransitionType transitionType = ETransitionType.Walk)
    {
        if (player == null)
            return;

        var animator = player.GetComponent<TransitionControl>();
        var underneathTile = player.attachedTile.LowestTileFromUnderneath;
        var destinationTile = GetTileFromOppositeDirection(underneathTile);
        
        if(animator) 
            animator.DirectTransition(PositionForPlayer);

        if (destinationTile == null)
        {
            player.Die();
            return;
        }

        destinationTile = destinationTile.HighestTileFromAbove;

        if ((destinationTile.TileData.IsWalkable || destinationTile.TileData.TileType == ETileType.Void) && !destinationTile.IsPlayerOnTile)
            destinationTile.PlacePlayer(player);
        else
            base.PlacePlayer(player);
    }
}