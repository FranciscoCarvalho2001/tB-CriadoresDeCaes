using System.ComponentModel.DataAnnotations.Schema;

namespace CriadorCaes.Models
{
    /// <summary>
    /// descrição das fotografias
    /// </summary>
    public class Fotografias
    {
        /// <summary>
        /// PK
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do dcoumento com a fotografia do animal
        /// </summary>
        public string NomeFicheiro { get; set; }

        /// <summary>
        /// Data em que a fotografia foi tirada
        /// </summary>
        public DateTime Data {get; set; }

        /// <summary>
        /// local onde fotografia foi tirada
        /// </summary>
        public string Local { get; set; }

        //*********************************************************
        [ForeignKey(nameof(Animal))] // <=> [ForeignKey("Animal")]
        public int AnimalFK { get; set; }

        public Animais Animal { get; set; }
        /*
         * o uso de [anotadores] permite alterar o comportamento
         * dos 'objetos' do nosso programa:
         *  -atributos
         * 
         */
    }
}
