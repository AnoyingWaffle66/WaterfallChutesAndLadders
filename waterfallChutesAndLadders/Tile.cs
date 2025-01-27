namespace waterfallChutesAndLadders
{
    public class Tile
    {
        public Tile() { }
        public Tile(TileType tile_type, int activate_position, int go_to_position)
        {
            this.Tile_type = tile_type;
            this.Activate_position = activate_position;
            this.Go_to_position = go_to_position;
        }

        private TileType tile_type;
        public TileType Tile_type
        {
            get { return tile_type; }
            set { tile_type = value; }
        }
        private int activate_position;
        public int Activate_position
        {
            get { return activate_position; }
            set { activate_position = value; }
        }
        private int go_to_position;
        public int Go_to_position
        {
            get { return go_to_position; }
            set { go_to_position = value; }
        }
    }
    public enum TileType
    {
        CHUTE,
        LADDER,
        BLANK
    }
}
