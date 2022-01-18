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

    <Command("join")>
    <Summary("Joins your voice channel.")>
    Public Async Function cmdJoin() As Task
        Await ReplyAsync(embed:=Await audioService.joinAsync(Context.Guild, DirectCast(Context.User, IVoiceState), Context.Channel))
    End Function

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
                'Await loggingHandler.LogInformationAsync("audio", $"Queued {searchResponse.Tracks.Count} tracks.")
            Else
                Dim track = searchResponse.Tracks(0)
                player.Queue.Enqueue(track)
                Await ReplyAsync($"Added {track.Title} to the queue")
                'Await loggingHandler.LogInformationAsync("audio", $"Enqueued: {track.Title}")
            End If
        Else
            Dim track = searchResponse.Tracks(0)

            If Not String.IsNullOrWhiteSpace(searchResponse.Playlist.Name) Then
                For i = 0 To searchResponse.Tracks.Count - 1
                    If i = 0 Then
                        Await player.PlayAsync(track)
                        'Await ReplyAsync($"Now Playing: **{track.Title}**")
                        Await ReplyAsync(embed:=Await embedHandler.victoriaNowPlayingEmbed(track.Title, track))
                    Else
                        player.Queue.Enqueue(searchResponse.Tracks(i))
                    End If
                Next i

                'Await ReplyAsync($"Queued {searchResponse.Tracks.Count} tracks.")
            Else
                Await player.PlayAsync(track)
                'Await ReplyAsync($"Now Playing: **{track.Title}**")
                Await ReplyAsync(embed:=Await embedHandler.victoriaNowPlayingEmbed(track.Title, track))
            End If
        End If
        Await audioService.setVolumeAsync(Context.Guild, 25, Context.Channel)
    End Function

    <Command("disconnect")>
    <[Alias]("leave")>
    <Summary("Leaves voice channel.")>
    Public Async Function cmdLeave() As Task
        Await ReplyAsync(embed:=Await audioService.leaveAsync(Context.Guild, TryCast(Context.User, IVoiceState), Context.Channel))
    End Function

    <Command("volume")>
    <[Alias]("vol")>
    <Summary("Set the volume of the bot. Default is 25.")>
    <Remarks("\vol 25 | range is 0 - 150")>
    Public Async Function cmdVol(vol As Integer) As Task
        Await ReplyAsync(embed:=Await audioService.setVolumeAsync(Context.Guild, vol, Context.Channel))
    End Function

    <Command("pause")>
    <[Alias]("resume")>
    <Summary("A toggle command to pause/resume. Could be used both ways.")>
    Public Async Function cmdPause() As Task
        Await ReplyAsync(embed:=Await audioService.togglePauseAsync(Context.Guild, Context.Channel))
    End Function

    <Command("skip")>
    <Summary("Skips the current song.")>
    Public Async Function cmdSkip() As Task
        Await ReplyAsync(embed:=Await audioService.skipTrack(Context.Guild, Context.Channel))
    End Function

    <Command("queue")>
    <[Alias]("list")>
    <Summary("Shows a short list of the current queue. If there's no queue it will show current song.")>
    Public Async Function cmdList() As Task
        Await ReplyAsync(embed:=Await audioService.listTracks(Context.Guild, Context.Channel))
    End Function

    <Command("clear")>
    <Summary("Clears current queue.")>
    Public Async Function cmdClear() As Task
        Await ReplyAsync(embed:=Await audioService.clearTracks(Context.Guild, Context.Channel))
    End Function

    <Command("stop")>
    <Summary("Stops playback completely.")>
    Public Async Function cmdStop() As Task
        Await ReplyAsync(embed:=Await audioService.stopAsync(Context.Guild, Context.Channel))
    End Function

    <Command("restart")>
    <Summary("Restarts the current song.")>
    Public Async Function cmdRestart() As Task
        Await ReplyAsync(embed:=Await audioService.restartAsync(Context.Guild, Context.Channel))
    End Function

    <Command("seek")>
    <Summary("Seek to a certain point in the current song.")>
    <[Alias]("sk")>
    <Remarks("\sk <Option> - seeks to the given time on the given video | Options = 1s,1m,1h")>
    Public Async Function cmdSeek(<Remainder> time As TimeSpan) As Task
        Await ReplyAsync(embed:=Await audioService.seekAsync(Context.Guild, time, Context.Channel))
    End Function

    <Command("shuffle")>
    <Summary("Shuffles current queue if there are enough songs.")>
    Public Async Function cmdShuffle() As Task
        Await ReplyAsync(embed:=Await audioService.shuffleAsync(Context.Guild, Context.User, Context.Channel))
    End Function

    <Command("np")>
    <Summary("Shows the current song.")>
    Public Async Function cmdNowPlaying() As Task
        Await ReplyAsync(embed:=Await audioService.nowPlayingAsync(Context.Guild, Context, Context.Channel))
    End Function

    <Command("repeat")>
    <Summary("repeats current song.")>
    Public Async Function cmdRepeat() As Task
        Await ReplyAsync(embed:=Await audioService.repeatAsync(Context.Guild, Context.Channel))
    End Function

    <Command("radio")>
    <Summary("Plays a raw 24/7 Anime music stream from LISTEN.Moe")>
    Public Async Function Radio() As Task
        PlayAsync("https://listen.moe/stream")
    End Function

End Class