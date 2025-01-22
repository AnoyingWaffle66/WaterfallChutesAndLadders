namespace waterfallChutesAndLadders
{
    internal class Game
    {
        public Game()
        {

        }
        private Tile[][] board;
        public Tile[][] Board
        {
            get { return board; }
        }

        public void turn(Player player)
        {
            // Update the current player's position via a roll of a six sided die.
            player.Position = player.Position + Dice.roll();
        }

        public void handle_tile(Player player, Tile tile)
        {
            switch (tile.Tile_type)
            {
                case TileType.LADDER:
                case TileType.CHUTE:
                    player.Position = tile.Go_to_position;
                    break;
                case TileType.BLANK:
                    check_won(player);
                    break;
            }
        }

        private bool check_won(Player player)
        {
            return player.Position == 100;
        }
    }
}
