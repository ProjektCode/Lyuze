Imports Newtonsoft.Json

Partial Public Class GenshinArtifact
	<JsonProperty("name")>
	Public Property Name() As String

	<JsonProperty("max_rarity")>
	Public Property MaxRarity() As Long

	<JsonProperty("2-piece_bonus")>
	Public Property The2PieceBonus() As String

	<JsonProperty("4-piece_bonus")>
	Public Property The4PieceBonus() As String
End Class

Partial Public Class GenshinArtifact
	Public Shared Function FromJson(ByVal json As String) As GenshinArtifact
		Return JsonConvert.DeserializeObject(Of GenshinArtifact)(json, Converter.Settings)
	End Function
End Class
