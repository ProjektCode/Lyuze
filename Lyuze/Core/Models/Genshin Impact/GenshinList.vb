Imports Newtonsoft.Json

Partial Public Class GenshinList

    Public Shared Function FromJson(ByVal json As String) As List(Of String)
        Return JsonConvert.DeserializeObject(Of List(Of String))(json, Converter.Settings)
    End Function

End Class
