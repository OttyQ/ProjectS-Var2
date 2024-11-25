using UnityEngine;

public class GoldSpawner : IGoldSpawner
{
    private readonly GameObject _goldPrefab;

    public GoldSpawner(GameObject goldPrefab)
    {
        _goldPrefab = goldPrefab;
    }

    public void SpawnGoldObject(Transform cell)
    {
        if (cell == null)
        {
            Debug.LogWarning("Cannot spawn gold: cell transform is null.");
            return;
        }

        GameObject gold = Object.Instantiate(_goldPrefab, cell.position, Quaternion.identity);
    
        gold.transform.SetParent(cell);
    
        gold.transform.localPosition = Vector3.zero;

        gold.transform.localScale = Vector3.one * 0.5f;

        Debug.Log($"Gold spawned at cell {cell.name}. Position: {gold.transform.position}");
    }
}
