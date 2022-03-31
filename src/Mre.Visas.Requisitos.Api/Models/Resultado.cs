using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models
{
  public class Resultado
  {/// <summary>
   /// estado del resultado
   /// OK = POSITIVO
   /// ERROR = NEGATIVO
   /// </summary>
    public string Estado { get; set; }
    /// <summary>
    /// MENSAJE 
    /// RESULTADO VACIO SI ESTA CORRECTO Y SY CONTIENE ES EL ERROR
    /// </summary>
    public string Mensaje { get; set; }
    /// <summary>
    /// EL RESULTADO DE LA RUTA
    /// </summary>
    public string Ruta { get; set; }
  }
}
