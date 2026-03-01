using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Coins")]
    public int coins = 0;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI bestCoinsText;

    [Header("Distance")]
    [SerializeField] private Transform player;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI bestDistanceText;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    private bool isGameOver;

    private float startX;
    private int currentDistance;
    private int bestDistance;
    private int bestCoins;

    public Transform Player => player;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        startX = player.position.x;

        bestDistance = PlayerPrefs.GetInt("BestDistance", 0);
        bestCoins = PlayerPrefs.GetInt("BestCoins", 0);
    }

    private void Update()
    {
        if (isGameOver || player == null) return;

        currentDistance = Mathf.Max(0,
            Mathf.FloorToInt(player.position.x - startX));

        distanceText.text = $"Distance: {currentDistance}m";
    }

    public void AddCoin()
    {
        coins++;
        coinText.text = $"Coins: {coins}";
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (currentDistance > bestDistance)
        {
            bestDistance = currentDistance;
            PlayerPrefs.SetInt("BestDistance", bestDistance);
        }

        if (coins > bestCoins)
        {
            bestCoins = coins;
            PlayerPrefs.SetInt("BestCoins", bestCoins);
        }

        PlayerPrefs.Save();

        bestDistanceText.text = $"Best Distance: {bestDistance}m";
        bestCoinsText.text = $"Best Coins: {bestCoins}";

        coinText.gameObject.SetActive(false);
        distanceText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}