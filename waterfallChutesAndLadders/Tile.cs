namespace waterfallChutesAndLadders
{
    internal struct Tile
    {
        public Tile(TileType tile_type, int activate_position, int go_to_position)
        {
            this.tile_type = tile_type;
            this.activate_position = activate_position;
            this.go_to_position = go_to_position;
        }

        private TileType tile_type;
        public TileType Tile_type
        {
            get { return tile_type; }
        }
        private int activate_position;
        public int Activate_position
        {
            get { return activate_position; }
        }
        private int go_to_position;
        public int Go_to_position
        {
            get { return go_to_position; }
        }
    }
    public enum TileType
    {
        CHUTE,
        LADDER,
        BLANK,
        NONE
    }
}
