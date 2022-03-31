using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class Solicitante
  {
    public string Identificacion { get; set; }
    public string TipoIdentificacion { get; set; }

    public string Ciudad { get; set; }

    public string ConsuladoNombre { get; set; }

    public string ConsuladoPais { get; set; }

    public string Direccion { get; set; }

    public int Edad { get; set; }

    public string Nacionalidad { get; set; }

    public string Nombres { get; set; }

    public string Pais { get; set; }

    public string Telefono { get; set; }

    public string Correo { get; set; }
  }
}
