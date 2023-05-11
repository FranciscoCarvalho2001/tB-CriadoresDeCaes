using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// <summary>
        /// lista das fotografias associadas a um animal
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; }

        /// <summary>
        /// FK das raças 
        /// </summary>
        [ForeignKey(nameof(Raca))]
        [Display(Name = "Raça")]
        public int RacaFk { get; set; }
        public Racas Raca{ get; set; }

        /// <summary>
        /// FK para o criador dos cães
        /// </summary>
        [ForeignKey(nameof(Criadores))]
        [Display(Name = "Criadores")]
        public int CriadorFK { get; set; }
        public Criadores Criadores { get; set; }
    }
}
