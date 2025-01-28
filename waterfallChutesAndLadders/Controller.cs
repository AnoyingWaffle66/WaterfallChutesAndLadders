namespace waterfallChutesAndLadders;

public class Controller
{
    public Controller()
    {
        players = new List<Player>();
        game = new Game();
    }

    private List<Player> players;
    private Game game;

    public void start()
    {
        int player_count = 2;
        bool exit = false;
        while (!exit)
        {
            players = new List<Player>();
            game = new Game();
            do
            {
                Console.Clear();
                Console.WriteLine("Welcome to Chutes and Ladders!\n\nPlayers will take turns rolling one 6 sided die and move forward that many spaces\nif you land on a tile with a 'C' you will go down the board to the number next to the 'C'\nif you land on a tile with an 'L' you will go up the board to the number next to the 'L'\n");
                Console.WriteLine("Enter player count (2-4)");
            } while (handle_input(Console.ReadLine(), ref player_count, integer_prompt: true, min: 2, max: 4));

            Console.WriteLine(player_count);

            for (int i = 0; i < player_count; i++)
            {
                players.Add(new Player());
            }
            int player_counter = 0;
            int player_roll = 0;
            for (int turn_counter = 0; !game.check_won(players[player_counter]); turn_counter++)
            {
                player_counter = turn_counter % player_count;
                int previous_player = (player_counter - 1 + player_count) % player_count + 1;
                do
                {
                    draw();
                    if (turn_counter != 0)
                    {
                        Console.WriteLine("Player " + (previous_player) + " rolled: " + player_roll);
                    }
                    Console.WriteLine("Player " + (player_counter + 1) + " press enter to roll");
                } while (handle_input(Console.ReadLine(), ref player_count, playing: true));
                player_roll = Dice.roll();
                game.turn(players[player_counter], player_roll);
            }
            draw();
            Console.WriteLine("Player " + (player_counter + 1) + " has won");
            Console.WriteLine("Press enter to continue");
            while (handle_input(Console.ReadLine(), ref player_count, playing: true)) { }
            Console.Clear();
            int menu_choice = 0;
            bool menu_selected = false;
            while (!menu_selected)
            {
                Console.WriteLine("\nPress\n1) To go back to main menu\n2) Exit");
                handle_input(Console.ReadLine(), ref menu_choice, integer_prompt: true, min: 1, max: 3);

                switch (menu_choice)
                {
                    case 1:
                        menu_selected = true;
                        break;
                    case 2:
                        menu_selected = true;
                        exit = true;
                        break;
                    default:
                        menu_selected = false;
                        break;
                }
                Console.Clear();
            }
        }
    }

    public void draw()
    {
        Console.Clear();

        // Displays in plaintext the players and their positions, in their relevant color.
        Console.Write("Positions: ");
        for (int i = 0; i < players.Count; i++)
        {
            Console.Write(
                          $"Player {i + 1} position: {players[i].Position + 1}");
            if (i < players.Count - 1) Console.Write(", ");
        }
        Console.WriteLine("\n");

        // Get the boards and tiles.
        Tile[,] board = game.Board;
        int width = game.Board_width;

        // Print the top and bottoms of each rows. This needs to be adjusted
        // if the totalInnerWidth of the MakeCell function changes.
        void PrintHorizontalBoundary()
        {
            for (int col = 0; col < width; col++)
            {
                Console.Write("---------");
            }
            Console.WriteLine("-");
        }

        /* This helper method produces a fixed-width cell ignoring ANSI color codes
           for length calculations, so columns stay aligned. */
        string MakeCell(string content, int totalInnerWidth = 6)
        {
            // Strip ANSI codes for measuring visible length.
            string noColor = System.Text.RegularExpressions.Regex
                .Replace(content, @"\u001b\[[0-9;]*m", "");

            // If the visible text is longer than totalInnerWidth, truncate.
            if (noColor.Length > totalInnerWidth)
            {
                // Truncate just the visible text.
                noColor = noColor.Substring(0, totalInnerWidth);
                return noColor;
            }
            else
            {
                // If it fits, keep the original (with color) and pad with spaces.
                int padCount = totalInnerWidth - noColor.Length;
                return content + new string(' ', padCount);
            }
        }

        // Prints out the top row boundary of the board.
        PrintHorizontalBoundary();

        // This is the part that allows the board to "snake" while printing
        // out all the rows. It essentially goes back and forth after each ten
        // tiles and reverses the direction of the printing using modulus.
        for (int row = 9; row >= 0; row--)
        {
            int bottomRowIndex = 9 - row;

            int[] columns;
            if (bottomRowIndex % 2 == 1)
            {
                // Odd: Left to right.
                columns = new int[width];
                for (int c = 0; c < width; c++) columns[c] = c;
            }
            else
            {
                // Even: Right to left.
                columns = new int[width];
                for (int c = 0; c < width; c++) columns[c] = width - 1 - c;
            }

            // Print one interior line at a time.
            for (int col = 0; col < columns.Length; col++)
            {
                string content = "";

                /* Check for players on this tile, if so, add player and color to string list. */
                List<string> playersHere = new List<string>();
                for (int p = 0; p < players.Count; p++)
                {
                    if (players[p].Position == row * 10 + columns[col])
                    {
                        playersHere.Add( "P");
                    }
                }

                // Append the players if present.
                if (playersHere.Count > 0)
                {
                    content = string.Join("", playersHere);
                }

                // Get the text for the text ignoring the ANSI color stuff.
                string cellText = MakeCell(content);

                // Print box interior, for example: "| PP "
                Console.Write("| " + cellText + " ");
            }
            Console.WriteLine("|");
            for (int col = 0; col < columns.Length; col++)
            {
                int idx = row * 10 + columns[col];
                Tile tile = board[row, columns[col]];
                string content = "" + (idx + 1);
                if (tile.Tile_type == TileType.CHUTE)
                {
                    content += idx == tile.Activate_position ? " C" + (tile.Go_to_position + 1) : ""/*" c" + (tile.Activate_position + 1)*/;
                }
                else if (tile.Tile_type == TileType.LADDER)
                {
                    content += idx == tile.Activate_position ? " L" + (tile.Go_to_position + 1) : ""/*" l" + (tile.Activate_position + 1)*/;
                }
                Console.Write("|" + MakeCell(content) + "  ");
            }
            // Right boundary for this row.
            Console.WriteLine("|");

            // Bottom boundary of this row.
            PrintHorizontalBoundary();
        }

        // Should be fully displayed.
    }


