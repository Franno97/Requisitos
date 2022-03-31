using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mre.Visas.Requisitos.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RequisitoController : ControllerBase
  {

    private IConfiguration configuration;
    public string TokenAcceso = string.Empty;

    public RequisitoController(IConfiguration iconfiguration)
    {
      configuration = iconfiguration;
    }

    [Route("GrabarDocumentoZipAsync")]
    [HttpPost]
    //[EnableCors("AllowOrigin")]
    public Models.Resultado GrabarDocumentoZipAsync()
    {
      Models.Resultado resultado;
      try
      {
        string urlTramites = configuration.GetSection("RemoteServices").GetSection("Tramites").GetSection("BaseUrl").Value;
        string urlPathZip = configuration.GetSection("PathZip").Value;

        var tramiteId = Convert.ToString(HttpContext.Request.Form["tramiteId"]);
        if (tramiteId == null)
        { 
        
        }
        var tramite = new Models.Response.TramiteResponse();
        #region Consultar Tramite
        HttpClient Client;
        String Uri = string.Empty;
        string PlacesJson = string.Empty;
        Client = new HttpClient();
        Uri = new Uri(urlTramites) + "api/Tramite/ConsultarTramitePorId";
        Models.Request.TramiteRequest tramiteRequest = new Models.Request.TramiteRequest { Id = Guid.Parse(tramiteId) };
        var data = JsonConvert.SerializeObject(tramiteRequest);
        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = Client.PostAsync(Uri, content).Result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
          tramite = JsonConvert.DeserializeObject<Models.Response.TramiteResponse>(response.Content.ReadAsStringAsync().Result);
        }

        #endregion

        //Vamos a consultar el tramite con sus datos

        ////inicio xml
        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
        doc.LoadXml("<Datos_Documentos>" +
                    "  <Id_ciudadano>" + tramite.result.Beneficiario.CodigoMDG + "</Id_ciudadano>" +
                    "  <Id_tramite>" + tramiteId + "</Id_tramite>" +
                    "</Datos_Documentos>");


        XmlNode root = doc.DocumentElement;

        #region PASAPORTE
        System.Xml.XmlNode nodoPASP = doc.CreateNode("element", "PASP","");
        System.Xml.XmlNode elementNumeroPasaporte = doc.CreateNode("element", "Num_Pasaporte", "");
        System.Xml.XmlNode elementNombre = doc.CreateNode("element", "nombre", "");
        System.Xml.XmlNode elementFechaCaducidad = doc.CreateNode("element", "fec_caducidad_PASP", "");
        System.Xml.XmlNode elementGUIDPASP = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("PASP")) != null)
        {
          elementNumeroPasaporte.InnerText = tramite.result.Beneficiario.Pasaporte.Numero;
          elementNombre.InnerText = tramite.result.Beneficiario.Nombres + " " + tramite.result.Beneficiario.PrimerApellido + " " + tramite.result.Beneficiario.SegundoApellido;
          elementFechaCaducidad.InnerText = tramite.result.Beneficiario.Pasaporte.FechaExpiracion.ToString("dd/MM/yyyy");
          elementGUIDPASP.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("PASP")).Id.ToString();
        }
        else
        {
          elementNumeroPasaporte.InnerText = " ";
          elementNombre.InnerText = " ";
          elementFechaCaducidad.InnerText = " ";
          elementGUIDPASP.InnerText = Guid.Empty.ToString();
        }

        nodoPASP.AppendChild(elementNumeroPasaporte);
        root.AppendChild(nodoPASP);

        nodoPASP.AppendChild(elementNombre);
        root.AppendChild(nodoPASP);

        nodoPASP.AppendChild(elementFechaCaducidad);
        root.AppendChild(nodoPASP);

        nodoPASP.AppendChild(elementGUIDPASP);
        root.AppendChild(nodoPASP);
        #endregion

        #region CEDULA
        System.Xml.XmlNode nodoCEDU = doc.CreateNode("element", "CEDU", "");
        System.Xml.XmlNode elementNumeroCedula = doc.CreateNode("element", "Num_Cedula", "");
        System.Xml.XmlNode elementNombreCedula = doc.CreateNode("element", "Nombre", "");
        System.Xml.XmlNode elementFechaCaducidadCEDU = doc.CreateNode("element", "fec_caducidad_CEDU", "");
        System.Xml.XmlNode elementGUIDCEDU = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("CEDU")) != null)
        {
          elementNumeroCedula.InnerText = tramite.result.Beneficiario.Pasaporte.Numero;
          elementNombreCedula.InnerText = tramite.result.Beneficiario.Nombres + " " + tramite.result.Beneficiario.PrimerApellido + " " + tramite.result.Beneficiario.SegundoApellido;
          elementFechaCaducidadCEDU.InnerText = tramite.result.Beneficiario.Pasaporte.FechaExpiracion.ToString("dd/MM/yyyy");
          elementGUIDCEDU.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("CEDU")).Id.ToString();
        }
        else
        {
          elementNumeroCedula.InnerText = " ";
          elementNombreCedula.InnerText = " ";
          elementFechaCaducidadCEDU.InnerText = " ";
          elementGUIDCEDU.InnerText = Guid.Empty.ToString();
        }

        nodoCEDU.AppendChild(elementNumeroCedula);
        root.AppendChild(nodoCEDU);

        nodoCEDU.AppendChild(elementNombreCedula);
        root.AppendChild(nodoCEDU);

        nodoCEDU.AppendChild(elementFechaCaducidadCEDU);
        root.AppendChild(nodoCEDU);

        nodoCEDU.AppendChild(elementGUIDCEDU);
        root.AppendChild(nodoCEDU);
        #endregion

        #region REGISTRO CONSULAR
        System.Xml.XmlNode nodoRCON = doc.CreateNode("element", "RCON", "");
        System.Xml.XmlNode eleRCONNumeroPasaporte = doc.CreateNode("element", "Num_Pasaporte", "");
        System.Xml.XmlNode eleRCONNombre = doc.CreateNode("element", "Nombre", "");
        System.Xml.XmlNode eleRCONFechaCaducidad = doc.CreateNode("element", "fec_caducidad_RCON", "");
        System.Xml.XmlNode eleRCONGUID = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("RCON")) != null)
        {
          eleRCONNumeroPasaporte.InnerText = tramite.result.Beneficiario.Pasaporte.Numero;
          eleRCONNombre.InnerText = tramite.result.Beneficiario.Nombres + " " + tramite.result.Beneficiario.PrimerApellido + " " + tramite.result.Beneficiario.SegundoApellido;
          eleRCONFechaCaducidad.InnerText = tramite.result.Beneficiario.Pasaporte.FechaExpiracion.ToString("dd/MM/yyyy");
          eleRCONGUID.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("RCON")).Id.ToString();
        }
        else
        {
          eleRCONNumeroPasaporte.InnerText = " ";
          eleRCONNombre.InnerText = " ";
          eleRCONFechaCaducidad.InnerText = " ";
          eleRCONGUID.InnerText = Guid.Empty.ToString();
        }

        nodoRCON.AppendChild(eleRCONNumeroPasaporte);
        root.AppendChild(nodoRCON);

        nodoRCON.AppendChild(eleRCONNombre);
        root.AppendChild(nodoRCON);

        nodoRCON.AppendChild(eleRCONFechaCaducidad);
        root.AppendChild(nodoRCON);

        nodoRCON.AppendChild(eleRCONGUID);
        root.AppendChild(nodoRCON);
        #endregion

        #region ANTECEDENTES PENALES
        System.Xml.XmlNode nodoAPEN = doc.CreateNode("element", "APEN", "");
        System.Xml.XmlNode eleAPENFechaCaducidad = doc.CreateNode("element", "fec_caducidad_RCON", "");
        System.Xml.XmlNode eleAPENApostillado = doc.CreateNode("element", "num_reg_apostillado", "");
        System.Xml.XmlNode eleAPENNombre = doc.CreateNode("element", "nombre", "");
        System.Xml.XmlNode eleAPENLugar = doc.CreateNode("element", "lugar", "");
        System.Xml.XmlNode eleAPENGUID = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("APEN")) != null)
        {
          eleAPENFechaCaducidad.InnerText = " ";
          eleAPENApostillado.InnerText = " ";
          eleAPENNombre.InnerText = " ";
          eleAPENLugar.InnerText = " ";
          eleAPENGUID.InnerText = Guid.Empty.ToString();
        }
        else
        {
          eleAPENFechaCaducidad.InnerText = " ";
          eleAPENApostillado.InnerText = " ";
          eleAPENNombre.InnerText = " ";
          eleAPENLugar.InnerText = " ";
          eleAPENGUID.InnerText = Guid.Empty.ToString();
        }

        nodoAPEN.AppendChild(eleAPENFechaCaducidad);
        root.AppendChild(nodoAPEN);

        nodoAPEN.AppendChild(eleAPENApostillado);
        root.AppendChild(nodoAPEN);

        nodoAPEN.AppendChild(eleAPENNombre);
        root.AppendChild(nodoAPEN);

        nodoAPEN.AppendChild(eleAPENLugar);
        root.AppendChild(nodoAPEN);

        nodoAPEN.AppendChild(eleAPENGUID);
        root.AppendChild(nodoAPEN);
        #endregion

        #region PAGO
        System.Xml.XmlNode nodoPAGO = doc.CreateNode("element", "PAGO", "");
        System.Xml.XmlNode elemPAGOLugar = doc.CreateNode("element", "lugar_PAGO", "");
        System.Xml.XmlNode elemPAGOFechaEmision = doc.CreateNode("element", "fec_emision_PAGO", "");
        System.Xml.XmlNode elemPAGONumTransaccion = doc.CreateNode("element", "num_transaccion", "");
        System.Xml.XmlNode elemPAGONumCuenta = doc.CreateNode("element", "num_cuenta", "");
        System.Xml.XmlNode elemPAGOMonto = doc.CreateNode("element", "monto", "");
        System.Xml.XmlNode elemPAGOGUID = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("PAGO")) != null)
        {
          elemPAGOLugar.InnerText = tramite.result.Beneficiario.Pasaporte.Numero;
          elemPAGOFechaEmision.InnerText = tramite.result.Beneficiario.Nombres + " " + tramite.result.Beneficiario.PrimerApellido + " " + tramite.result.Beneficiario.SegundoApellido;
          elemPAGONumTransaccion.InnerText = tramite.result.Beneficiario.Pasaporte.FechaExpiracion.ToString("dd/MM/yyyy");
          elemPAGONumCuenta.InnerText = " ";
          elemPAGOMonto.InnerText = " ";
          elemPAGOGUID.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("PAGO")).Id.ToString();
        }
        else
        {
          elemPAGOLugar.InnerText = " ";
          elemPAGOFechaEmision.InnerText = " ";
          elemPAGONumTransaccion.InnerText = " ";
          elemPAGONumCuenta.InnerText = " ";
          elemPAGOMonto.InnerText = " ";
          elemPAGOGUID.InnerText = Guid.Empty.ToString();
        }

        nodoPAGO.AppendChild(elemPAGOLugar);
        root.AppendChild(nodoPAGO);

        nodoPAGO.AppendChild(elemPAGOFechaEmision);
        root.AppendChild(nodoPAGO);

        nodoPAGO.AppendChild(elemPAGONumTransaccion);
        root.AppendChild(nodoPAGO);

        nodoPAGO.AppendChild(elemPAGONumCuenta);
        root.AppendChild(nodoPAGO);

        nodoPAGO.AppendChild(elemPAGOMonto);
        root.AppendChild(nodoPAGO);

        nodoPAGO.AppendChild(elemPAGOGUID);
        root.AppendChild(nodoPAGO);
        #endregion

        #region PARTIDA DE NACIMIENTO
        System.Xml.XmlNode nodoPNAC = doc.CreateNode("element", "PNAC", "");
        System.Xml.XmlNode elePNACNombre = doc.CreateNode("element", "nombre", "");
        System.Xml.XmlNode elePNACFechaNacimiento = doc.CreateNode("element", "fec_nacimiento", "");
        System.Xml.XmlNode elePNACPais = doc.CreateNode("element", "pais", "");
        System.Xml.XmlNode elePNACNombrePadre = doc.CreateNode("element", "nombre_Padre", "");
        System.Xml.XmlNode elePNACNombreMadre = doc.CreateNode("element", "nombre_Madre", "");
        System.Xml.XmlNode elePNACGUID = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("PNAC")) != null)
        {
          elePNACNombre.InnerText = tramite.result.Beneficiario.Nombres + " " + tramite.result.Beneficiario.PrimerApellido + " " + tramite.result.Beneficiario.SegundoApellido;
          elePNACFechaNacimiento.InnerText = tramite.result.Beneficiario.FechaNacimiento.ToString("dd/MM/yyyy");
          elePNACPais.InnerText = tramite.result.Beneficiario.Domicilio.Pais;
          elePNACNombrePadre.InnerText = " ";//pendiente
          elePNACNombreMadre.InnerText = " ";//pendiente
          elePNACGUID.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("PNAC")).Id.ToString();
        }
        else
        {
          elePNACNombre.InnerText = " ";
          elePNACFechaNacimiento.InnerText = " ";
          elePNACPais.InnerText = " ";
          elePNACNombrePadre.InnerText = " ";
          elePNACNombreMadre.InnerText = " ";
          elePNACGUID.InnerText = Guid.Empty.ToString();
        }

        nodoPNAC.AppendChild(elePNACNombre);
        root.AppendChild(nodoPNAC);

        nodoPNAC.AppendChild(elePNACFechaNacimiento);
        root.AppendChild(nodoPNAC);

        nodoPNAC.AppendChild(elePNACPais);
        root.AppendChild(nodoPNAC);

        nodoPNAC.AppendChild(elePNACNombrePadre);
        root.AppendChild(nodoPNAC);

        nodoPNAC.AppendChild(elePNACNombreMadre);
        root.AppendChild(nodoPNAC);

        nodoPNAC.AppendChild(elePNACGUID);
        root.AppendChild(nodoPNAC);
        #endregion

        #region CONADIS
        System.Xml.XmlNode nodoCOND = doc.CreateNode("element", "COND", "");
        System.Xml.XmlNode eleCONDNombre = doc.CreateNode("element", "nombre", "");
        System.Xml.XmlNode eleCONDFechaCaducidad = doc.CreateNode("element", "fec_caducidad_COND", "");
        System.Xml.XmlNode eleCONDFechaEmision = doc.CreateNode("element", "fec_emision_COND", "");
        System.Xml.XmlNode eleCONDNumeroCarnet = doc.CreateNode("element", "num_carnet", "");
        System.Xml.XmlNode eleCONDGUID = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("COND")) != null)
        {
          eleCONDNombre.InnerText = tramite.result.Beneficiario.Nombres + " " + tramite.result.Beneficiario.PrimerApellido + " " + tramite.result.Beneficiario.SegundoApellido;
          eleCONDFechaCaducidad.InnerText = " ";
          eleCONDFechaEmision.InnerText = " ";
          eleCONDNumeroCarnet.InnerText = tramite.result.Beneficiario.CarnetConadis;//pendiente
          eleCONDGUID.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("COND")).Id.ToString();
        }
        else
        {
          eleCONDNombre.InnerText = " ";
          eleCONDFechaCaducidad.InnerText = " ";
          eleCONDFechaEmision.InnerText = " ";
          eleCONDNumeroCarnet.InnerText = " ";
          eleCONDGUID.InnerText = Guid.Empty.ToString();
        }

        nodoCOND.AppendChild(eleCONDNombre);
        root.AppendChild(nodoCOND);

        nodoCOND.AppendChild(eleCONDFechaCaducidad);
        root.AppendChild(nodoCOND);

        nodoCOND.AppendChild(eleCONDFechaEmision);
        root.AppendChild(nodoCOND);

        nodoCOND.AppendChild(eleCONDNumeroCarnet);
        root.AppendChild(nodoCOND);

        nodoCOND.AppendChild(eleCONDGUID);
        root.AppendChild(nodoCOND);
        #endregion

        #region FOTO
        System.Xml.XmlNode nodoFOTO = doc.CreateNode("element", "FOTO", "");
        System.Xml.XmlNode elementFotoGUID = doc.CreateNode("element", "GUIDDOC", "");

        if (tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("FOTO")) != null)
        {
          elementFotoGUID.InnerText = tramite.result.Documentos.FirstOrDefault(x => x.TipoDocumento.Equals("FOTO")).Id.ToString();
        }
        else
        {
          elementFotoGUID.InnerText = " ";
        }

        nodoFOTO.AppendChild(elementFotoGUID);
        root.AppendChild(nodoFOTO);

        #endregion

        bool validar = true;
        List<Models.Internal.Archivo> lstArchivo = new List<Models.Internal.Archivo>();
        int contador = HttpContext.Request.Form.Files.Count;

        for (int i = 0; i < contador; i++)
        {
          var doc2 = HttpContext.Request.Form.Files[i];//archivo
          var nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(doc2.FileName);
          if (!nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "PASP") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "CEDU") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "RCON") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "APEN") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "PAGO") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "PNAC") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "COND") &&
              !nombreArchivo.Equals(tramite.result.Beneficiario.CodigoMDG + "_" + "FOTO"))
          {
            validar = false;
          }
        }
        if (!validar)
        {
          resultado = new Models.Resultado
          {
            Estado = "ERROR",
            Mensaje = "Formato de nombres de archivos es incorrecto",
            Ruta = string.Empty
          };
        }
        else
        {
          for (int i = 0; i < contador; i++)
          {
            var doc3 = HttpContext.Request.Form.Files[i];//archivo

            lstArchivo.Add(new Models.Internal.Archivo
            {
              nombre = System.IO.Path.GetFileName(doc3.FileName),
              extension = System.IO.Path.GetExtension(doc3.FileName),
              documento = Utils.Compartida.ReadToEnd(doc3.OpenReadStream())
            });
          }

          byte[] compressedBytes;
          //con unidad compartida
          string archivo = System.IO.Path.Combine(urlPathZip, "Tramites", tramite.result.Beneficiario.CodigoMDG + ".zip");
          using (var outStream = new MemoryStream())
          {
            using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
              //xml
              byte[] bytesXML = Encoding.Default.GetBytes(doc.OuterXml);
              string fileNameXML = tramite.result.Beneficiario.CodigoMDG + ".xml";
              var fileInArchiveXML = archive.CreateEntry(fileNameXML, CompressionLevel.Optimal);
              using (var entryStream = fileInArchiveXML.Open())
              using (var fileToCompressStream = new MemoryStream(bytesXML))
              {
                fileToCompressStream.CopyTo(entryStream);
              }


              //archivos
              foreach (var datos in lstArchivo)
              {
                byte[] fileBytes = datos.documento;
                string fileName = datos.nombre;
                var fileInArchive = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                using (var entryStream = fileInArchive.Open())
                using (var fileToCompressStream = new MemoryStream(fileBytes))
                {
                  fileToCompressStream.CopyTo(entryStream);
                }
              }
            }
            compressedBytes = outStream.ToArray();
            using (var fileStream = new FileStream(archivo, FileMode.Create))
            {
              outStream.Seek(0, SeekOrigin.Begin);
              outStream.CopyTo(fileStream);
            }
            //temporal
            archivo = System.IO.Path.Combine(urlPathZip, "TramitesTemporal", tramite.result.Beneficiario.CodigoMDG + ".zip");
            using (var fileStream = new FileStream(archivo, FileMode.Create))
            {
              outStream.Seek(0, SeekOrigin.Begin);
              outStream.CopyTo(fileStream);
            }
          }


          resultado = new Models.Resultado
          {
            Estado = "OK",
            Mensaje = "Archivo almacenado",
            Ruta = archivo
          };
        }

      }
      catch (Exception ex)
      {
        resultado = new Models.Resultado
        {
          Estado = "ERROR",
          Mensaje = ex.Message.ToString(),
          Ruta = string.Empty
        };
      }
      return resultado;
    }
  }
}
