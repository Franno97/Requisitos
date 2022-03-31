using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Internal
{
  public class Archivo
  {
    public string nombre { get; set; }
    public string extension { get; set; }
    public byte[] documento { get; set; }

  }
}
