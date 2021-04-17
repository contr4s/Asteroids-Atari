using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: Singleton<GameManager>
{
    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            if (_score < 0)
                Debug.LogError("score can't be less than 0");
            _score = value;
            mangerUI.UpdateScoreBoard(string.Format("Score: {0}", _score));
        }
    }

    private int _curLevel = 0;
    public int CurLevel
    {
        get => _curLevel;
        private set
        {
            _curLevel = value;
            mangerUI.UpdateLevelBoard(string.Format("Level: {0}", _curLevel));
        }
    }

    [Header("Set in Inspector")]
    public AsteroidsScriptableObject asteroidsSO;

    public UIManager mangerUI;
    public AudioManager audioManager;
    public AsteroidsManager asteroidsManager;
    public PoliceShipsManager policeShipsManager;

    public ExplosionsPool explosionsPool;

    [SerializeField]
    private float _timeBetweenSpawningPoliceShips = 1f;

    [SerializeField]
    private int _startAsteroidsAmount;
    [SerializeField]
    private int _startPoliceShipAmount;

    private const float _minDistToPlayerShip = 5;

    private void Start()
    {
        StartCoroutine(StartGame(mangerUI.TimeOfShowingPreGamePanel));
    }

    public static Vector3 FindGoodLocation()
    {
        Vector3 pos = ScreenBounds.randomOnScreenLoc;
        while ((pos - PlayerShip.position).magnitude < _minDistToPlayerShip)
        {
            pos = ScreenBounds.randomOnScreenLoc;
        }
        return pos;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        mangerUI.GameOverUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    private IEnumerator StartGame(float timeOfShowingPreGamePanel)
    {
        yield return new WaitForSeconds(timeOfShowingPreGamePanel);
        mangerUI.PregameUI.SetActive(false);
        StartLevel();
    }

    private void StartLevel()
    {
        for (int i = 0; i < _curLevel + _startAsteroidsAmount; i++)
        {
            asteroidsManager.SpawnParentAsteroid();
        }

        StartCoroutine(policeShipsManager.SpawnPoliceShips(_startPoliceShipAmount + CurLevel, _timeBetweenSpawningPoliceShips));
    }


}