    public bool handle_input(string input, ref int integer_ref, bool playing = false, bool integer_prompt = false, int min = 0, int max = 1)
    {
        if (playing)
        {
            return !string.IsNullOrEmpty(input);
        }

        if (integer_prompt)
        {
            if (!int.TryParse(input, out integer_ref) || integer_ref > max || integer_ref < min)
            {
                Console.WriteLine("Bad input");
                return true;
            }
        }

        return false;
    }

    public static Tile[,] generate_board()
    {
        Player tester;
        Tile[,] board_to_return;
        do
        {

            const int board_width = 10;
            board_to_return = new Tile[board_width, board_width];
            for (int row = 0; row < board_width; row++)
            {
                for (int column = 0; column < board_width; column++)
                {
                    if (row == 0 && column == 0)
                    {
                        board_to_return[row, column] = new Tile(TileType.BLANK, 0, 0);
                        continue;
                    }
                    if (row == 9 && column == 9)
                    {
                        board_to_return[row, column] = new Tile(TileType.BLANK, 99, 99);
                        continue;
                    }
                    TileType type = TileType.BLANK;
                    switch (Dice.roll(1, 30))
                    {
                        case 1:
                        case 4:
                            type = TileType.CHUTE;
                            break;
                        case 2:
                        case 3:
                        case 5:
                            type = TileType.LADDER;
                            break;
                        default:
                            type = TileType.BLANK;
                            break;
                    }
                    int current_idx = row * board_width + column;
                    Tile tile = new Tile();
                    int temp = 0;
                    switch (type)
                    {
                        case TileType.BLANK:
                            tile = new Tile(type, current_idx, current_idx);
                            break;
                        case TileType.CHUTE:
                            tile = new Tile(type, Dice.roll(1, 97), current_idx);
                            if (tile.Go_to_position > tile.Activate_position)
                            {
                                temp = tile.Go_to_position;
                                tile.Go_to_position = tile.Activate_position;
                                tile.Activate_position = temp;
                            }
                            break;
                        case TileType.LADDER:
                            tile = new Tile(type, current_idx, Dice.roll(1, 98));
                            if (tile.Go_to_position < tile.Activate_position)
                            {
                                temp = tile.Go_to_position;
                                tile.Go_to_position = tile.Activate_position;
                                tile.Activate_position = temp;
                            }
                            break;
                    }

                    board_to_return[row, column] = tile;

                }
            }
            for (int i = 0; i < board_width; i++)
            {
                for (int j = 0; j < board_width; j++)
                {
                    int idx = i * board_width + j;
                    int go_to = board_to_return[i, j].Go_to_position;
                    int activate = board_to_return[i, j].Activate_position;
                    TileType t_type = board_to_return[i, j].Tile_type;
                    Tile go_tile = board_to_return[go_to / 10, go_to % 10];
                    Tile activate_tile = board_to_return[activate / 10, activate % 10];
                    go_tile.Activate_position = activate;
                    go_tile.Go_to_position = go_to;
                    go_tile.Tile_type = t_type;
                    activate_tile.Activate_position = activate;
                    activate_tile.Go_to_position = go_to;
                    activate_tile.Tile_type = t_type;
                }
            }
            tester = new Player();
            for (int turn = 0; turn < 35; turn++)
            {
                int position = Dice.roll();
                tester.Position += position;
                Game.handle_tile(tester, board_to_return[tester.Position / 10, tester.Position % 10], position, ignore_console: true);
                if (tester.Position == 99)
                {
                    break;
                }
            }
        } while (tester.Position == 99);
        return board_to_return;
    }
}