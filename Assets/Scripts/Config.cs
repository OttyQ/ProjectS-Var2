using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class Config : ScriptableObject
{
    [Header("Game Settings")]
    [SerializeField] private int _fieldSize = 3;
    [SerializeField] private int _maxDepth = 3;
    [SerializeField] private int _initialShovelCount = 10;
    [SerializeField] private int _requiredGoldBars = 3;
    [SerializeField] private float _goldSpawnChance = 0.2f;
    [SerializeField] private float _goldSpawnChanceIncrement = 0.1f;

    public int FieldSize => _fieldSize;
    public int MaxDepth => _maxDepth;
    public int InitialShovelCount => _initialShovelCount;
    public int RequiredGoldBars => _requiredGoldBars;
    public float GoldSpawnChance => _goldSpawnChance;
    public float GoldSpawnChanceIncrement => _goldSpawnChanceIncrement;
}
