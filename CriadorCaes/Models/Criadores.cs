namespace CriadorCaes.Models
{
    /// <summary>
    /// descrição do criador de cães
    /// </summary>
    public class Criadores
    {
        public Criadores() { 
            ListaAnimais=new HashSet<Animais>();
            ListaRacas=new HashSet<Racas>();
        }
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// nome do criador
        /// </summary>
        public string Nome { get; set;}

        /// <summary>
        /// nome comercial do criador de cães
        /// </summary>
        public string NomeComercial { get; set;}

        /// <summary>
        /// morada
        /// </summary>
        public string Morada { get; set;}  

        /// <summary>
        /// Código postal
        /// </summary>
        public string CodPostal { get; set;}

        /// <summary>
        /// email do criador
        /// </summary>
        public string Email { get; set;}

        /// <summary>
        /// telemóvel do criador
        /// </summary>
        public string Telemovel { get; set; }
       
        /// <summary>
        /// FK para a lista de cães/cadelas, propriedade do criador
        /// </summary>
        public ICollection<Animais> ListaAnimais { get; set;}

        /// <summary>
        /// M-N
        /// FK para a lista de Raças atribuidas ao criador
        /// </summary>
        public ICollection<Racas> ListaRacas { get; set; }
    }
}
