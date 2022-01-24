Imports Discord
Imports Victoria

NotInheritable Class embedHandler

    Public Shared Async Function basicEmbed(src As String, msg As String, clr As String) As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"{src}") _
            .WithDescription($"{msg}") _
            .WithColor(New Color(clr)) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function errorEmbed(_source As String, _error As String) As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"ERROR | {_source}") _
            .WithDescription($"{_error}") _
            .WithColor(Color.Red) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function victoriaAddedQueue(msg As String) As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithDescription($"Added {msg} to queue") _
            .WithColor(Color.Blue) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function victoriaNowPlayingEmbed(track As LavaTrack) As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"NOW PLAYING") _
            .WithDescription($"{track.Title}{Environment.NewLine}{track.Url}") _
            .WithColor(Color.Blue) _
            .WithThumbnailUrl(If(YouTube.GetThumbnail(track.Url), "https://i.imgur.com/Kl2Qrd2.png")) _
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

    Public Shared Async Function victoriaEmptyChannel() As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
         .WithDescription("Please make sure I'm connected to a voice channel.") _
         .WithColor(Color.DarkRed) _
         .WithCurrentTimestamp() _
         .Build())

        Return embed
    End Function

End Class
