using UnityEngine;
using UnityEngine.UI;

public class ScoreHelper : MonoBehaviour
{

    [SerializeField] private Text m_scoreText;
    [SerializeField] private Text m_highScoreText;
    [SerializeField] private bool m_displayValueOnly = true;
    private int Score()
    {
        return GameSettings.Instance().Score();
    }

    private void DisplayScore()
    {
        if (m_scoreText == null)
        {
            return;
        }
        if (m_displayValueOnly)
        {
            m_scoreText.text = Score().ToString();
            return;
        }

        m_scoreText.text = "Score : " + Score().ToString();

    }

    private void DisplayHighScore()
    {
        if (m_highScoreText == null)
        {
            return;
        }

        if (m_displayValueOnly)
        {
            m_highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
            return;
        }
        m_highScoreText.text = "High Score : " + PlayerPrefs.GetInt("HighScore").ToString();
    }

    private void Update()
    {
        if(m_scoreText != null)
            DisplayScore();

        if(m_highScoreText != null)
            DisplayHighScore();
    }
}
