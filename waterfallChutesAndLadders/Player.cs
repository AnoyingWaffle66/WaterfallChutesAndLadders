namespace waterfallChutesAndLadders
{
    internal class Player
    {
        public Player(string color)
        {
            this.position = 0;
            this.color = color;
        }

        private int position;
        public int Position
        {
            get { return position; }
        }

        private string color;
        private string Color
        {
            get { return color; }
        }
    }
}
