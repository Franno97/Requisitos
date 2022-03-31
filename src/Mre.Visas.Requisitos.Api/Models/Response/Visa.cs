using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class Visa
  {
    public DateTime FechaEmision { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public string Numero { get; set; }

    public bool PoseeVisa { get; set; }

    public string Tipo { get; set; }

    public bool ConfirmacionVisa { get; set; }
  }
}
