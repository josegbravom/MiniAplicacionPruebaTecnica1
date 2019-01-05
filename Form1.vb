Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf



Public Class frmMiniAppPdfRest

    Private Sub bIniciarGeneracion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bIniciarGeneracion.Click

        Dim Direccion As String
        Dim RecibeREST As HttpWebRequest
        Dim Respuesta As HttpWebResponse
        Dim Leer As StreamReader
        Dim ResultadoString As String
        'para crear el pdf
        Dim documentoPDF As New Document

        Dim appPath As String = Application.StartupPath()
        appPath &= "\ReportePdf.pdf"
        PdfWriter.GetInstance(documentoPDF, New FileStream(appPath, FileMode.Create))
        documentoPDF.Open()
        


        Direccion = "https://my-json-server.typicode.com/HaibuSolutions/prueba-tecnica-sf/user"
        'Se obtiene el REST json
        RecibeREST = DirectCast(WebRequest.Create(Direccion), HttpWebRequest)
        'Metodo Get
        Respuesta = DirectCast(RecibeREST.GetResponse(), HttpWebResponse)
        'Obtengo el REST en una cadena
        Leer = New IO.StreamReader(Respuesta.GetResponseStream())
        ResultadoString = Leer.ReadToEnd()

        Dim Campos As Object = New JavaScriptSerializer().Deserialize(Of List(Of Object))(ResultadoString)

        For Each Campo As Object In Campos

            'MsgBox(Campo("direccion")("calle").ToString)
            documentoPDF.Add(New Paragraph(Campo("nombre"),
                          FontFactory.GetFont(FontFactory.TIMES, 11,
                              iTextSharp.text.Font.NORMAL)))

        Next

        documentoPDF.Close()
        If System.IO.File.Exists(appPath) Then
            If MsgBox("Texto convertido a fichero PDF correctamente " + _
                   "¿desea abrir el fichero PDF resultante?",
                   MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                'Abrimos el fichero PDF con la aplicación asociada
                System.Diagnostics.Process.Start(appPath)
            End If
        Else
            MsgBox("El fichero PDF no se ha generado, " + _
                   "compruebe que tiene permisos en la carpeta de destino.",
                   MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)
        End If



    End Sub
    
End Class
