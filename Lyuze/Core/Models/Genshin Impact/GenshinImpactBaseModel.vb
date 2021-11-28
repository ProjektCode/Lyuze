Imports Newtonsoft.Json

Partial Public Class GenshinImpactBase
    <JsonProperty("types")>
    Public Property Types() As List(Of String)
End Class

Partial Public Class GenshinImpactBase
    Public Shared Function FromJson(ByVal json As String) As GenshinImpactBase
        Return JsonConvert.DeserializeObject(Of GenshinImpactBase)(json, Converter.Settings)
    End Function
End Class

