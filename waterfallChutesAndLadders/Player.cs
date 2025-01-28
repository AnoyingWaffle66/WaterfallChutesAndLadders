namespace waterfallChutesAndLadders
{
    public class Player
    {
        public Player()
        {
            Position = 0;
        }

        private int position;
        public int Position
        {
            get { return position; }
            set
            {
                position = value > 99 ? 99 : value;
            }
        }
    }
}
