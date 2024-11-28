using UnityEngine;

public class GoldSpawner : IGoldSpawner
{
    private readonly GameObject _goldPrefab;

    public GoldSpawner(GameObject goldPrefab)
    {
        _goldPrefab = goldPrefab;
    }

    public GameObject SpawnGoldObject(Transform cell)
    {
        GameObject goldObject = Object.Instantiate(_goldPrefab, cell.position, Quaternion.identity);

        return goldObject;
    }
}
