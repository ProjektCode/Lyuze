Imports Newtonsoft.Json

Partial Public Class TraceMoe
	<JsonProperty("frameCount")>
	Public Property FrameCount() As Long

	<JsonProperty("error")>
	Public Property [Error]() As String

	<JsonProperty("result")>
	Public Property Result() As List(Of Result)
End Class

Partial Public Class Result
	<JsonProperty("anilist")>
	Public Property Anilist() As Long

	<JsonProperty("filename")>
	Public Property Filename() As String

	<JsonProperty("episode")>
	Public Property Episode() As Long?

	<JsonProperty("from")>
	Public Property From() As Double

	<JsonProperty("to")>
	Public Property [To]() As Double

	<JsonProperty("similarity")>
	Public Property Similarity() As Double

	<JsonProperty("video")>
	Public Property Video() As Uri

	<JsonProperty("image")>
	Public Property Image() As Uri
End Class

Partial Public Class TraceMoe
	Public Shared Function FromJson(ByVal json As String) As TraceMoe
		Return JsonConvert.DeserializeObject(Of TraceMoe)(json, Converter.Settings)
	End Function
End Class

