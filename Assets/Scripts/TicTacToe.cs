using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGame : MonoBehaviour
{
    public GameObject xPrefab;
    public GameObject oPrefab;

    public Transform[] squares = new Transform[9];

    // 0 = empty, 1 = your X, 2 = computer O
    private int[] board = new int[9];

    private bool computerThinking = false;
    private bool gameOver = false;

    private int[,] winningLines =
    {
        { 0, 1, 2 },
        { 3, 4, 5 },
        { 6, 7, 8 },
        { 0, 3, 6 },
        { 1, 4, 7 },
        { 2, 5, 8 },
        { 0, 4, 8 },
        { 2, 4, 6 }
    };

    public void PlaySquare(int index)
    {
        if (gameOver || computerThinking)
            return;

        if (index < 0 || index >= board.Length)
            return;

        if (board[index] != 0)
            return;

        PlacePiece(index, 1);

        if (CheckGameEnd(1))
            return;

        computerThinking = true;
        StartCoroutine(ComputerTurn());
    }

    private IEnumerator ComputerTurn()
    {
        yield return new WaitForSeconds(0.5f);

        List<int> emptySquares = new List<int>();

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
                emptySquares.Add(i);
        }

        int randomPosition = Random.Range(0, emptySquares.Count);
        int chosenSquare = emptySquares[randomPosition];

        PlacePiece(chosenSquare, 2);

        computerThinking = false;
        CheckGameEnd(2);
    }

    private void PlacePiece(int index, int player)
    {
        board[index] = player;

        GameObject prefab = player == 1 ? xPrefab : oPrefab;

        GameObject piece = Instantiate(
            prefab,
            squares[index],
            false
        );

        piece.transform.localPosition = Vector3.zero;

        // Keep the rotation and scale saved in the prefab.
        piece.transform.localRotation = prefab.transform.localRotation;
        piece.transform.localScale = prefab.transform.localScale;
    }

    private bool CheckGameEnd(int player)
    {
        for (int line = 0; line < 8; line++)
        {
            int a = winningLines[line, 0];
            int b = winningLines[line, 1];
            int c = winningLines[line, 2];

            if (board[a] == player &&
                board[b] == player &&
                board[c] == player)
            {
                gameOver = true;
                return true;
            }
        }

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
                return false;
        }

        gameOver = true;
        return true;
    }
    [ContextMenu("Test Square 0")]
    private void TestSquare0()
    {
        PlaySquare(0);
    }
}