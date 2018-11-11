namespace TecGames.Models
{
    /// <summary>
    /// Base abstracta para objetos con identidad y nombre.
    /// </summary>
    public abstract class IdentityBasis
    {
        protected int id;
        protected string name;

        /// <summary>
        /// Inicializa una instancia de <see cref="IdentityBasis"/>.
        /// </summary>
        public IdentityBasis()
        {

        }

        /// <summary>
        /// Inicializa una instancia de <see cref="IdentityBasis"/>.
        /// </summary>
        /// <param name="id">Identificador del elemento.</param>
        /// <param name="name">Nombre o descripción del elemento.</param>
        public IdentityBasis(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        /// <summary>
        /// Identidad del elemento.
        /// </summary>
        public int Id => id;

        /// <summary>
        /// Nombre o descripción del elemento.
        /// </summary>
        public string Name => name;
    }
}