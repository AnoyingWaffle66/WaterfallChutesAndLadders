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
            for (int i = 0; i < board_width; i++)
            {
                for (int j = 0; j < board_width; j++)
                {
                    board_to_return[i, j] = new Tile();
                }
            }

        }
    }
}
