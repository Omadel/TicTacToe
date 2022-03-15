using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TicTacToe
{
    public class PlayerAI : MonoBehaviour
    {
        [SerializeField] private int _PlayerIndex=1;
        private void Awake()
        {
            GameManager.Instance.OnNewTurn += Play;
            GameManager.Instance.OnWin += Diable;
        }

        private void Diable(int player)
        {
            GameManager.Instance.OnNewTurn -= Play;
        }

        private async void Play(int playerIndex)
        {
            await Task.Delay(20);
            if(playerIndex != _PlayerIndex) return;
            GameManager gm = GameManager.Instance;
            Board board = gm.Board;
            gm.PlayTurn(FindBestMove(board.Tiles));
        }

        private Vector2 FindBestMove(Dictionary<Vector2, Tile> tiles)
        {
            Dictionary<Vector2, int> board = new Dictionary<Vector2, int>();
            List<Vector2> freeTiles = new List<Vector2>();
            foreach(KeyValuePair<Vector2, Tile> tile in tiles)
            {
                board.Add(tile.Key, tile.Value.PlayerControl);
                if(tile.Value.PlayerControl == 0) freeTiles.Add(tile.Key);
            }

            Move[] Winmoves = GetBoardChildren(board, freeTiles, 2);
            Move bestWinMove = GetBestMove(Winmoves);
            if(bestWinMove > 0) return bestWinMove.MovePosition;

            Move[] moves = GetBoardChildren(board, freeTiles, 1);
            Move bestMove = GetBestMove(moves);
            if(bestMove > 0) return bestMove.MovePosition;

            return freeTiles[Random.Range(0, freeTiles.Count)];
        }

        private Move[] GetBoardChildren(Dictionary<Vector2, int> board, List<Vector2> freeTiles, int currentPlayer)
        {
            Move[] children = new Move[freeTiles.Count];
            for(int i = 0; i < freeTiles.Count; i++)
            {
                Dictionary<Vector2, int> boardCopy = board.ToDictionary(entry => entry.Key, entry => entry.Value);
                boardCopy[freeTiles[i]] = currentPlayer;
                bool won = GameManager.Instance.Board.CheckWin(currentPlayer, boardCopy);
                children[i] = new Move(won ? currentPlayer : 0);
                children[i].Board = boardCopy;
                children[i].MovePosition = freeTiles[i];
            }
            return children;
        }

        private Move GetBestMove(Move[] moves)
        {
            Move bestMove = new Move(-10);
            foreach(Move move in moves)
            {
                if(move < bestMove) continue;
                bestMove = move;
            }
            return bestMove;
        }

    }

    internal class Move
    {
        public Move() { }
        public Move(int value)
        {
            Value = value;
        }

        public Dictionary<Vector2, int> Board { get; set; }
        public Vector2 MovePosition { get; set; }
        public int Value { get; set; }

        public static implicit operator int(Move move)
        {
            return move.Value;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine($"won: {Value == 1}, Move: {MovePosition}");
            for(int y = 3 - 1; y >= 0; y--)
            {
                s.AppendLine($"{Board[new Vector2(0, y)]}|{Board[new Vector2(1, y)]}|{Board[new Vector2(2, y)]}");
            }
            return s.ToString();
        }
    }

}
