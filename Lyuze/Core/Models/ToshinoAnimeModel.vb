Imports Newtonsoft.Json

Partial Public Class ToshinoAnime
    <JsonProperty("quote", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Quote() As String

    <JsonProperty("anime", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Anime() As String

    <JsonProperty("id", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Id() As Long?

    <JsonProperty("name", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Name() As String

    <JsonProperty("url", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Url() As Uri
End Class

Partial Public Class ToshinoAnime
    Public Shared Function FromJson(ByVal json As String) As ToshinoAnime
        Return JsonConvert.DeserializeObject(Of ToshinoAnime)(json, Converter.Settings)
    End Function
End Class