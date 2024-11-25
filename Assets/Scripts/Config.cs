using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class Config : ScriptableObject
{
    [Header("Game Settings")]
    [SerializeField] private int _fieldSize = 3; 
    public int fieldSize => _fieldSize;

    [SerializeField] private int _maxDepth = 3;
    public int maxDepth => _maxDepth;

    [SerializeField] private int _initialShovelCount = 10;

    public int initialShovelCount => _initialShovelCount;

    [SerializeField] private int _requiredGoldBars = 3;

    public int requiredGoldBars => _requiredGoldBars;

    [SerializeField] private float _goldSpawnChance = 0.2f;

    public float goldSpawnChance => _goldSpawnChance;

    [SerializeField] private float _goldSpawnChanceIncrement = 0.1f;

    public float goldSpawnChanceIncrement => _goldSpawnChanceIncrement;
}
