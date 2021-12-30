Imports Discord
Imports Discord.WebSocket
Imports Victoria
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class embedHandler

    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function errorEmbed(_source As String, _error As String) As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"ERROR | {_source}") _
            .WithDescription($"{_error}") _
            .WithColor(Color.Red) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function victoriaNowPlayingEmbed(_song As String, track As LavaTrack) As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"NOW PLAYING") _
            .WithDescription($"{_song}") _
            .WithColor(Color.Blue) _
            .WithThumbnailUrl(If(YouTubeService.GetThumbnail(track.Url), "https://i.imgur.com/Kl2Qrd2.png")) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function victoriaNoQueueEmbed() As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle("NO QUEUE") _
            .WithDescription($"There are no songs in the current queue") _
            .WithColor(Color.Red) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function victoriaInvalidUsageEmbed(channel As ITextChannel) As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle("Invalid Command Usage") _
            .WithDescription($"Command cannot be used in {channel.Name}.") _
            .WithColor(Color.Red) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function victoriaSuccessfulClearEmbed() As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
         .WithDescription("Queue has been cleared!") _
         .WithColor(Color.LightGrey) _
         .WithCurrentTimestamp() _
         .Build())

        Return embed
    End Function

End Class
