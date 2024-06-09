using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button readyButton;
    private NetPlayerObject localPlayer;
    private bool isReady = false; // �÷��̾ �غ� �������� ����

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
            localPlayer.CmdSetReady(true); // �÷��̾ �غ� �������� ������ �˸�

            // ��ư ���� ����
            ColorBlock colors = readyButton.colors;
            if (isReady)
            {
                colors.normalColor = Color.green; // �غ� ������ �� ����
                colors.selectedColor = Color.green;
            }
            else
            {
                colors.normalColor = Color.white; // �غ� ���� ������ �� ����
                colors.selectedColor = Color.white;
            }
            readyButton.colors = colors;
        }
    }
}