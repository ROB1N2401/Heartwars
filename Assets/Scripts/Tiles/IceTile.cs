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
        
        //todo debug
        Debug.Log(oppositeTile.gameObject.name);
        if(oppositeTile == null)
            return;
        
        oppositeTile = oppositeTile.HighestTileFromAbove;
    
        if (oppositeTile.TileData.IsWalkable)
            oppositeTile.PlacePlayer(player);
    }

    protected Tile GetTileFromOppositeDirection(Tile neighbourTile)
    {
        if (neighbourTile == null)
            return null;
        neighbourTile = neighbourTile.LowestTileFromUnderneath;
        
        var directionOfTheOppositeTile = neighbourTile.transform.position - LowestTileFromUnderneath.transform.position;
        directionOfTheOppositeTile = -directionOfTheOppositeTile.normalized;
        //todo debug
        _direction = directionOfTheOppositeTile;
        var rayToTheOppositeTile = new Ray(transform.position, directionOfTheOppositeTile);
        RaycastHit hit;

        if (Physics.Raycast(rayToTheOppositeTile, out hit)) 
            return hit.transform.GetComponent<Tile>();

        return null;
    }

    private Vector3 _direction;
    private new void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, _direction);
    }
}
