using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [SerializeField] GameObject _player;

    GameObject _occupiedTile;
    List<GameObject> _adjacentTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetTileUnderneath();
        GetAdjacentTiles();

        if (_adjacentTiles.Count > 0)
        {
            foreach (GameObject go in _adjacentTiles)
            {
                go.transform.gameObject.GetComponent<Outline>().enabled = false;
            }
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 50000f, (1 << 9), QueryTriggerInteraction.Ignore))
        {
            //Debug.Log(hit.transform.name);
            if (_adjacentTiles.Contains(hit.transform.gameObject))
            {
                hit.transform.gameObject.GetComponent<Outline>().enabled = true;
                hit.transform.gameObject.GetComponent<Outline>().OutlineColor = new Color(1.0f, 0, 0);
                if (Input.GetMouseButtonDown(0))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    void GetTileUnderneath()
    {
        Ray ray = new Ray(_player.transform.position, -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.1f))
        {
            _occupiedTile = hit.transform.gameObject;
            //Debug.Log(_occupiedTile.name);
        }
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
            //Debug.DrawRay(_occupiedTile.transform.position, direction);

            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                _adjacentTiles.Add(hit.transform.gameObject);
            }
        }
    }
}
