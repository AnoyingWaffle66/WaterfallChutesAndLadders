using CSC160_ConsoleMenu
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
    }
}
