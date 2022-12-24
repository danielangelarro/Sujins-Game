namespace SujinsCards
{
    /// <summary>
    /// Clase que representa el modelo que debven seguir todas las cartas
    /// al ser creadas.
    /// </summary>
    public abstract class ICard
    {
        public string Name;
        public string Description;
        public int Price;
        public string Image;
        public bool IsActive { get; set; }

        /// <summary>
        /// Cambia a true el valor de la variable IsActive que indica si una carta
        /// esta sobre el campo o no.
        /// </summary>
        public void MoveCard()
        {
            IsActive = true;
        }
    }
}