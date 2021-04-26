using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField] GameObject _player;

    GameObject _occupiedTile;
    List<GameObject> _adjacentTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        GetVoidTileUnderneath();
        CheckAdjacentTiles();
        for (int i = 0; i < _adjacentTiles.Count; i++)
        {
            Debug.Log(_adjacentTiles[i].name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAdjacentTiles();  
    }

    void GetVoidTileUnderneath()
    {
        Ray ray = new Ray(_player.transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f, (1 << 8)))
        {
            _occupiedTile = hit.transform.gameObject;
            //Debug.Log(_occupiedTile.name);
        }
    }

    void CheckAdjacentTiles()
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

            ray = new Ray(_occupiedTile.transform.position, direction);
            Debug.DrawRay(_occupiedTile.transform.position, direction);

            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                Debug.Log(hit.transform.name);

                ray = new Ray(hit.transform.position, Vector3.up);
                if (Physics.Raycast(ray, out hit, 1.0f))
                {
                    _adjacentTiles.Add(hit.transform.gameObject);
                }
            }
        }
    }

}
