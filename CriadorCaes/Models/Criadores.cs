using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")]
        [Display(Name = "Nome")]
        
        public string Nome { get; set;}

        /// <summary>
        /// nome comercial do criador de cães
        /// </summary>
        [Display(Name = "Nome Criador")]
        public string NomeComercial { get; set;}

        /// <summary>
        /// morada
        /// </summary>     
        public string Morada { get; set;}

        /// <summary>
        /// Código postal
        /// </summary>
        [Display(Name = "Código Postal")]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-ZÇÁÃÂÀÕ]+",ErrorMessage ="O {0} tem de ser de forma XXXX-XXX NOME DA TERRA")]
        [StringLength(50)]
        public string CodPostal { get; set;}

        /// <summary>
        /// email do criador
        /// </summary>
        [EmailAddress(ErrorMessage ="O {0} não está escrito corretamente")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [RegularExpression("[a-z._0-9]+@gmail.com", ErrorMessage ="")]
        [StringLength(40)]
        //[RegularExpression("(([abcde.....xyz._]+)| (aluno|estt|esgt|esta) [0-9] {}")]
        public string Email { get; set;}

        /// <summary>
        /// telemóvel do criador
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [Display(Name ="Telemóvel")]
        [StringLength(9, MinimumLength =9,ErrorMessage ="O {0} deve ter {1} digitos")]
        [RegularExpression("9[1236][0-9]{7}", ErrorMessage ="O número de {0} deve começar por 91,92,93,96 e só pode escrever digitos")]
        //((+100)[0-9]{2,5})?[0 - 9]{5,9}
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
