Imports Discord.WebSocket
Imports Discord.Commands
Imports Discord
Imports Microsoft.Extensions.DependencyInjection
Imports System.Media
Imports System.IO
Imports System.Threading

NotInheritable Class eventHandler
    Private Shared ReadOnly _client As DiscordSocketClient = serviceHandler.provider.GetRequiredService(Of DiscordSocketClient)
    Private Shared ReadOnly _cmdService As CommandService = serviceHandler.provider.GetRequiredService(Of CommandService)
    Private Shared ReadOnly _images As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Function loadEvents() As Task
        AddHandler _client.Log, AddressOf logAsync
        AddHandler _cmdService.Log, AddressOf logAsync
        AddHandler _client.Ready, AddressOf onReady
        AddHandler _client.MessageReceived, AddressOf messageRecieved
        AddHandler _client.UserJoined, AddressOf onUserJoined
        AddHandler _client.UserLeft, AddressOf onUserLeave

        Return Task.CompletedTask
    End Function

#Region "Discord Events"

    Private Async Function logAsync(msg As LogMessage) As Task
        Await loggingHandler.LogAsync(msg.Source, msg.Severity, msg.Message)
    End Function

    Private Async Function onReady() As Task
        Await _client.SetStatusAsync(UserStatus.Online)

        Dim t = New Timer(Async Sub(__)
                              Await _client.SetGameAsync(_utils.sList.ElementAtOrDefault(_utils.sIndex), type:=ActivityType.Watching)
                              _utils.sIndex = If(_utils.sIndex + 1 = _utils.sList.Count, 0, _utils.sIndex + 1)
                          End Sub, Nothing, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(120))
        _utils.winHide()
        SystemSounds.Asterisk.Play()
    End Function

    Private Async Function messageRecieved(arg As SocketMessage) As Task
        Dim message = TryCast(arg, SocketUserMessage)
        Dim context = New SocketCommandContext(_client, message)
        Dim settings = Lyuze.Settings.Data

        If message.Author.IsBot OrElse TypeOf message.Channel Is IDMChannel Then
            Return
        End If

        'Anti-Discord invite
        If message.Content.Contains("https://discord.gg/") Then
            If Not TryCast(message.Channel, SocketGuildChannel).Guild.GetUser(message.Author.Id).GuildPermissions.Administrator Then
                Await message.DeleteAsync
                Await message.Channel.SendMessageAsync($"{message.Author.Mention} You can not send Discord invite links in this server.")
                Return
            End If
        End If

        Dim argPos As Integer = 0

        If Not (message.HasStringPrefix(settings.Discord.Prefix, argPos) OrElse message.HasMentionPrefix(_client.CurrentUser, argPos)) Then
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
        Dim settings = Lyuze.Settings.Data
        Try
            Dim channel = arg.Guild.GetTextChannel(settings.IDs.WelcomeId)
            Dim msg = $"{arg.Username}#{arg.Discriminator} has joined the server"
            Dim submsg = arg.Guild.MemberCount
            Dim path = Await _images.CreateBannerImageAsync(arg, msg, submsg)
            Await channel.SendFileAsync(path, String.Empty)

            File.Delete(path)
        Catch ex As Exception
            loggingHandler.LogCriticalAsync("bot", ex.Message)
        End Try
    End Function

    Private Async Function onUserLeave(arg As SocketGuildUser) As Task
        Dim settings = Lyuze.Settings.Data
        Try
            Dim channel = arg.Guild.GetTextChannel(settings.IDs.LeaveId)
            Dim msg = $"{arg.Username}#{arg.Discriminator} Has left the server"
            Dim submsg = $"May thy user have thy peace"
            Dim path = Await _images.CreateBannerImageAsync(arg, msg, submsg)
            Await channel.SendFileAsync(path, String.Empty)

            File.Delete(path)
        Catch ex As Exception
            loggingHandler.LogCriticalAsync("bot", ex.Message)
        End Try
    End Function

#End Region

End Class
