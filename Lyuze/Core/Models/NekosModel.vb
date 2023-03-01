Imports System.Runtime.InteropServices
Imports Newtonsoft.Json

Partial Public Class Nekos
	<JsonProperty("results")>
	Public Property Results() As List(Of NekoInfo)
End Class

Partial Public Class NekoInfo
	<JsonProperty("artist_href")>
	Public Property ArtistHref() As Uri
	<JsonProperty("artist_name")>
	Public Property ArtistName() As String
	<JsonProperty("source_url")>
	Public Property SourceUrl() As Uri
	<JsonProperty("url")>
	Public Property Url() As Uri
End Class

Partial Public Class Nekos
	Public Shared Function FromJson(ByVal json As String) As Nekos
		Return JsonConvert.DeserializeObject(Of Nekos)(json, Converter.Settings)
	End Function
End Class


