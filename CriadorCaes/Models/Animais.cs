namespace CriadorCaes.Models
{
    /// <summary>
    /// descrição dos animais
    /// </summary>
    public class Animais
    {
        public Animais() 
        { 
            ListaFotografias=new HashSet<Fotografias>();
        }

        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// nome do cão/cadela
        /// </summary>
        public string Nome { get; set; }    

        /// <summary>
        /// data de nascimento
        /// </summary>
        public DateTime DataNascimento { get; set; }

        /// <summary>
        /// data em que o cão foi comprado
        /// </summary>
        public DateTime DataCompra {get; set; }

        /// <summary>
        /// Sexo do cão 
        /// M - Macho
        /// F - Fêmea
        /// </summary>
        public string Sexo { get; set; }

        /// <summary>
        /// número de registo no LOP
        /// </summary>
        public string NumLOP { get; set; }

        // ***************************************************

        public ICollection<Fotografias> ListaFotografias { get; set; }

    }
}
