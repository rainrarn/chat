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
    private bool isReady = false; // 플레이어가 준비 상태인지 여부

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
            //localPlayer.CmdSetReady(true); // 플레이어가 준비 상태임을 서버에 알림

            // 버튼 색상 변경
            ColorBlock colors = readyButton.colors;
            if (isReady)
            {
                colors.normalColor = Color.green; // 준비 상태일 때 색상
                colors.selectedColor = Color.green;
                localPlayer.CmdSetReady(true);
            }
            else
            {
                colors.normalColor = Color.white; // 준비 해제 상태일 때 색상
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