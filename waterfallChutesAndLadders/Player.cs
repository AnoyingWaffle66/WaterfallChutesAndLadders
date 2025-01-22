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

        /* This following method isn't in the document, but would be needed to update
         * the player position as the game goes on. - Erik, implementing the dice roll. */
        public void new_position(int position)
        {
            this.position = position;
        }

        private string color;
        private string Color
        {
            get { return color; }
        }
    }
}
