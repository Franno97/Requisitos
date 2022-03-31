using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class Domicilio
  {
    public string Ciudad { get; set; }

    public string Direccion { get; set; }

    public string Pais { get; set; }

    public string Provincia { get; set; }

    public string TelefonoCelular { get; set; }

    public string TelefonoDomicilio { get; set; }

    public string TelefonoTrabajo { get; set; }
  }
}
