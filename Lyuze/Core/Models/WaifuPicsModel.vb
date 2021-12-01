Imports Newtonsoft.Json

Partial Public Class WaifuPics
	<JsonProperty("url")>
	Public Property Url() As Uri
End Class

Partial Public Class WaifuPics
	Public Shared Function FromJson(ByVal json As String) As WaifuPics
		Return JsonConvert.DeserializeObject(Of WaifuPics)(json, Converter.Settings)
	End Function
End Class
