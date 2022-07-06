Imports Newtonsoft.Json

Partial Public Class UselessFactsModel
    <JsonProperty("id")>
    Public Property Id() As Guid

    <JsonProperty("text")>
    Public Property Text() As String

    <JsonProperty("source")>
    Public Property Source() As String

    <JsonProperty("source_url")>
    Public Property SourceUrl() As Uri

    <JsonProperty("language")>
    Public Property Language() As String

    <JsonProperty("permalink")>
    Public Property Permalink() As Uri
End Class

Partial Public Class UselessFactsModel
    Public Shared Function FromJson(ByVal json As String) As UselessFactsModel
        Return JsonConvert.DeserializeObject(Of UselessFactsModel)(json, Converter.Settings)
    End Function
End Class