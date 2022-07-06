Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket

NotInheritable Class ColdWarZombiesService
    Private Shared ReadOnly basePath As String = AppDomain.CurrentDomain.BaseDirectory
    Private Shared ReadOnly rec As String = $"{basePath}Resources\Data\COD\CW\ZM"

    Public Shared Async Function RandomClass(user As SocketUser) As Task(Of Embed)
        Try
            Dim json As String = String.Empty
            Using r As New StreamReader($"{rec}\types.json")
                json = Await r.ReadToEndAsync
            End Using

            Dim types = WeaponType.FromJson(json)
            Dim rand As New Random
            Dim w = rand.Next(types.Count)
            Dim weapon = types(w)

            Select Case weapon.ToLower
                Case "assualt rifle"
                    Return Await ColdWarZombiesUtils.AssualtRifle(user)
                Case "smg"
                    Return Await ColdWarZombiesUtils.SubMachineGun(user)
                Case "tactical rifle"
                    Return Await ColdWarZombiesUtils.TacticalRifle(user)
                Case "lmg"
                    Return Await ColdWarZombiesUtils.LightMachineGun(user)
                Case "sniper"
                    Return Await ColdWarZombiesUtils.SniperRifle(user)
                Case "pistol"
                    Return Await ColdWarZombiesUtils.Pistol(user)
                Case "shotgun"
                    Return Await ColdWarZombiesUtils.Shotgun(user)
                Case "special"
                    Return Await ColdWarZombiesUtils.Special(user)
            End Select


        Catch ex As Exception
            Return embedHandler.errorEmbed("COD - ColdWar Zombies", ex.Message).Result
        End Try
    End Function

End Class
