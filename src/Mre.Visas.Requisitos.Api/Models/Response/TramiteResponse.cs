using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class TramiteResponse
  {
    public int httpStatusCode { get; set; }
    public Tramite result { get; set; }
  }
}
