Imports Discord.WebSocket
Imports Discord.Commands
Imports Discord
Imports Victoria
Imports Microsoft.Extensions.DependencyInjection
Imports System.Media
Imports System.IO
Imports System.Threading

NotInheritable Class eventHandler
    Private Shared ReadOnly _lavaNode As LavaNode = serviceHandler.provider.GetRequiredService(Of LavaNode)
    Private Shared ReadOnly _client As DiscordSocketClient = serviceHandler.provider.GetRequiredService(Of DiscordSocketClient)
    Private Shared ReadOnly _cmdService As CommandService = serviceHandler.provider.GetRequiredService(Of CommandService)
    Private Shared ReadOnly _images As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Private settings As settingsHandler
    Private t As Timer

    Public Function loadEvents() As Task
        AddHandler _client.Log, AddressOf logAsync
        AddHandler _cmdService.Log, AddressOf logAsync
        AddHandler _client.Ready, AddressOf onReady
        AddHandler _client.MessageReceived, AddressOf messageRecieved
        AddHandler _lavaNode.OnTrackEnded, AddressOf audioService.trackEnded
        AddHandler _lavaNode.OnTrackStarted, AddressOf audioService.trackStart
        AddHandler _client.UserJoined, AddressOf onUserJoined

        Return Task.CompletedTask
    End Function

#Region "Discord Events"

    Private Async Function logAsync(msg As LogMessage) As Task
        Await (loggingHandler.LogAsync(msg.Source, msg.Severity, msg.Message))
    End Function
    Private Async Function onReady() As Task 'Place custom game event here once masterClass is made
        settings = settingsHandler.Load

        If Not _lavaNode.IsConnected Then
            Try
                Await _lavaNode.ConnectAsync
            Catch ex As Exception
                Throw ex
            End Try
        End If

        Await _client.SetStatusAsync(UserStatus.Online)


        t = New Timer(Async Sub(__)
                          Await _client.SetGameAsync(_utils.sList.ElementAtOrDefault(_utils.sIndex), type:=ActivityType.Watching)
                          _utils.sIndex = If(_utils.sIndex + 1 = _utils.sList.Count, 0, _utils.sIndex + 1)
                      End Sub, Nothing, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(120))
        SystemSounds.Asterisk.Play()
    End Function
    Private Async Function messageRecieved(arg As SocketMessage) As Task
        Dim message = TryCast(arg, SocketUserMessage)
        Dim context = New SocketCommandContext(_client, message)
        settings = settingsHandler.Load

        If message.Author.IsBot OrElse TypeOf message.Channel Is IDMChannel Then
            Return
        End If

        If message.Content.Contains("https://discord.gg/") Then
            If Not TryCast(message.Channel, SocketGuildChannel).Guild.GetUser(message.Author.Id).GuildPermissions.Administrator Then
                Await message.DeleteAsync
                Await message.Channel.SendMessageAsync($"{message.Author.Mention} You can not send Discord invite links in this server.")
                Return
            End If
        End If

        Dim argPos As Integer = 0

        If Not (message.HasStringPrefix(settings.prefix, argPos) OrElse message.HasMentionPrefix(_client.CurrentUser, argPos)) Then
            Return
        End If

        Dim result = Await _cmdService.ExecuteAsync(context, argPos, serviceHandler.provider)

        If Not result.IsSuccess Then
            If result.Error = CommandError.UnknownCommand Then
                Return
            End If
        End If


    End Function
    Private Async Function onUserJoined(arg As SocketGuildUser) As Task
        settings = settingsHandler.Load
        Try
            Dim channel = arg.Guild.GetTextChannel(settings.welcomeID)
            Dim path = Await _images.createWelcomeImageAsync(arg)
            Await channel.SendFileAsync(path, String.Empty)

            File.Delete(path)
        Catch ex As Exception
            loggingHandler.ErrorLog("UserJoined", ex.Message)
        End Try
    End Function

#End Region

End Class
