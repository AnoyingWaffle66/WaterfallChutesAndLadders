namespace waterfallChutesAndLadders
{
    internal class Controller
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
            // fill with startup code logic
            int player_count = 2;
            do
            {
                Console.WriteLine("Enter player count (2-4)");
            } while (handle_input(Console.ReadLine(), ref player_count, integer_prompt: true, min: 2, max: 4));

            Console.WriteLine(player_count);

            for (int i = 0; i < player_count; i++)
            {
                string player_color = "";
                switch (i)
                {
                    case 0:
                        player_color = Color.colors["red"];
                        break;
                    case 1:
                        player_color = Color.colors["green"];
                        break;
                    case 2:
                        player_color = Color.colors["yellow"];
                        break;
                    case 3:
                        player_color = Color.colors["gray"];
                        break;
                }
                players.Add(new Player(player_color));
            }
            int player_counter = 0;
            for (int turn_counter = 0; !game.check_won(players[player_counter]); turn_counter++)
            {
                player_counter = turn_counter % player_count;
                do
                {
                    draw();
                    Console.WriteLine("Player " + (player_counter + 1) + " press enter to roll");
                } while (handle_input(Console.ReadLine(), ref player_count, playing: true));
                game.turn(players[player_counter]);
            }

            Console.WriteLine("Player " + (player_counter + 1) + " has won");

        }

        public void draw()
        {
            Console.Clear();

            // I researched how to use color codes and this is what I got.
            // I'm not sure how to integrate it well with our current system, if we even can.
            string reset = "\u001b[0m"; // ANSI reset code.

            // Displays in plaintext the players and their positions, in their relevant color.
            Console.Write("Positions: ");
            for (int i = 0; i < players.Count; i++)
            {
                Console.Write(players[i].Color +
                              $"Player {i + 1} position: {players[i].Position + 1}" + reset);
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
                    Console.Write("--------");
                }
                Console.WriteLine("-");
            }

            /* This helper method produces a fixed-width cell ignoring ANSI color codes
               for length calculations, so columns stay aligned. I haven't done much
               Regex or ANSI so I'll be honest this is something I barely understand. 
               This needs work, since after I got this all done and tested I realized our  
               diagram was 2x2. To compensate, for now it's 5 long in the interior to fit 
               all the players and any potential 'C' or 'L' from chutes and ladders. */
            string MakeCell(string content, int totalInnerWidth = 5)
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
                foreach (int col in columns)
                {
                    Tile tile = board[row, col];

                    // Make 'content' string to eventually append all cell data into.
                    string content = "";

                    // If chute or ladder, color-code them.
                    if (tile.Tile_type == TileType.CHUTE)
                    {
                        content = "\u001b[95mC\u001b[0m"; // Magenta.
                    }
                    else if (tile.Tile_type == TileType.LADDER)
                    {
                        content = "\u001b[35mL\u001b[0m"; // Purple.
                    }

                    /* Check for players on this tile, if so, add player and color to string list.
                       However, it does not see players on a chute or ladder tile. The commented out
                       code in there will display the player on a chute/ladder tile, however it'll
                       display it on both ends. I'm not sure how to limit it to just the actual tile. */
                    List<string> playersHere = new List<string>();
                    for (int p = 0; p < players.Count; p++)
                    {
                        if (players[p].Position == tile.Activate_position /*|| players[p].Position == tile.Go_to_position*/)
                        {
                            // Just "P" in the player's color
                            playersHere.Add(players[p].Color + "P" + reset);
                        }
                    }

                    // Append the players if present.
                    if (playersHere.Count > 0)
                    {
                        content += string.Join("", playersHere);
                    }

                    // Get the text for the text ignoring the ANSI color stuff.
                    string cellText = MakeCell(content);

                    // Print box interior, for example: "| PP "
                    Console.Write("| " + cellText + " ");
                }

                // Right boundary for this row.
                Console.WriteLine("|");

                // Bottom boundary of this row.
                PrintHorizontalBoundary();
            }

            // Should be fully displayed.
        }


        public bool handle_input(string input, ref int player_count, bool playing = false, bool integer_prompt = false, int min = 0, int max = 1)
        {
            if (playing)
            {
                return !string.IsNullOrEmpty(input);
            }

            if (integer_prompt)
            {
                if (!int.TryParse(input, out player_count) || player_count > max || player_count < min)
                {
                    Console.WriteLine("Bad input");
                    return true;
                }
            }

            return false;
        }

        public static Tile[,] generate_board()
        {
            const int board_width = 10;
            Tile[,] board_to_return = new Tile[board_width, board_width];
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
                    switch (Dice.roll(1, 10))
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            type = TileType.BLANK;
                            break;
                        case 9:
                            type = TileType.CHUTE;
                            break;
                        case 10:
                            type = TileType.LADDER;
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
            return board_to_return;
        }
    }
}