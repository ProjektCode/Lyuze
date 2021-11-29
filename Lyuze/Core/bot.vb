Imports Discord
Imports Discord.WebSocket
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Microsoft.Extensions.DependencyInjection
Imports Victoria
Imports System.Threading
Imports Discord.Net.Providers.WS4Net
Imports JikanDotNet

Public Class bot
    Private _client As DiscordSocketClient
    Private _cmdService As CommandService
    Private ReadOnly eManager As New eventHandler

    Public Sub bot()
        _client = New DiscordSocketClient(New DiscordSocketConfig() With {
            .LogLevel = LogSeverity.Info,
            .DefaultRetryMode = RetryMode.AlwaysRetry,
            .WebSocketProvider = WS4NetProvider.Instance,
            .AlwaysDownloadUsers = True,
            .MessageCacheSize = 200,
            .ExclusiveBulkDelete = True
         })

        _cmdService = New CommandService(New CommandServiceConfig() With {
            .CaseSensitiveCommands = False,
            .DefaultRunMode = RunMode.Async,
            .IgnoreExtraArgs = True
        })

        'Setting up services
        Dim collection = New ServiceCollection
        collection.AddSingleton(_client)
        collection.AddSingleton(_cmdService)
        collection.AddLavaNode(Sub(LavaConfig)
                                   LavaConfig.SelfDeaf = False
                                   LavaConfig.ReconnectAttempts = 5
                               End Sub)
        collection.AddSingleton(Of InteractiveService)
        collection.AddSingleton(Of IJikan, Jikan)
        collection.AddSingleton(Of Images)
        collection.AddSingleton(Of MasterUtils)
        collection.AddHttpClient
        serviceHandler.setProvider(collection)

    End Sub

    Public Async Function mainAsync() As Task
        Dim settings = Lyuze.Settings.Data
        bot()

        Await commandHandler.loadCommandsAsync
        Await eManager.loadEvents()
        Await _client.LoginAsync(TokenType.Bot, settings.Discord.Token)
        Await _client.StartAsync
        Await Task.Delay(Timeout.Infinite)
    End Function




End Class
