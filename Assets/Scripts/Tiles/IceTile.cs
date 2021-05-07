using UnityEngine;

public class IceTile : Tile
{ 
    public override void PlacePlayer(Player player)
    {
        if(player == null)
            return;
    
        var underneathTile = player.attachedTile.LowestTileFromUnderneath;
        var oppositeTile = GetTileFromOppositeDirection(underneathTile);
        
        base.PlacePlayer(player);

        if (oppositeTile == null)
        {
            player.Die();
            return;
        }

        oppositeTile = oppositeTile.HighestTileFromAbove;
        
        if (oppositeTile.TileData.IsWalkable)
            oppositeTile.PlacePlayer(player);
        else if(oppositeTile.TileData.TileType == ETileType.Void)
            player.Die();
    }

    protected Tile GetTileFromOppositeDirection(Tile neighbourTile)
    {
        if (neighbourTile == null)
            return null;
        neighbourTile = neighbourTile.LowestTileFromUnderneath;

        var positionOfTheBaseTile = LowestTileFromUnderneath.transform.position;
        var directionOfTheOppositeTile = neighbourTile.transform.position - positionOfTheBaseTile;
        directionOfTheOppositeTile = -directionOfTheOppositeTile.normalized;
        
        //todo debug
        _direction = directionOfTheOppositeTile;
        _start = positionOfTheBaseTile;
        
        var rayToTheOppositeTile = new Ray(positionOfTheBaseTile, directionOfTheOppositeTile);
        RaycastHit hit;

        if (Physics.Raycast(rayToTheOppositeTile, out hit)) 
            return hit.transform.GetComponent<Tile>();

        return null;
    }

    private Vector3 _direction;
    private Vector3 _start;
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(_start, _direction);
    }
}
