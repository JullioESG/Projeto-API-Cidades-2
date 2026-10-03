using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Cidade
    {
        public int Id {get; set; }

        public string Nome { get; set; }

        public int Populacao { get; set; }

        public int IdEstado { get; set; }
        
        public int IdPais { get; set; }
    }
}