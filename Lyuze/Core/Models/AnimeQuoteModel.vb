Imports Newtonsoft.Json

Partial Public Class AnimeQuote
	<JsonProperty("anime")>
	Public Property Anime() As String

	<JsonProperty("character")>
	Public Property Character() As String

	<JsonProperty("quote")>
	Public Property Quote() As String
End Class

Partial Public Class AnimeQuote
	Public Shared Function FromJson(ByVal json As String) As AnimeQuote
		Return JsonConvert.DeserializeObject(Of AnimeQuote)(json, Converter.Settings)
	End Function
End Class