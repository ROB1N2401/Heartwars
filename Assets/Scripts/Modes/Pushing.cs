using UnityEngine;

public class Pushing : Mode
{
    private void Update()
    {
        GetAdjacentTilesAndPlayers();

        foreach (var go in _adjacentTiles)
        {
            var outline = go.transform.gameObject.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;
        }

        foreach (var player in _adjacentPlayers)
        {
            var outline = player.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;
        }

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.MaxValue, (1 << 9 | 1 << 10), QueryTriggerInteraction.Ignore))
        {
            var playerToPush = hit.transform.GetComponent<Player>();
            if(playerToPush == null)
                return;

            if (_adjacentPlayers.Contains(playerToPush))
            {
                var outline = playerToPush.GetComponent<Outline>();
                if(outline != null)
                    playerToPush.GetComponent<Outline>().enabled = true;
                
                //todo reduce hardcode
                if (Input.GetMouseButtonDown(0))
                    PlayerManager.Instance.CurrentPlayer.PushOtherPlayer(playerToPush);
            }
        }
    }
}
