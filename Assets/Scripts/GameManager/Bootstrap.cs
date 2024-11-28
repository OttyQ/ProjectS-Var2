//using Unity.VisualScripting;
//using UnityEngine;


///// <summary>
///// Класс для инициализации.
///// Включает в себя загрузку сохраненных данных, настройку зависимостей, сохранение данных и подписку на события.
///// </summary>
//public class Bootstrap : MonoBehaviour
//{
//    [Header("Req Elements")]
//    [SerializeField] private RewardManager rewardManager;
//    [SerializeField] private View view;
//    [SerializeField] private GridFill gridFill;
//    [SerializeField] private WinMenu winMenu;
//    [SerializeField] private GameObject cellPrefab;
//    [SerializeField] private Transform gridParent;
//    [SerializeField] private GameObject goldPrefab;
//    [SerializeField] private UIBag bag;
//    [SerializeField] private GameState gameState;

//    private CountHandler countHandler;
//    private SaverLoader saverLoader;
//    private GameSaveData gameSaveData;
//    private Config config;

//    private void Awake()
//    {
//        config = GameConfigProvider.instance.GameConfig;
//        InitializeGame();
//    }

//    private void OnDisable()
//    {
//        UnsubscribeEvents();
//    }

//    public void OnApplicationQuit() => SaveData();
//    public void OnApplicationPause(bool pause)
//    {
//        if (pause) SaveData();
//    }

//    private void RestartGame()
//    {
//        Debug.Log("Bootstrap restarting game...");
//        saverLoader.DeleteSaveFile();
//        UnsubscribeEvents();
//        InitializeGame();
//    }

//    public void InitializeGame()
//    {
//        Debug.Log("Initializing game...");
//        AssignDependencies();
//        if (gameSaveData != null)
//        {
//            Debug.Log("Initializing from saved data...");
//            InitialFromSaveData();
//        }
//        else
//        {
//            Debug.Log("Initializing from config...");
//            InitialFromConfig();
//        }
//        SubscribeEvents();
//        Debug.Log("Initialization complete.");
//    }

//    private void SaveData()
//    {
//        Debug.Log("Saving game data...");
//        GameSaveData saveData = new GameSaveData
//        {
//            shovels = countHandler.GetRemainingShovels(),
//            collectedGold = countHandler.GetCollectedRewards(),
//            cells = gridFill.GetCellsData()
//        };

//        saverLoader.SaveGame(saveData);
//    }

//    private void InitialFromConfig()
//    {
//        countHandler.Initialize(config.initialShovelCount, config.requiredGoldBars);
//        InitializeCommon();
//        gridFill.InitializeGrid(config.fieldSize);
//    }

//    private void InitialFromSaveData()
//    {
//        countHandler.Initialize(gameSaveData.shovels, config.requiredGoldBars, gameSaveData.collectedGold);
//        InitializeCommon();
//        gridFill.InitializeGridFromData(gameSaveData.cells, rewardManager);
//        gridFill.DataGridSpawnGold(rewardManager);
//    }

//    /// <summary>
//    /// Общая инициализация элементов игры, не зависящая от конфигурации или сохраненных данных.
//    /// </summary>
//    private void InitializeCommon()
//    {
//        rewardManager.Initialize(config.goldSpawnChance, config.goldSpawnChanceIncrement, goldPrefab);
//        gameState.Initialize(winMenu, countHandler);
//        gridFill.Initialize(gridParent, cellPrefab, config.maxDepth, countHandler);
//    }

//    private void AssignDependencies()
//    {
//        countHandler ??= GetComponent<CountHandler>();
//        view ??= GetComponent<View>();
//        rewardManager ??= GetComponent<RewardManager>();
//        bag ??= FindObjectOfType<UIBag>();
//        gameState ??= GetComponent<GameState>();
//        saverLoader ??= GetComponent<SaverLoader>();
//        gameSaveData = saverLoader.LoadGame();
//        Debug.Log($"GameSaveData loaded: {gameSaveData != null}");
//    }

//    /// <summary>
//    /// Подписка/отписка на события в зависимости от флага.
//    /// </summary>
//    private void ConfigureEvents(bool subscribe)
//    {
//        if (subscribe)
//        {
//            rewardManager.SubscribeToCellEvents(gridFill.GetCells());
//            countHandler.OnAllRewardCollected += gameState.Win;
//            countHandler.OnShovelCountChanged += view.UpdateShovelCount;
//            countHandler.OnRewardCountChanged += view.UpdateRewardCount;
//            bag.OnGoldAddedToBag += countHandler.CollectReward;
//            gameState.OnGameRestart += RestartGame;
//            countHandler.UpdateView();
//        }
//        else
//        {
//            countHandler.OnAllRewardCollected -= gameState.Win;
//            countHandler.OnShovelCountChanged -= view.UpdateShovelCount;
//            countHandler.OnRewardCountChanged -= view.UpdateRewardCount;
//            bag.OnGoldAddedToBag -= countHandler.CollectReward;
//            gameState.OnGameRestart -= RestartGame;
//        }
//    }

//    private void SubscribeEvents() => ConfigureEvents(true);
//    private void UnsubscribeEvents() => ConfigureEvents(false);
//}
