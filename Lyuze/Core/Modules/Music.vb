Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Victoria
Imports Victoria.Enums
Imports Microsoft.Extensions.DependencyInjection

<Name("Music")>
<Summary("Lets play some music!")>
Public Class Music
    Inherits interactivebase(Of SocketCommandContext)

    ReadOnly _lavaNode As LavaNode = serviceHandler.provider.GetRequiredService(Of LavaNode)
    Private noQueue As Boolean

    <Command("join")>
    <Summary("Joins your voice channel.")>
    Public Async Function cmdJoin() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.joinAsync(g, DirectCast(Context.User, IVoiceState), DirectCast(chnl, ITextChannel)).Result)
    End Function
    'Change into embed
    <Command("play")>
    <Summary("Plays song - Can be done by text search or YouTube Playlist/Song URL.")>
    <Remarks("/play <YouTube URL> | Villians by k/da")>
    Public Async Function PlayAsync(<Remainder> searchQuery As String) As Task
        If String.IsNullOrWhiteSpace(searchQuery) Then
            Await ReplyAsync("Please provide search terms.")
            Return
        End If

        If Not _lavaNode.HasPlayer(Context.Guild) Then
            Await ReplyAsync("I'm not connected to a voice channel.")
            Return
        End If

        Dim searchResponse = If(Uri.IsWellFormedUriString(searchQuery, UriKind.Absolute), Await _lavaNode.SearchAsync(searchQuery), Await _lavaNode.SearchYouTubeAsync(searchQuery))

        If searchResponse.LoadStatus = searchResponse.LoadStatus.LoadFailed OrElse searchResponse.LoadStatus = searchResponse.LoadStatus.NoMatches Then
            Await ReplyAsync($"I wasn't able to find anything for `{searchQuery}`.")
            Return
        End If

        Dim player = _lavaNode.GetPlayer(Context.Guild)

        If player.PlayerState = PlayerState.Playing OrElse player.PlayerState = PlayerState.Paused Then
            If Not String.IsNullOrWhiteSpace(searchResponse.Playlist.Name) Then
                For Each track In searchResponse.Tracks
                    player.Queue.Enqueue(track)
                Next track
                Await ReplyAsync($"Queued {searchResponse.Tracks.Count} tracks.")
                Await loggingHandler.LogInformationAsync("audio", $"Queued {searchResponse.Tracks.Count} tracks.")
            Else
                Dim track = searchResponse.Tracks(0)
                player.Queue.Enqueue(track)
                Await ReplyAsync($"Added {track.Title} to the queue")
                Await loggingHandler.LogInformationAsync("audio", $"Enqueued: {track.Title}")
            End If
        Else
            Dim track = searchResponse.Tracks(0)

            If Not String.IsNullOrWhiteSpace(searchResponse.Playlist.Name) Then
                For i = 0 To searchResponse.Tracks.Count - 1
                    If i = 0 Then
                        Await player.PlayAsync(track)
                        Await ReplyAsync($"Now Playing: **{track.Title}**")
                    Else
                        player.Queue.Enqueue(searchResponse.Tracks(i))
                    End If
                Next i

                Await ReplyAsync($"Queued {searchResponse.Tracks.Count} tracks.")
            Else
                Await player.PlayAsync(track)
                Await ReplyAsync($"Now Playing: **{track.Title}**")
            End If
        End If
        Await audioService.setVolumeAsync(Context.Guild, 25, Context.Channel)
    End Function

    <Command("disconnect")>
    <[Alias]("leave")>
    <Summary("Leaves voice channel.")>
    Public Async Function cmdLeave() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.leaveAsync(g, TryCast(Context.User, IVoiceState), chnl).Result)
    End Function

    <Command("volume")>
    <[Alias]("vol")>
    <Summary("Set the volume of the bot. Default is 25.")>
    <Remarks("\vol 25 | range is 0 - 150")>
    Public Async Function cmdVol(vol As Integer) As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.setVolumeAsync(g, vol, chnl).Result)
    End Function

    <Command("pause")>
    <[Alias]("resume")>
    <Summary("A toggle command to pause/resume. Could be used both ways.")>
    Public Async Function cmdPause() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.togglePauseAsync(g, chnl).Result)
    End Function

    <Command("skip")>
    <Summary("Skips the current song.")>
    Public Async Function cmdSkip() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.skipTrack(g, chnl).Result)
    End Function

    <Command("queue")>
    <[Alias]("list")>
    <Summary("Shows a short list of the current queue. If there's no queue it will show current song.")>
    Public Async Function cmdList() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild
        Dim player = _lavaNode.GetPlayer(g)

        If chnl.Id = audioService._channel.Id Then

            If player.PlayerState = PlayerState.Paused Or player.PlayerState = PlayerState.Playing Then

                If player.Queue.Count < 1 And player.Track IsNot Nothing Then
                    noQueue = True
                Else
                    noQueue = False
                End If

                If noQueue = True Then

                    Await embedHandler.victoriaNowPlayingEmbed(player.Track.Title, player.Track)
                Else

                    Await PagedReplyAsync(audioService.listTracks(g, chnl).Result)
                End If

            Else
                Await ReplyAsync(embed:=embedHandler.errorEmbed("Audio - Queue", "Make sure the player is playing something first!").Result)

            End If
        Else
            ReplyAsync(embed:=embedHandler.victoriaInvalidUsageEmbed(chnl).Result)

        End If

    End Function

    <Command("clear")>
    <Summary("Clears current queue.")>
    Public Async Function cmdClear() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.clearTracks(g, chnl).Result)
    End Function

    <Command("stop")>
    <Summary("Stops playback completely.")>
    Public Async Function cmdStop() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.stopAsync(g, chnl).Result)
    End Function

    <Command("restart")>
    <Summary("Restarts the current song.")>
    Public Async Function cmdRestart() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.restartAsync(g, chnl).Result)
    End Function

    <Command("seek")>
    <Summary("Seek to a certain point in the current song.")>
    <[Alias]("sk")>
    <Remarks("\sk <Option> - seeks to the given time on the given video | Options = 1s,1m,1h")>
    Public Async Function cmdSeek(<Remainder> time As TimeSpan) As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.seekAsync(g, time, chnl).Result)
    End Function

    <Command("shuffle")>
    <Summary("Shuffles current queue if there are enough songs.")>
    Public Async Function cmdShuffle() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild
        Dim u As IVoiceState = Context.User

        Await chnl.SendMessageAsync(embed:=audioService.shuffleAsync(g, u, chnl).Result)
    End Function

    <Command("np")>
    <Summary("Shows the current song.")>
    Public Async Function cmdNowPlaying() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.nowPlayingAsync(g, Context, chnl).Result)
    End Function

    <Command("repeat")>
    <Summary("repeats current song.")>
    Public Async Function cmdRepeat() As Task
        Dim chnl = Context.Channel
        Dim g = Context.Guild

        Await chnl.SendMessageAsync(embed:=audioService.repeatAsync(g, chnl).Result)
    End Function

    <Command("radio")>
    <Summary("Plays a 24/7 Anime music stream from LISTEN.Moe")>
    Public Async Function Radio() As Task
        PlayAsync("https://listen.moe/stream")
    End Function

End Class