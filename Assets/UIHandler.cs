using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class UIHandler : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.OnWin += (int player) =>
            {
                gameObject.SetActive(true);
                Text text = GetComponentInChildren<Text>();
                text.text = "Win";
                text.color = player == 1 ? Color.green : Color.red;
            };
            GameManager.Instance.OnDraw += () =>
            {
                gameObject.SetActive(true);
                GetComponentInChildren<Text>().text = "Draw";
            };
            gameObject.SetActive(false);
        }
    }
}
