using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
    public class Board : MonoBehaviour
    {
        public const int Size = 3;
        public Dictionary<Vector2, Tile> Tiles => _Tiles;

        [SerializeField] private GameObject _TilePrefab;
        private Dictionary<Vector2, Tile> _Tiles = new Dictionary<Vector2, Tile>();


        private void Awake()
        {
            for(int x = 0; x < Size; x++)
            {
                for(int y = 0; y < Size; y++)
                {
                    GameObject go = GameObject.Instantiate(_TilePrefab, transform);
                    go.transform.position = new Vector3(x, 0, y);
                    _Tiles.Add(new Vector2(x, y), go.GetComponent<Tile>());
                }
            }
            Camera.main.transform.position = new Vector3(Size / 4f, 10, Size / 4f);
        }

        public void PlayTile(Vector2 position,int playerIndex = 2)
        {
            if(!IsTileFree(position)) return;
            _Tiles[position].SetPlayerControl(playerIndex);
        }

        public bool IsTileFree(Vector2 position)
        {
            return _Tiles[position].PlayerControl == 0;
        }

        public int GetTileControl(Vector3 position)
        {
            return _Tiles[position].PlayerControl;
        }

        public bool CheckWin(int playerIndex)
        {
            return CheckWin(playerIndex, TilesToTilesInt(_Tiles));
        }

        public bool CheckWin(int playerIndex, Dictionary<Vector2, int> tiles)
        {
            if(CheckRow(playerIndex, tiles)) return true;
            if(CheckCol(playerIndex, tiles)) return true;
            if(CheckDiag(playerIndex, tiles)) return true;

            return false;
        }

        private bool CheckRow(int playerIndex, Dictionary<Vector2, int> tiles)
        {
            for(int x = 0; x < Size; x++)
            {
                int colCount = 0;
                for(int y = 0; y < Size; y++)
                {
                    if(tiles[new Vector2(x, y)] != playerIndex) break;
                    colCount++;
                    if(colCount >= 3) return true;
                }
            }
            return false;
        }

        private bool CheckCol(int playerIndex, Dictionary<Vector2, int> tiles)
        {
            for(int y = 0; y < Size; y++)
            {
                int rowCount = 0;
                for(int x = 0; x < Size; x++)
                {
                    if(tiles[new Vector2(x, y)] != playerIndex) break;
                    rowCount++;
                    if(rowCount >= 3) return true;
                }
            }
            return false;
        }

        private bool CheckDiag(int playerIndex, Dictionary<Vector2, int> tiles)
        {
            int diagCount = 0;
            for(int i = 0; i < Size; i++)
            {
                if(tiles[new Vector2(i, i)] != playerIndex) break;
                diagCount++;
                if(diagCount >= 3) return true;
            }
            diagCount = 0;
            for(int i = 0; i < Size; i++)
            {
                if(tiles[new Vector2(i, Size-1-i)] != playerIndex) break;
                diagCount++;
                if(diagCount >= 3) return true;
            }
            return false;
        }

        private Dictionary<Vector2, int> TilesToTilesInt(Dictionary<Vector2, Tile> tiles)
        {
            Dictionary<Vector2, int> tilesInt = new Dictionary<Vector2, int>();
            foreach(KeyValuePair<Vector2, Tile> tile in tiles) tilesInt.Add(tile.Key, tile.Value.PlayerControl);
            return tilesInt;
        }
    }
}