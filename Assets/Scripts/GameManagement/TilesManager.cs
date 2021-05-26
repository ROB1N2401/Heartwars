using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Placement))]
public class TilesManager : MonoBehaviour
{
    public static TilesManager Instance { get; private set; }

    [SerializeField] private TileBlueprintEntry[] blueprints;

    public TileBlueprintEntry[] Blueprints => blueprints;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectNewBlueprint(int id_in)
    {
        var placementRef = GetComponent<Placement>();
        placementRef.ReselectBlueprint(id_in);
    }
}
