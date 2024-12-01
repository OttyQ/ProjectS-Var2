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
        Vector3 newPos = cell.position;
        newPos.z = -0.5f;
        GameObject goldObject = Object.Instantiate(_goldPrefab, newPos, Quaternion.identity);

        return goldObject;
    }
}
