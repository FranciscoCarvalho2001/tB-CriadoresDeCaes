namespace CriadorCaes.Models
{
    /// <summary>
    /// descrição da raça
    /// </summary>
    public class Racas
    {   
        public Racas() {
            ListaAnimais = new HashSet<Animais>();
            ListaCriadores = new HashSet<Criadores>();  
        }

        /// <summary>
        ///  PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Designação da Raça
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Lista dos animais que são de uma raça
        /// </summary>
        public ICollection<Animais> ListaAnimais { get; }
        /// <summary>
        /// M-N
        /// Lista dos criadores de uma determinada raça
        /// </summary>
        public ICollection<Criadores> ListaCriadores { get; set; }
    }
}
