namespace waterfallChutesAndLadders
{
    internal class Game
    {
        public Game()
        {
            board = new Tile[board_width, board_width];
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

        public void turn(Player player)
        {
            // Update the current player's position via a roll of a six sided die.
            player.Position = player.Position + Dice.roll();
            int p_position = player.Position;
            handle_tile(player, board[p_position / board_width, p_position % board_width]);

        }

        public void handle_tile(Player player, Tile tile)
        {
            bool won = false;
            switch (tile.Tile_type)
            {
                case TileType.LADDER:
                case TileType.CHUTE:
                    player.Position = tile.Go_to_position;
                    break;
                case TileType.BLANK:
                    won = check_won(player);
                    break;
                default:
                    Console.WriteLine("Invalid TileType provided to handle_type method.");
                    break;
            }
        }

        private bool check_won(Player player)
        {
            return player.Position == 99;
        }
    }
}
