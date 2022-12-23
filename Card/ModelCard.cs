namespace SujinsCards
{
    public abstract class ICard
    {
        public string Name;
        public string Description;
        public int Price;
        public string Image;
        public bool IsActive { get; set; }

        public void MoveCard()
        {
            IsActive = true;
        }
    }
}