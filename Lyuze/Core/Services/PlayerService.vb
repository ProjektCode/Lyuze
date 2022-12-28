Imports MongoDB.Bson
Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection
Imports Discord.WebSocket

NotInheritable Class PlayerService
    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function UpdateBackground(user As SocketGuildUser, url As String) As Task(Of Embed)

        Try
            Dim _player As PlayerModel = Await Player.GetUser(user)
            _player.Background = url
            Await Player.UpdateUser(user, _player)

            Return embedHandler.basicEmbed("Background Updated", $"Your profile background has been updated to {url}").Result
        Catch ex As Exception
            Return embedHandler.errorEmbed("SET - BACKGROUND", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function UpdateAboutMe(user As SocketGuildUser, <Remainder> about As String) As Task(Of Embed)

        Try

            If Not about.Length > 256 Then
                Dim _player As PlayerModel = Await Player.GetUser(user)
                _player.AboutMe = about
                Await Player.UpdateUser(user, _player)

                Return embedHandler.basicEmbed("About Me Updated", $"Your profile About me has been updated to *{about}*").Result
            End If
            Return embedHandler.errorEmbed("SET - ABOUT", "The length exceeds 256 characters. Try to keep it below 100. We don't need to know
                    everything just enough for an introduction :)").Result
        Catch ex As Exception
            Return embedHandler.errorEmbed("SET - ABOUT", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function UpdateFavCharacter(user As SocketGuildUser, <Remainder> character As String) As Task(Of Embed)

        Try

            If Not character.Length > 256 Then
                Dim _player As PlayerModel = Await Player.GetUser(user)
                _player.FavChar = character
                Await Player.UpdateUser(user, _player)

                Return embedHandler.basicEmbed("Favorite Character Updated", $"Your favorite character has been updated to *{character}*").Result
            End If
            Return embedHandler.errorEmbed("SET - CHARACTER", "The length exceeds 256 characters. Is there a character with a name that long?").Result
        Catch ex As Exception
            Return embedHandler.errorEmbed("SET - CHARACTER", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function UpdateProfile(user As SocketGuildUser, type As String) As Task(Of Embed)

        Try
            If type.ToLower = "private" Or type.ToLower = "public" Then
                Dim _player As PlayerModel = Await Player.GetUser(user)
                _player.PublicProfile = type
                Await Player.UpdateUser(user, _player)

                Return embedHandler.basicEmbed("Profile Updated", $"Your profile has been updated to {type}").Result
            End If

            Return embedHandler.errorEmbed("SET - PROFILE", "your profile can either be **private** or **public**").Result
        Catch ex As Exception
            Return embedHandler.errorEmbed("SET - PROFILE", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function UpdateLevelNotify(user As SocketGuildUser, type As String) As Task(Of Embed)
        Try
            Select Case type.ToLower
                Case "no"
                    Dim _player As PlayerModel = Await Player.GetUser(user)
                    _player.LevelNotify = False
                    Await Player.UpdateUser(user, _player)

                    Return embedHandler.basicEmbed("Profile Updated", $"Your level notifications has been set to {type}").Result
                Case "yes"
                    Dim _player As PlayerModel = Await Player.GetUser(user)
                    _player.LevelNotify = True
                    Await Player.UpdateUser(user, _player)

                    Return embedHandler.basicEmbed("Profile Updated", $"Your level notifications has been set to {type}").Result
                Case Else
                    Dim _player As PlayerModel = Await Player.GetUser(user)
                    _player.LevelNotify = True
                    Await Player.UpdateUser(user, _player)

                    Return embedHandler.basicEmbed("Profile Updated", $"Your level notifications has been set to {type} since you did not pick a viable answer. Please say yes or no.").Result
            End Select
        Catch ex As Exception
            Return embedHandler.errorEmbed("SET - LEVEL NOTIFY", ex.Message).Result
        End Try
    End Function


End Class