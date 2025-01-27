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
                Console.WriteLine("Enter player count (1-4)");
            } while (handle_input(Console.ReadLine(), ref player_count, integer_prompt: true, min: 1, max: 4));

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
                        player_color = Color.colors["yellow"];
                        break;
                }
                players.Add(new Player(player_color));
            }
            int player_counter = 0;
            for (int turn_counter = 0; !game.check_won(players[player_counter]);)
            {
                game.turn(players[player_counter]);
                draw();
                turn_counter++;
                player_counter = turn_counter % player_count;
            }

            Console.WriteLine("Player " + (player_counter + 1) + " has won");

        }

        public void draw()
        {
            foreach (Player player in players)
            {
                Console.WriteLine("Player position " + player.Position);
            }
            //this is code
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

        public static int PromptForMenuSelection(IEnumerable<string> options, bool withQuit)
        {
            int select = -1;
            int min = 1;
            for (int i = 0; i < options.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {options.ElementAt(i)}");
            }
            if (withQuit)
            {
                min = 0;
                Console.WriteLine("\n 0.Quit\n");
            }
            do
            {
                select = PromptForInt("what is your choice", min, options.Count());
            } while (select == -1);
            return select;
        }
        public static int PromptForInt(string prompt, int min, int max)
        {
            int m = -1;
            bool b = false;
            if (min > max) { throw new ArgumentException("min can not be greater than max"); }
            do
            {
                string s = PromptForInput(prompt, false);
                b = int.TryParse(s, out m);
                if (!b)
                {
                    Console.WriteLine("please insert vaild int");
                }
                else
                {
                    if (m < min || m > max)
                    {
                        Console.WriteLine($"invalid input please insert a value between {min} and {max}");
                    }
                }
            } while (!b);
            return m;
        }
        public static string PromptForInput(string prompt, bool allowEmpty)
        {
            string s = null;
            if (string.IsNullOrWhiteSpace(prompt)) { throw new ArgumentException("promt can not be null, Whitespace, or empty"); }
            do
            {
                Console.Write(prompt);
                s = Console.ReadLine();
                if (string.IsNullOrEmpty(s) && !allowEmpty)
                {
                    Console.WriteLine("input ivalid");
                    continue;
                }
            } while (s == null);
            return s;
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
                    TileType type = TileType.BLANK;
                    switch (Dice.roll())
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            type = TileType.BLANK;
                            break;
                        case 5:
                            type = TileType.CHUTE;
                            break;
                        case 6:
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
                            tile = new Tile(type, Dice.roll(1, 98), current_idx);
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
