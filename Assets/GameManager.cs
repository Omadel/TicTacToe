using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
        public Action  OnDraw;
        public Action<int> OnNewTurn, OnWin;
        public static GameManager Instance => _Instance;
        public Board Board => _Board;
        public int CurrentPlayer = 1;
        
        [SerializeField] private Board _Board;

        [SerializeField] private int _CurrentTurn;

        private static GameManager _Instance;

        private void Awake()
        {
            if(_Instance != null)
            {
                Debug.LogWarning("Already an instance", _Instance);
            }

            _Instance = this;
        }

        private void Start()
        {
            OnNewTurn?.Invoke(CurrentPlayer);
        }

        public void PlayTurn(Vector2 position)
        {
            _Board.PlayTile(position, CurrentPlayer);
            bool win = _Board.CheckWin(CurrentPlayer);
            if(win)
            {
                OnWin?.Invoke(CurrentPlayer);
                Debug.Log($"Player {CurrentPlayer} wins");
                return;
            }
            CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
            _CurrentTurn++;
            if(_CurrentTurn>=Board.Size * Board.Size)
            {
                OnDraw?.Invoke();
                return;
            }
            OnNewTurn?.Invoke(CurrentPlayer);
        }
    }
}
