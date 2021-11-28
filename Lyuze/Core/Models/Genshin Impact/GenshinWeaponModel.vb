Imports Newtonsoft.Json

Partial Public Class GenshinWeapon
	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("type")>
	Public Property Type() As String

	<JsonProperty("rarity")>
	Public Property Rarity() As Long

	<JsonProperty("baseAttack")>
	Public Property BaseAttack() As Long

	<JsonProperty("subStat")>
	Public Property SubStat() As String

	<JsonProperty("passiveName")>
	Public Property PassiveName() As String

	<JsonProperty("passiveDesc")>
	Public Property PassiveDesc() As String

	<JsonProperty("location")>
	Public Property Location() As String
End Class

Partial Public Class GenshinWeapon
	Public Shared Function FromJson(ByVal json As String) As GenshinWeapon
		Return JsonConvert.DeserializeObject(Of GenshinWeapon)(json, Converter.Settings)
	End Function

End Class

