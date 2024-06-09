using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    public Button readyButton;
    public Text countdownText; 
    public Text questionText;
    public Text answerText;
    public Text scoreText;
    private NetPlayerObject localPlayer;
    private bool isReady = false; // �÷��̾ �غ� �������� ����

    private Dictionary<NetPlayerObject, Text> playerScores = new Dictionary<NetPlayerObject, Text>();

    private void Update()
    {
        if (localPlayer == null)
        {
            localPlayer = FindObjectOfType<NetPlayerObject>();
        }
    }

    public void OnReadyButtonClicked()
    {
       
        if (localPlayer != null)
        {
            isReady = !isReady;
            //localPlayer.CmdSetReady(true); // �÷��̾ �غ� �������� ������ �˸�

            // ��ư ���� ����
            ColorBlock colors = readyButton.colors;
            if (isReady)
            {
                colors.normalColor = Color.green; // �غ� ������ �� ����
                colors.selectedColor = Color.green;
                localPlayer.CmdSetReady(true);
            }
            else
            {
                colors.normalColor = Color.white; // �غ� ���� ������ �� ����
                colors.selectedColor = Color.white;
                localPlayer.CmdSetReady(false);
            }
            readyButton.colors = colors;
        }
    }
    public void UpdateScoreText(NetPlayerObject player)
    {
        if (player == localPlayer)
        {
            scoreText.text = "Score: " + player.score;
        }
    }
}