using System;
using System.Collections.Generic;

namespace Api_db.Models
{
    public partial class Cuentum
    {
        public Cuentum()
        {
            Clientes = new HashSet<Cliente>();
        }

        public int Idcuenta { get; set; }
        public string? Numero { get; set; }
        public decimal? Saldo { get; set; }

        public virtual ICollection<Cliente> Clientes { get; set; }
    }
}
