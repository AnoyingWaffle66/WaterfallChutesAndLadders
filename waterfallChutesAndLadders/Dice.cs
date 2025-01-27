namespace waterfallChutesAndLadders
{
    public static class Dice
    {
        private static Random rand = new Random();

        public static int roll(int low = 1, int high = 6, int amount = 1)
        {
            //Inputting roll(1, 6, 2) will act as a six sided die 2 times and return the total.
            int total = 0;

            for (int i = 0; i < amount; i++)
            {
                total += rand.Next(low, high + 1);
            }

            return total;
        }
    }
}