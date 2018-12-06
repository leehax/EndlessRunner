using UnityEngine;


public class GameSettings : GenericSingleton<GameSettings>
{
    protected override void OnAwake()
    {
        m_persistent = false;

    }

#region GameState
    public enum GameStates
    {
        Paused,
        Playing,
        GameOver
    }

    private GameStates m_gameState = GameStates.Paused;

    public GameStates GameState()
    {
        return m_gameState;
    }

    public void EndGame()
    {
        m_gameState = GameStates.GameOver;
        OnGameEnd();
    }

    public void StartGame()
    {
        m_gameState = GameStates.Playing;
    }

    public void PauseGame()
    {
        m_gameState = GameStates.Paused;
    }

    public delegate void StateChange();

    public static event StateChange OnGameEnd;

#endregion

#region GameProperties
    private float m_distanceBetweenPlatforms = 32f;
    private Vector3 m_tileScale = new Vector3(1.5f, 1f, 5f);
    private Vector3 m_wallScale = new Vector3(1f, 4f, 32f);

    public Vector3 WallScale()
    {
        return m_wallScale;
    }
    public Vector3 TileScale()
    {
        return m_tileScale;
    }
    public float DistanceBetweenPlatforms()
    {
        return m_distanceBetweenPlatforms;
    }
#endregion

#region Score
    private int m_score = 0;


    public int Score()
    {
        return m_score;
    }

    public void AddScore(int amount)
    {
        m_score += amount;

        if (m_score > PlayerPrefs.GetInt("HighScore",0))
        {
            PlayerPrefs.SetInt("HighScore",m_score);
        }
    }

#endregion






}