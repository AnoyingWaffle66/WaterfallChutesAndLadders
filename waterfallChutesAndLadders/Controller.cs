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

        public static void start()
        {
            // fill with startup code logic
        }

        public void draw()
        {
            //this is code
        }

        public bool handle_input()
        {
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
