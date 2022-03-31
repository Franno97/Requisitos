﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mre.Visas.Requisitos.Api.Models.Response
{
  public class Documento
  {
    /// <summary>
    /// Id de la tabla
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Id
    /// </summary>
    public Guid TramiteId { get; set; }

    /// <summary>
    /// Nombre del archivo
    /// </summary>
    public string Nombre { get; set; }

    /// <summary>
    /// Ruta del documentoa almancenado el sharepoint
    /// </summary>
    public string Ruta { get; set; }

    /// <summary>
    /// Tipo Documento tiene 
    /// </summary>
    public string TipoDocumento { get; set; }

    /// <summary>
    /// Observacion del documento
    /// </summary>
    public string Observacion { get; set; }
  }
}
