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

        Dim Texto As String
        Dim Activos As Integer
        Dim Inactivos As Integer
        documentoPDF.Add(New Paragraph("RESULTADOS PARA LA PRUEBA VB.NET" & vbNewLine & vbNewLine,
                          FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE, 16,
                              iTextSharp.text.Font.NORMAL)))
        Texto = "Id".PadRight(10) & "Comuna".ToString().PadRight(30) & "Estatus".ToString().PadRight(10)
        documentoPDF.Add(New Paragraph(Texto,
                          FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE, 11,
                              iTextSharp.text.Font.NORMAL)))
        Dim Rut As String
        Dim RutGenerado As String
        Dim Posicion As Integer
        'Total Leidos en el REST JSON
        Dim TotalLeidos As Integer = 0
        Dim IdNoValidos As String = ""
        For Each Campo As Object In Campos
            TotalLeidos += 1
            Posicion = InStr(Campo("rut"), "-")
            'Genero el campo validador nuevo para compararlo con el que trae el REST
            Rut = Campo("rut").ToString().Substring(0, Posicion - 1)
            RutGenerado = ValidaRut(Rut)
            'Se compara el RUT nuevo con el del REST JSON y se valida la fecha de nacimiento
            If (RutGenerado = Campo("rut")) And IsDate(Campo("fechaNacimiento")) Then
                'Conteo de Estatus
                Select Case Campo("activo")
                    Case "0"
                        Inactivos += 1
                    Case "1"
                        Activos += 1
                End Select
                'Se detectó que el campo comuna no es común, no encontré la instrucción para validar el item dentro del arreglo del REST y lo traté con un Try
                Try
                    Texto = Campo("id").ToString().PadRight(10) & Campo("direccion")("comuna").ToString().PadRight(30) & IIf(Campo("activo") = "0", "Inactivo", "Activo").ToString().PadRight(10)

                Catch ex As Exception
                    Texto = Campo("id").ToString().PadRight(10) & " " & Campo("direccion")("nombre").ToString().PadRight(30) & IIf(Campo("activo") = "0", "Inactivo", "Activo").ToString().PadRight(10)
                End Try

                documentoPDF.Add(New Paragraph(Texto,
                              FontFactory.GetFont(FontFactory.COURIER, 11,
                                  iTextSharp.text.Font.NORMAL)))
            Else
                IdNoValidos &= "RUT: " & Campo("rut") & "   Fecha de Nac.: " & Campo("fechaNacimiento") & vbNewLine
            End If
        Next


        'Se muestra la totalidad de activos e inactivos
        documentoPDF.Add(New Paragraph("Activos: " & Activos.ToString,
                          FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE, 11,
                              iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph("Inactivos: " & Inactivos.ToString,
                          FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE, 11,
                              iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph("Total Leídos: " & TotalLeidos.ToString,
                          FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE, 11,
                              iTextSharp.text.Font.NORMAL)))
        documentoPDF.Add(New Paragraph("Info no válida para revisar:" & vbNewLine & IdNoValidos,
                         FontFactory.GetFont(FontFactory.COURIER, 11,
                             iTextSharp.text.Font.NORMAL)))
        'Se cierra y se guarda el pdf
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
    Public Function ValidaRut(ByVal ElNumero As String) As String
        Dim Resultado As String = ""
        Dim Multiplicador As Integer = 2
        Dim iNum As Integer = 0
        Dim Suma As Integer = 0

        Select Case ElNumero.Length
            Case 7
                For i As Integer = 7 To 1 Step -1
                    iNum = Mid(ElNumero, i, 1)
                    Suma += iNum * Multiplicador
                    Multiplicador += 1
                    If Multiplicador = 8 Then Multiplicador = 2
                Next
            Case 8
                For i As Integer = 8 To 1 Step -1
                    iNum = Mid(ElNumero, i, 1)
                    Suma += iNum * Multiplicador
                    Multiplicador += 1
                    If Multiplicador = 8 Then Multiplicador = 2
                Next
        End Select

        
        Resultado = CStr(11 - (Suma Mod 11))
        If Resultado = "10" Then Resultado = "K"
        If Resultado = "11" Then Resultado = "0"
        Return ElNumero & "-" & Resultado
    End Function
End Class
