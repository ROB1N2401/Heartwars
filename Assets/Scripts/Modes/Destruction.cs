using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [SerializeField] Player _player;

    private Tile _occupiedTile;
    private List<GameObject> _adjacentTiles = new List<GameObject>();

    void Update()
    {
        GetTileUnderneath();
        GetAdjacentTiles();

        foreach (var go in _adjacentTiles)
        {
            var outline = go.transform.gameObject.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000f, (1 << 9), QueryTriggerInteraction.Ignore))
        {
            var tile = hit.transform.GetComponent<Tile>();
            if(tile.Equals(null))
                return;
            
            if (_adjacentTiles.Contains(tile.gameObject))
            {
                tile.GetComponent<Outline>().enabled = true;
                tile.GetComponent<Outline>().OutlineColor = Color.black;
                if (Input.GetMouseButtonDown(0)) 
                    _player.DestroyTile(tile);
            }
        }
    }

    void GetTileUnderneath()
    {
        Ray ray = new Ray(_player.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.1f)) 
            _occupiedTile = hit.transform.GetComponent<Tile>();
    }

    void GetAdjacentTiles()
    {
        _adjacentTiles.Clear();
        
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

            var playerPosition = _player.transform.position;
            
            //todo reduce hardcode
            ray = new Ray(playerPosition + Vector3.down * .1f, direction);

            //todo reduce hardcode
            if (Physics.Raycast(ray, out hit, 1.0f)) 
                _adjacentTiles.Add(hit.transform.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled)
            return;

        for (int i = 0; i < 6; i++)
        {
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
                    direction = new Vector3(-1, 0, -1);
                    break;
                case 3:
                    direction = new Vector3(-1, 0, 0);
                    break;
                case 4:
                    direction = new Vector3(-1, 0, 1);
                    break;
                case 5:
                    direction = new Vector3(1, 0, 1);
                    break;
                default:
                    Debug.LogError("Failed to assign a direction");
                    break;
            }

            Debug.DrawRay(_player.transform.position + Vector3.down * .1f, direction);
        }
    }
}
