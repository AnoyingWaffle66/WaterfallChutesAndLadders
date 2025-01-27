namespace waterfallChutesAndLadders
{
    internal class Game
    {
        public Game()
        {
            //board = new Tile[board_width, board_width];
            board = Controller.generate_board();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.WriteLine("\n" + board[i, j].Tile_type.ToString() + " Activate position: " + board[i, j].Activate_position + " Go to position: " + board[i, j].Go_to_position);
                }
            }
        }
        private const int board_width = 10;
        public int Board_width
        {
            get { return board_width; }
        }
        private Tile[,] board;
        public Tile[,] Board
        {
            get { return board; }
        }

        public void turn(Player player, int player_roll)
        {
            // Update the current player's position via a roll of a six sided die.
            player.Position += player_roll;
            int p_position = player.Position;
            handle_tile(player, board[p_position / board_width, p_position % board_width], player_roll);

        }

        public void handle_tile(Player player, Tile tile, int player_roll)
        {
            switch (tile.Tile_type)
            {
                case TileType.LADDER:
                    if (player.Position == tile.Activate_position)
                    {
                        Console.WriteLine($"You've landed on a ladder by rolling a {player_roll}! You're moving up to position {tile.Go_to_position + 1}");
                        Console.WriteLine("Press enter to continue!");
                        Console.ReadLine();
                        player.Position = tile.Go_to_position;
                    }
                    break;
                case TileType.CHUTE:
                    if (player.Position == tile.Activate_position)
                    {
                        Console.WriteLine($"You've landed on a chute by rolling a {player_roll}! You're moving down to position {tile.Go_to_position + 1}");
                        Console.WriteLine("Press enter to continue!");
                        Console.ReadLine();
                        player.Position = tile.Go_to_position;
                    }
                    break;
                case TileType.BLANK:
                    break;
                default:
                    Console.WriteLine("Invalid TileType provided to handle_type method.");
                    break;
            }
        }

        public bool check_won(Player player)
        {
            return player.Position == 99;
        }
    }
}
