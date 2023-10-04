Imports Discord.WebSocket
Imports Discord.Commands
Imports Discord
Imports Microsoft.Extensions.DependencyInjection
Imports System.Media
Imports System.IO
Imports System.Threading
Imports Pastel
Imports System.Drawing
Imports Victoria

NotInheritable Class eventHandler
    Inherits ModuleBase(Of SocketCommandContext)

    Private Shared ReadOnly _lavaNode As LavaNode = serviceHandler.provider.GetRequiredService(Of LavaNode)
    Private Shared ReadOnly _client As DiscordSocketClient = serviceHandler.provider.GetRequiredService(Of DiscordSocketClient)
    Private Shared ReadOnly _cmdService As CommandService = serviceHandler.provider.GetRequiredService(Of CommandService)
    Private Shared ReadOnly _images As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)


    Public Function loadEvents() As Task
        AddHandler _client.Log, AddressOf logAsync
        AddHandler _cmdService.Log, AddressOf logAsync
        AddHandler _client.Ready, AddressOf onReady
        AddHandler _client.MessageReceived, AddressOf onMessageRecieved
        AddHandler _lavaNode.OnTrackEnded, AddressOf audioService.trackEnded
        AddHandler _lavaNode.OnTrackStarted, AddressOf audioService.trackStart
        AddHandler _client.UserJoined, AddressOf onUserJoined
        AddHandler _client.UserLeft, AddressOf onUserLeave

        Return Task.CompletedTask
    End Function

#Region "Discord Events"

    Private Async Function logAsync(msg As LogMessage) As Task
        Await loggingHandler.LogAsync(msg.Source, msg.Severity, msg.Message)
    End Function

    Private Async Function onReady() As Task
        If Not _lavaNode.IsConnected Then
            Try
                Await _lavaNode.ConnectAsync
                Await loggingHandler.LogInformationAsync("victoria", "LavaNode Connected")
            Catch ex As Exception
                Throw ex
            End Try
        End If

        Await _client.SetStatusAsync(UserStatus.Online)
        Dim i = _utils.RandomListIndex(Settings.Data.Status)
        Dim t = New Timer(Async Sub(__)
                              Await _client.SetGameAsync(_utils.sList.ElementAtOrDefault(i), type:=ActivityType.Listening)
                              i = If(i + 1 = _utils.sList.Count, 0, i + 1)
                          End Sub, Nothing, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(120))
        '_utils.winHide()
        SystemSounds.Asterisk.Play()
    End Function

    Private Async Function onMessageRecieved(arg As SocketMessage) As Task

        Dim message = TryCast(arg, SocketUserMessage)
        Dim context = New SocketCommandContext(_client, message)
        Dim settings = Lyuze.Settings.Data
        Dim user = context.User

        If message.Author.IsBot OrElse TypeOf message.Channel Is IDMChannel Then
            Return
        End If

        'Database
        Player.CheckProfile(user)
        _lvl.MsgCooldown(arg, context)



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
            Dim msg = $"{arg.Username} {settings.WelcomeMessage(_utils.RandomListIndex(settings.WelcomeMessage))}"
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
            Dim msg = $"{arg.Username} Has left the server"
            Dim submsg = $"{settings.GoodbyeMessage(_utils.RandomListIndex(settings.GoodbyeMessage))}"
            Dim path = Await _images.CreateBannerImageAsync(arg, msg, submsg)
            Await channel.SendFileAsync(path, String.Empty)

            File.Delete(path)
        Catch ex As Exception
            loggingHandler.LogCriticalAsync("bot", ex.Message)
        End Try
    End Function

#End Region

End Class
