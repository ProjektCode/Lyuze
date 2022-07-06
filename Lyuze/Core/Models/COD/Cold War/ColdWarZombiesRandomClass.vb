Imports Newtonsoft.Json

Partial Public Class ColdWar_Zombies_RandomClass
    <JsonProperty("Weapon")>
    Public Property Weapon() As List(Of String)

    <JsonProperty("Optic")>
    Public Property Optic() As List(Of String)

    <JsonProperty("Muzzle")>
    Public Property Muzzle() As List(Of String)

    <JsonProperty("Barrel")>
    Public Property Barrel() As List(Of String)

    <JsonProperty("Body")>
    Public Property Body() As List(Of String)

    <JsonProperty("Underbarrel")>
    Public Property Underbarrel() As List(Of String)

    <JsonProperty("Magazine")>
    Public Property Magazine() As List(Of String)

    <JsonProperty("Handle")>
    Public Property Handle() As List(Of String)

    <JsonProperty("Stock")>
    Public Property Stock() As List(Of String)
End Class

Partial Public Class ColdWar_Zombies_RandomClass
    Public Shared Function FromJson(ByVal json As String) As ColdWar_Zombies_RandomClass
        Return JsonConvert.DeserializeObject(Of ColdWar_Zombies_RandomClass)(json, Converter.Settings)
    End Function
End Class