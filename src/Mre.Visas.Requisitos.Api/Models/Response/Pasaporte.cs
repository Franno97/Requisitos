using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class Pasaporte
  {
    public string CiudadEmision { get; set; }

    public DateTime FechaEmision { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public DateTime FechaNacimiento { get; set; }

    public string Nombres { get; set; }

    public string Numero { get; set; }

    public string PaisEmision { get; set; }
  }
}
