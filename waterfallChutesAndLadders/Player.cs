namespace waterfallChutesAndLadders
{
    internal class Player
    {
        public Player(string color)
        {
            Position = 0;
            this.color = color;
        }

        private int position;
        public int Position
        {
            get { return position; }
            set
            {
                position = value > 100 ? 100 : value;
            }
        }

        private string color;
        private string Color
        {
            get { return color; }
        }
    }
}
