using waterfallChutesAndLadders;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Generate1Through6InDefaultRollParams()
        {
            SortedSet<int> generate_ints = new SortedSet<int>();

            while (generate_ints.Count != 6)
            {
                generate_ints.Add(Dice.roll());
            }

            Assert.IsTrue(generate_ints.Count == 6);
        }

        [TestMethod]
        public void Generate1Through7InRollParams()
        {
            SortedSet<int> generate_ints = new SortedSet<int>();

            while (generate_ints.Count != 7)
            {
                generate_ints.Add(Dice.roll(1, 7));
            }

            Assert.IsTrue(generate_ints.Count == 7);
        }

        [TestMethod]
        public void Roll7DiceValueGreaterThan6()
        {
            int numbers_rolled_total = 0;

            numbers_rolled_total = Dice.roll(amount: 7);

            Assert.IsTrue(numbers_rolled_total > 6);
        }

        [TestMethod]
        public void GenerateBoardIs10x10()
        {
            Tile[,] board;
            int board_total_tiles;

            board = Controller.generate_board();
            board_total_tiles = board.Length;

            Assert.IsTrue(board_total_tiles == 100);
        }

        [TestMethod]
        public void PlayerMaxTilePositionIs99()
        {
            Player player = new Player("");

            player.Position = 100000;

            Assert.IsTrue(player.Position != 100000);
            Assert.IsTrue(player.Position == 99);
        }
    }
}