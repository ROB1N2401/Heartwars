using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _tileBP;
    [SerializeField] GameObject _tile;

    readonly Vector3 _raycastOffset = new Vector3(0, -0.05f, 0);
    GameObject _occupiedTile;
    GameObject _blueprint;
    List<GameObject> _adjacentTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        GetVoidTileUnderneath();
        GetAdjacentTiles();
        //for (int i = 0; i < _adjacentTiles.Count; i++)
        //{
        //    Debug.Log(_adjacentTiles[i].name);
        //}
    }

    void Start()
    {
        _blueprint = Instantiate(_tileBP) as GameObject;
        _blueprint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetVoidTileUnderneath();
        GetAdjacentTiles();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (_adjacentTiles.Contains(hit.transform.gameObject))
            {
                _blueprint.transform.position = hit.transform.position - (_raycastOffset * 2);
                _blueprint.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    var tile = Instantiate(_tile, _blueprint.transform.position, _blueprint.transform.rotation);
                    _blueprint.SetActive(false);
                }
            }
        }
        else if (_blueprint.gameObject.activeSelf)
        {
            _blueprint.SetActive(false);
        }
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


            ray = new Ray(_occupiedTile.transform.position + _raycastOffset, direction);
            //Debug.DrawRay(_occupiedTilePosition, direction);

            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                ray = new Ray(hit.transform.position + _raycastOffset, Vector3.up);
                //Debug.Log(hit.transform.name);
                //Debug.DrawRay(hit.transform.position, Vector3.up);

                if (!Physics.Raycast(ray, 1.0f))
                {
                    _adjacentTiles.Add(hit.transform.gameObject);
                }
            }
        }
    }

}
