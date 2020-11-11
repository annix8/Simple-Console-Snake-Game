namespace Snake.Model
{
    public class SnakeNode : NodeObject
    {
        public SnakeNode()
        {
            Symbol = '*';
        }

        public SnakeNode Next { get; set; }
        public SnakeNode Prev { get; set; }
    }
}
