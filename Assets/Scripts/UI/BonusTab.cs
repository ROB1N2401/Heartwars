using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusTab : MonoBehaviour
{
    public static BonusTab Instance { get; private set; }

    [SerializeField] GameObject bonusBuffPrefab;
    [SerializeField] GameObject bonusSpawnPrefab;

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

    //todo: find a good way to combine this and next method
    private void InstantiateBuffBonus()
    {
        var go = Instantiate(bonusBuffPrefab) as GameObject;
        go.transform.SetParent(this.transform);
    }

    private void InstantiateSpawnBonus(ESide side_in)
    {
        var go = Instantiate(bonusSpawnPrefab) as GameObject;
        go.transform.SetParent(this.transform);

        var imageRef = go.GetComponent<Image>();
        switch (side_in)
        {
            case ESide.Blue:
                imageRef.color = new Color(0.1f, 0.1f, 0.75f);
                break;
            case ESide.Green:
                imageRef.color = new Color(0.1f, 0.75f, 0.1f);
                break;
            case ESide.Orange:
                imageRef.color = new Color(255, 140, 0);
                break;
            case ESide.Purple:
                imageRef.color = new Color(144, 0, 255);
                break;
            case ESide.Red:
                imageRef.color = new Color(0.75f, 0.1f, 0.1f);
                break;
            case ESide.Yellow:
                imageRef.color = Color.yellow;
                break;
        }
    }

    public void UpdateBonusTab()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }

        if(PlayerManager.Instance.CurrentPlayer.HasSuchItemInInventory(ETileType.Spawn))
        {
            var playerInventoryRef = PlayerManager.Instance.CurrentPlayer.GetComponent<PlayerInventory>();
            List<Tile> spawnTiles = playerInventoryRef.GetAllTilesOfAGivenType(ETileType.Spawn);
            foreach(Tile tile in spawnTiles)
            {
                Debug.Log(tile.tileSide);
                InstantiateSpawnBonus(tile.tileSide);
            }
        }

        if (PlayerManager.Instance.CurrentPlayer.HasSuchItemInInventory(ETileType.Bonus))
            InstantiateBuffBonus();
    }
}