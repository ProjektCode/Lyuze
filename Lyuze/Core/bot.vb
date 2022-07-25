Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Discord.Addons.Interactive
Imports Discord.Net.Providers.WS4Net
Imports Microsoft.Extensions.DependencyInjection
Imports System.Threading
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
        collection.AddSingleton(Of InteractiveService)
        collection.AddSingleton(Of IJikan, Jikan)
        collection.AddSingleton(Of Images)
        collection.AddSingleton(Of MasterUtils)
        collection.AddSingleton(Of LevelingSystem)
        'collection.AddSingleton(Of GoldSystem)
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