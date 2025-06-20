using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject _entitiesHolder;

    [Header("Spawn Points")]
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;

    public Player Player { get; set; }
    public Enemy Enemy { get; set; }
    private LevelData _levelData;
    private BoardManager _boardManager;
    protected override void Awake()
    {
        base.Awake();

        _boardManager = GetComponentInChildren<BoardManager>();
    }
    private void Start()
    {
        _boardManager.InitializeEmpty();
        _boardManager.InitializeTiles();
        InitializeData();
    }

    public void InitializeData()
    {
        if (!GameManager.Instance.CurrentLevelData) return;
        _levelData = GameManager.Instance.CurrentLevelData;

        InitializeEntities();
    }

    public void InitializeEntities()
    {
        var playerPath = $"Entities/{_levelData.levelConfig.playerType}";
        var enemyPath = $"Entities/{_levelData.levelConfig.enemyType}";
        
        var playerPrefab = LoadManager.PrefabLoad<Player>(playerPath);
        var enemyPrefab = LoadManager.PrefabLoad<Enemy>(enemyPath);

        var playerData = LoadManager.DataLoad<EntityData>(playerPath);
        var enemyData = LoadManager.DataLoad<EntityData>(enemyPath);


        if (!playerPrefab || !enemyPrefab) return;

        Player = Instantiate(playerPrefab, _playerSpawnPoint.position, Quaternion.identity, _entitiesHolder.transform);
        Enemy = Instantiate(enemyPrefab, _enemySpawnPoint.position, Quaternion.identity, _entitiesHolder.transform);

        Player.GetData(playerData);
        Enemy.GetData(enemyData);
    }

    public void PlayerAction()
    {
        Player.Attack(Enemy);
    }
}
