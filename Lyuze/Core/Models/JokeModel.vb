Imports Newtonsoft.Json

Partial Public Class RandomJoke
	<JsonProperty("error")>
	Public Property [Error]() As Boolean

	<JsonProperty("category")>
	Public Property Category() As String

	<JsonProperty("type")>
	Public Property Type() As String

	<JsonProperty("setup")>
	Public Property Setup() As String

	<JsonProperty("delivery")>
	Public Property Delivery() As String

	<JsonProperty("joke")>
	Public Property Joke() As String

	<JsonProperty("flags")>
	Public Property Flags() As Flags

	<JsonProperty("id")>
	Public Property Id() As Long

	<JsonProperty("safe")>
	Public Property Safe() As Boolean

	<JsonProperty("lang")>
	Public Property Lang() As String
End Class

Partial Public Class Flags
	<JsonProperty("nsfw")>
	Public Property Nsfw() As Boolean

	<JsonProperty("religious")>
	Public Property Religious() As Boolean

	<JsonProperty("political")>
	Public Property Political() As Boolean

	<JsonProperty("racist")>
	Public Property Racist() As Boolean

	<JsonProperty("sexist")>
	Public Property Sexist() As Boolean

	<JsonProperty("explicit")>
	Public Property Explicit() As Boolean
End Class

Partial Public Class RandomJoke
	Public Shared Function FromJson(ByVal json As String) As RandomJoke
		Return JsonConvert.DeserializeObject(Of RandomJoke)(json, Converter.Settings)
	End Function
End Class



