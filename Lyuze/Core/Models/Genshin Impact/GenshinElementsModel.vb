Imports Newtonsoft.Json

Partial Public Class GenshinElements
	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("key")>
	Public Property Key() As String

	<JsonProperty("reactions")>
	Public Property Reactions() As List(Of Reaction)
End Class

Partial Public Class Reaction
	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("elements")>
	Public Property Elements() As List(Of String)

	<JsonProperty("description")>
	Public Property Description() As String
End Class

Partial Public Class GenshinElements
	Public Shared Function FromJson(ByVal json As String) As GenshinElements
		Return JsonConvert.DeserializeObject(Of GenshinElements)(json, Converter.Settings)
	End Function
End Class
