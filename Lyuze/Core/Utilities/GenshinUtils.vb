NotInheritable Class GenshinUtils

    Public Shared Async Function GetImage(cat As String, name As String, img As String) As Task(Of String)
        Dim newName As String

        If name = "Traveler" Then
            name = "traveler-anemo"
        End If
        If name.Contains(" ") Then
            newName = name.Replace(" ", "-").ToLower
        Else
            newName = name.ToLower
        End If
        If newName.Contains("traveler") Then
            img = "icon"
        End If


        Select Case cat.ToLower
            Case "char"
                Return $"https://api.genshin.dev/characters/{ newName}/{ img}.png"
            Case "wea"
                Return $"https://api.genshin.dev/weapons/{ newName}/{ img}.png"
            Case "con"
            Case "art"
                Return $"https://api.genshin.dev/artifacts/{ newName}/flower-of-life.png"
            Case "dom"
            Case "elements"
            Case "materials"
            Case Else
                Return "https://i.imgur.com/Kl2Qrd2.png"
        End Select

    End Function

    Public Shared Async Function GetRarity(rarity As Long) As Task(Of String)
        Dim stars As String = String.Empty

        For i = 0 To rarity - 1
            stars += "★"
        Next

        Return stars
    End Function

End Class