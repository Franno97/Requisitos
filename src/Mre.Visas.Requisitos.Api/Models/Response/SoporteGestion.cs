using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class SoporteGestion
  {/// <summary>
   /// Id de tramite
   /// </summary>
    public Guid TramiteId { get; set; }

    /// <summary>
    /// Nombre del archivo con extension
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Descripción del documento que se está adjuntando
    /// </summary>
    public string Descripcion { get; set; }

    /// <summary>
    /// Ruta del archivo de sharepoint
    /// </summary>
    public string Ruta { get; set; }
  }
}
