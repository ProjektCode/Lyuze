Imports Newtonsoft.Json

Partial Public Class GenshinCharacter
    <JsonProperty("name")>
    Public Property Name() As String

    <JsonProperty("vision")>
    Public Property Vision() As String

    <JsonProperty("weapon")>
    Public Property Weapon() As String

    <JsonProperty("nation")>
    Public Property Nation() As String

    <JsonProperty("affiliation")>
    Public Property Affiliation() As String

    <JsonProperty("rarity")>
    Public Property Rarity() As Long

    <JsonProperty("constellation")>
    Public Property Constellation() As String

    <JsonProperty("birthday")>
    Public Property Birthday() As String

    <JsonProperty("description")>
    Public Property Description() As String

    <JsonProperty("skillTalents")>
    Public Property SkillTalents() As Constellation()

    <JsonProperty("passiveTalents")>
    Public Property PassiveTalents() As Constellation()

    <JsonProperty("constellations")>
    Public Property Constellations() As Constellation()

    <JsonProperty("vision_key")>
    Public Property VisionKey() As String

    <JsonProperty("weapon_type")>
    Public Property WeaponType() As String
End Class

Partial Public Class Constellation
    <JsonProperty("name")>
    Public Property Name() As String

    <JsonProperty("unlock")>
    Public Property Unlock() As String

    <JsonProperty("description")>
    Public Property Description() As String

    <JsonProperty("level", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Level() As Long?

    <JsonProperty("type", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Type() As String
End Class

Partial Public Class GenshinCharacter
    Public Shared Function FromJson(ByVal json As String) As GenshinCharacter
        Return JsonConvert.DeserializeObject(Of GenshinCharacter)(json, Converter.Settings)
    End Function

End Class