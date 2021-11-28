Imports Newtonsoft.Json

Partial Public Class GenshinEnemies
	<JsonProperty("id")>
	Public Property Id() As String

	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("region")>
	Public Property Region() As String

	<JsonProperty("description")>
	Public Property Description() As String

	<JsonProperty("type")>
	Public Property Type() As String

	<JsonProperty("family")>
	Public Property Family() As String

	<JsonProperty("elements")>
	Public Property Elements() As List(Of String)

	<JsonProperty("drops")>
	Public Property Drops() As List(Of Drop)

	<JsonProperty("elemental-descriptions")>
	Public Property ElementalDescriptions() As List(Of ElementalDescription)
End Class

Partial Public Class Drop
	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("rarity")>
	Public Property Rarity() As Long

	<JsonProperty("minimum-level")>
	Public Property MinimumLevel() As Long
End Class

Partial Public Class ElementalDescription
	<JsonProperty("element")>
	Public Property Element() As String

	<JsonProperty("description")>
	Public Property Description() As String
End Class

Partial Public Class GenshinEnemies
	Public Shared Function FromJson(ByVal json As String) As GenshinEnemies
		Return JsonConvert.DeserializeObject(Of GenshinEnemies)(json, Converter.Settings)
	End Function
End Class

