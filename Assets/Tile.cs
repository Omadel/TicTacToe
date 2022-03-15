using UnityEngine;

namespace TicTacToe
{
    public class Tile : MonoBehaviour
    {
        public int PlayerControl => _PlayerControl;

        [SerializeField] private int _PlayerControl;

        private void Awake()
        {
            GameManager.Instance.OnWin += (_) => _PlayerControl = -1;
        }


        private void OnMouseUpAsButton()
        {
            if(GameManager.Instance.CurrentPlayer != 1 || _PlayerControl != 0) return;
            GameManager.Instance.PlayTurn(new Vector2(transform.position.x, transform.position.z));
        }

        public void SetPlayerControl(int playerIndex)
        {
            GetComponent<MeshRenderer>().material.color = playerIndex == 1 ? Color.green : Color.red;
            _PlayerControl = playerIndex;
        }
    }
}
