using System;

namespace tictoctoe
{
    class Program
    {
        static void Main(string[] args)
        {
            var stillplaying = true;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Tic Tac Toe' ya Hoşgeldiniz!");
            Console.ResetColor();

            while (stillplaying)
            {
                Console.WriteLine("Ne yapmak istersiniz?");
                Console.WriteLine("1.Yeni oyun başlat");
                Console.WriteLine("2.Çıkış yap");

                Console.WriteLine("Numara seçip <enter> 'a tıklayın");

                var secim = GetUserInput("[12]");

                switch (secim)
                {
                    case "1":
                        PlayGame();
                        Console.Clear();
                        break;
                    case "2":
                        stillplaying = false;
                        break;
                }
            }
        }

        private static void PlayGame()
        {
            string? numRowsChoice = "3";
            //while (numRowsChoice == null)
            //{
            //    Console.Write("Kaç tane satıra sahip olmak istiyorsunuz? (3,4 ya da 5)");
            //    numRowsChoice = GetUserInput("[3-5]");
            //}
            var boardsize = (int)Math.Pow(int.Parse(numRowsChoice), 2);
            var board = new string[boardsize];

            var turn = "X";
            while (true)
            {
                Console.Clear();

                var winner = WhoWins(board);
                if (winner != null)
                {
                    AnnounceResult(winner[0] + " Kazandı!", board);
                    break;
                }
                if (isBoardFull(board))
                {
                    AnnounceResult("Eşitlik gerçekleşti", board);
                    break;
                }

                Console.WriteLine(turn + " yerleştiriniz:");

                DrawBoard(board);

                Console.WriteLine("Ok tuşlarını kullanın ve <enter>'a basın");

                var xoLoc = GetXOLocation(board);
                board[xoLoc] = turn;

                turn = turn == "X" ? "O" : "X";


            }
        }

        private static int GetXOLocation(string[] board)
        {
            int numRows = (int)Math.Sqrt(board.Length);
            int currentrow = 0;
            int currentcol = 0;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == null) //tahtada boş yerin satır ve sütunları belirleniyor
                {
                    currentrow = i / numRows;
                    currentcol = i % numRows;
                    break;
                }
            }

            while (true)
            {
                Console.SetCursorPosition(currentcol * 4 + 2, currentrow * 4 + 3);
                var keyinfo = Console.ReadKey();
                Console.SetCursorPosition(currentcol * 4 + 2, currentrow * 4 + 3);
                Console.Write(board[currentrow * numRows + currentcol] ?? " ");

                switch (keyinfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (currentcol > 0)
                            currentcol--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentcol < numRows - 1)
                            currentcol++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (currentrow > 0)
                            currentrow--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentrow < numRows - 1)
                            currentrow++;
                        break;
                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        if (board[currentrow * numRows + currentcol] == null)
                            return currentrow * numRows + currentcol;
                        break;
                }
            }

        }

        private static void DrawBoard(string[] board) // tahtayı çizelim
        {
            int numRows = (int)Math.Sqrt(board.Length);

            Console.WriteLine();

            for (int row = 0; row < numRows; row++)
            {
                if (row != 0)
                    Console.WriteLine(" " + string.Join("|", Enumerable.Repeat("---", numRows)));

                Console.Write(" " + string.Join("|", Enumerable.Repeat("   ", numRows)) + "\n ");

                for (int col = 0; col < numRows; col++)
                {
                    if (col != 0)
                        Console.Write("|");
                    var space = board[row * numRows + col] ?? " ";
                    if (space.Length > 1) // xox varsa yani (X!).length > 1 (2)
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(" " + space[0] + " ");
                    Console.ResetColor();
                }

                Console.WriteLine("\n " + string.Join("|", Enumerable.Repeat("   ", numRows)));
            }

            Console.WriteLine();
        }

        private static bool isBoardFull(IEnumerable<string> board)
        {
            return board.All(space => space != null);
        }

        private static void AnnounceResult(string message, string[] board)
        {
            DrawBoard(board);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.Write("devam etmek için herhangi bir tuşa basın");
            Console.CursorVisible = false;
            Console.ReadKey();
            Console.CursorVisible = true;
        }

        private static string? WhoWins(string[] board)
        {
            var numRows = (int)Math.Sqrt(board.Length);

            //satırları kontrol edelim
            for (int row = 0; row < numRows; row++)
            {
                if (board[row * numRows] != null)
                {
                    bool hastictactoe = true;
                    for (int col = 1; col < numRows && hastictactoe; col++)
                    {
                        if (board[row * numRows + col] != board[row * numRows])
                            hastictactoe = false;
                    }
                    if (hastictactoe)
                    {
                        for (int col = 1; row < numRows; row++)
                            board[row * numRows + col] += "!"; //xox olan yerleri belirtiyor (X! O!)
                        return board[row * numRows];
                    }
                }
            }

            //sütunları kontrol edelim
            for (int col = 0; col < numRows; col++)
            {
                if (board[col] != null)
                {
                    bool hastictactoe = true;
                    for (int row = 1; row < numRows && hastictactoe; row++)
                    {
                        if (board[row * numRows + col] != board[col])
                            hastictactoe = false;
                    }
                    if (hastictactoe)
                    {
                        for (int row = 0; row < numRows; row++)
                            board[(row * numRows) + col] += "!";
                        return board[col];
                    }
                }
            }

            //sol yukardan sağ aşağı çapraz kontrol edelim
            if (board[0] != null)
            {
                bool hasTicTacToe = true;
                for (int row = 1; row < numRows && hasTicTacToe; row++)
                {
                    if (board[row * numRows + row] != board[0])
                        hasTicTacToe = false;
                }
                if (hasTicTacToe)
                {
                    for (int row = 0; row < numRows; row++)
                        board[row * numRows + row] += "!";
                    return board[0];
                }
            }
            // sağ yukardan sol aşağı çapraz kontrol edelim
            if (board[numRows - 1] != null)
            {
                bool hastictactoe = true;
                for (int row = 2; row <= numRows && hastictactoe; row++)
                {
                    if (board[numRows - 1] != board[row * (numRows - 1)])
                        hastictactoe = false;
                }
                if (hastictactoe)
                {
                    for (int row = 1; row < numRows; row++)
                        board[row * (numRows - 1)] += "!";
                    return board[numRows - 1];
                }
            }

            return null;
        }

        private static string? GetUserInput(string? validpattern = null)
        {
            var input = Console.ReadLine();
            input = input.Trim(); //baş ve sondaki boşluk karakterlerini kaldırır (trim)

            if (validpattern != null && !System.Text.RegularExpressions.Regex.IsMatch(input, validpattern)) // [12] bir ya da iki değerinden herhangi biri eşleşiyor mu 
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("");
                Console.ResetColor();
                return null;
            }
            return input;
        }
    }
}