Imports System.Text
Imports System.Threading
Imports System.Collections.Concurrent
Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Victoria
Imports Victoria.Enums
Imports Victoria.EventArgs
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class audioService

    Inherits InteractiveBase

    Private Shared ReadOnly _lavaNode As LavaNode = serviceHandler.provider.GetRequiredService(Of LavaNode)
    Private Shared ReadOnly _disconnectTokens = New ConcurrentDictionary(Of ULong, CancellationTokenSource)
    Private Shared ReadOnly _repeatTokens = New ConcurrentDictionary(Of ULong, Boolean)

    Public Shared _channel As ITextChannel

    Public Shared Async Function joinAsync(guild As IGuild, voiceState As IVoiceState, channel As ITextChannel) As Task(Of Embed)

        If _lavaNode.HasPlayer(guild) Then
            Return embedHandler.errorEmbed("Audio - Join", "Already connected to a voice channel.").Result
        End If
        If voiceState.VoiceChannel Is Nothing Then
            Return embedHandler.errorEmbed("Audio - Join", "You must be connected to a voice channel").Result
        End If

        Try
            Await _lavaNode.JoinAsync(voiceState.VoiceChannel, channel)
            _channel = channel
            Dim embed As New EmbedBuilder With {
                .Description = $":white_check_mark: Joined {voiceState.VoiceChannel.Name}
                    {Environment.NewLine}Commands are locked to <#{channel.Id}>.",
                .Color = Color.Green,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Joined Voice Channel"
                }
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Audio - Join", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function leaveAsync(guild As IGuild, voiceState As IVoiceState, channel As ITextChannel) As Task(Of Embed)

        Try
            'Checks if the text channel id the command was executed from matches the channel ID where the join command was
            '   executed. if it matches it leaves the voice channel if not it will since a embed saying the command cannot be
            '       used in the text channel.
            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)

                If player Is Nothing Then 'Check if player is null
                    Return embedHandler.errorEmbed("Audio - Leave", "Player not found").Result
                End If

                If voiceState.VoiceChannel Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Leave", "You must be connected to a voice channel").Result
                End If

                If player.PlayerState = PlayerState.Playing Or player.PlayerState = PlayerState.Connected Or player.PlayerState = PlayerState.Stopped Or player.PlayerState = PlayerState.Paused Then
                    Await player.StopAsync
                    Await _lavaNode.LeaveAsync(player.VoiceChannel)
                    'Await loggingHandler.LogInformationAsync("audio", $"[{Date.Now}] - Bot has left a voice channel.")

                    Dim embed As New EmbedBuilder With {
                    .Description = $":white_check_mark: Left {voiceState.VoiceChannel.Name}.",
                    .Color = Color.DarkGreen,
                    .Footer = New EmbedFooterBuilder With {
                        .Text = "Left Voice Channel"
                    }
                }

                    Return embed.Build
                End If

            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

            End If

        Catch ex As InvalidOperationException
            Return embedHandler.errorEmbed("Audio - Leave", ex.Message).Result
        End Try

    End Function
    'Change into embed eventually - Not working as intended
    Public Shared Async Function playAsync(guild As IGuild, searchQuery As String, channel As ITextChannel) As Task(Of String)

        Try

            If channel.Id = _channel.Id Then

                If String.IsNullOrWhiteSpace(searchQuery) Then
                    Return "Please provide search terms."
                End If

                If Not _lavaNode.HasPlayer(guild) Then
                    Return "I'm not connected to a voice channel."
                End If

                Dim searchResponse = If(Uri.IsWellFormedUriString(searchQuery, UriKind.Absolute), Await _lavaNode.SearchAsync(searchQuery), Await _lavaNode.SearchYouTubeAsync(searchQuery))

                If searchResponse.LoadStatus = searchResponse.LoadStatus.LoadFailed OrElse searchResponse.LoadStatus = searchResponse.LoadStatus.NoMatches Then
                    Return $"I wasn't able to find anything for `{searchQuery}`."
                End If

                Dim player = _lavaNode.GetPlayer(guild)

                If player.PlayerState = PlayerState.Playing OrElse player.PlayerState = PlayerState.Paused Then
                    If Not String.IsNullOrWhiteSpace(searchResponse.Playlist.Name) Then
                        For Each track In searchResponse.Tracks
                            player.Queue.Enqueue(track)
                        Next track
                        Await loggingHandler.LogInformationAsync("audio", $"Queued {searchResponse.Tracks.Count} tracks.")
                        Return $"Queued {searchResponse.Tracks.Count} tracks."
                    Else
                        Dim track = searchResponse.Tracks(0)
                        player.Queue.Enqueue(track)
                        Await loggingHandler.LogInformationAsync("audio", $"Enqueued: {track.Title}")
                        Return $"Added {track.Title} to the queue"
                    End If
                Else
                    Dim track = searchResponse.Tracks(0)

                    If Not String.IsNullOrWhiteSpace(searchResponse.Playlist.Name) Then
                        For i = 0 To searchResponse.Tracks.Count - 1
                            If i = 0 Then
                                Await player.PlayAsync(track)
                                Return $"Now Playing: **{track.Title}**"
                            Else
                                player.Queue.Enqueue(searchResponse.Tracks(i))
                            End If
                        Next i

                        Return $"Queued {searchResponse.Tracks.Count} tracks."
                    Else
                        Await player.PlayAsync(track)
                        Return $"Now Playing: **{track.Title}**"
                    End If
                End If
            Else
                Return $"Command can not be used in **{channel.Name}**"
                'Return embedHandler.victoriaInvalidUsageEmbed(channel)

            End If

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Public Shared Async Function setVolumeAsync(guild As IGuild, vol As Integer, channel As ITextChannel) As Task(Of Embed)

        Try
            If channel.Id = _channel.Id Then

                If vol > 150 Or vol <= 0 Then 'Limits the volume to only be between 1 and 150
                    Return embedHandler.errorEmbed("Audio - Volume", "Please make sure the value is between 1-150.").Result
                End If

                Dim player = _lavaNode.GetPlayer(guild)

                Await player.UpdateVolumeAsync(System.Math.Truncate(vol))
                'loggingHandler.LogInformationAsync("audio", $"Bot volume was set to {vol}")
                Dim embed As New EmbedBuilder With {
                    .Description = $"Volume has been set to :loud_sound:{vol}",
                    .Color = Color.DarkGreen,
                    .Footer = New EmbedFooterBuilder With {
                        .Text = "Volume"
                    }
                }

                Return embed.Build

            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

            End If

        Catch ex As Exception
            Return embedHandler.errorEmbed("Audio - Volume", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function togglePauseAsync(guild As IGuild, channel As ITextChannel) As Task(Of Embed)
        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)

                If Not player.PlayerState = PlayerState.Playing And player.Track Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Pause", "There is nothing playing").Result
                End If
                If Not player.PlayerState = PlayerState.Playing Then
                    Await player.ResumeAsync
                    Dim embedResume As New EmbedBuilder With {
                    .Description = $"**Resumed:** {player.Track.Title}",
                    .Color = Color.DarkGreen,
                    .Footer = New EmbedFooterBuilder With {
                        .Text = "Resumed"
                    }
                }

                    Return embedResume.Build
                End If

                Await player.PauseAsync
                Dim embedPause As New EmbedBuilder With {
                    .Description = $"**Paused:** {player.Track.Title}",
                    .Color = Color.DarkGreen,
                    .Footer = New EmbedFooterBuilder With {
                        .Text = "Paused"
                    }
                }

                Return embedPause.Build

            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

            End If


        Catch ex As InvalidOperationException
            Return embedHandler.errorEmbed("Audio - Resume/Pause", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function skipTrack(guild As IGuild, channel As ITextChannel) As Task(Of Embed)
        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)
                If player Is Nothing Then 'Check is the player is null
                    Return embedHandler.errorEmbed("Audio - Skip", "Player not found").Result
                End If

                If player.Queue.Count < 1 Then 'Checking queue to see if less than 1 if true skip
                    Return embedHandler.victoriaNoQueueEmbed().Result

                Else
                    Try

                        'Save current song for use
                        Dim currentTrack = player.Track
                        'Skip current track
                        Await player.SkipAsync
                        Dim embed As New EmbedBuilder With {
                            .Description = $"The song {currentTrack.Title} has been skipped",
                            .Color = Color.DarkGreen,
                            .Footer = New EmbedFooterBuilder With {
                                .Text = "Resumed"
                                }
                        }

                        Return embed.Build

                    Catch ex As Exception
                        Return embedHandler.errorEmbed("Audio - Skip", ex.Message).Result
                    End Try
                End If
            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result
            End If

        Catch ex As Exception
            Return embedHandler.errorEmbed("Audio - Skip", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function listTracks(guild As IGuild, channel As ITextChannel) As Task(Of PaginatedMessage)

        If channel.Id = _channel.Id Then

            Try
                Dim page = String.Empty
                Dim pages As New List(Of String)

                Dim player = _lavaNode.GetPlayer(guild)

                Dim title = $"*{player.Track.Url}*" & vbLf & "------------------------------------------------------------" & vbLf
                page = title

                If player Is Nothing Then 'Check if player is null
                    page = "No player found"
                    pages.Add(page)
                    Dim pageMsg As New PaginatedMessage With {
                            .Pages = pages
                        }
                    Return pageMsg
                End If

                If player.PlayerState = PlayerState.Playing Then

                    If player.Queue.Count < 1 And player.Track IsNot Nothing Then
                        page = $"**Now Playing:** *{player.Track.Title}* {Environment.NewLine} **URL:** {player.Track.Url}"
                        pages.Add(page)
                        Dim pageMsg As New PaginatedMessage With {
                            .Pages = pages
                        }
                        Return pageMsg
                    Else

                        Dim trackNum = 1
                        Dim listNum = 0
                        Dim avgNum = player.Queue.Count / 10
                        Dim queueAvg = MathF.Round(avgNum)

                        For Each track As LavaTrack In player.Queue
                            page += $"**[{trackNum}]** `{track.Title}`{Environment.NewLine}"
                            trackNum += 1
                            listNum += 1

                            If listNum = queueAvg Then
                                pages.Add(page)
                                page = title
                                listNum = 0
                            End If
                        Next

                        Dim pageMessage As New PaginatedMessage With {
                            .Pages = pages,
                            .Title = player.Track.Title
                        }

                        Return pageMessage
                    End If
                End If

            Catch ex As Exception
                Dim pages As New List(Of String)
                Dim page = ex.Message
                pages.Add(page)
                Dim pageMessage As New PaginatedMessage With {
                            .Pages = pages
                        }
                Return pageMessage
            End Try

        End If

    End Function

    Public Shared Async Function clearTracks(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)
                Dim sBuilder = New StringBuilder
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Skip", "Player not found").Result
                End If


                If player.PlayerState = PlayerState.Playing Or player.PlayerState = PlayerState.Paused And player.Queue.Count > 0 Then
                    player.Queue.Clear()
                End If

                If player.Queue.Count = 0 Then
                    Return embedHandler.victoriaSuccessfulClearEmbed().Result
                Else
                    Dim tracknum = 1
                    For Each track As LavaTrack In player.Queue
                        sBuilder.Append($"{tracknum}: [{track.Title}] - could not be cleared {Environment.NewLine}")
                        tracknum += 1
                    Next

                    Dim embed As New EmbedBuilder With {
                        .Description = $"These songs couldn't be cleared{Environment.NewLine}**{sBuilder}**",
                        .Color = Color.DarkGreen,
                        .Footer = New EmbedFooterBuilder With {
                            .Text = "Failed Clear"
                        }
                    }

                    Return embed.Build

                End If

            End If

        Catch ex As Exception
            Return embedHandler.errorEmbed("Audio - Clear", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function stopAsync(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Stop", "Player not found.").Result
                End If

                If player.PlayerState = PlayerState.Playing Then
                    Await player.StopAsync
                Else
                    Return embedHandler.errorEmbed("Audio - Stop", "Something has to be playing to be able to use this command.").Result
                End If
                'Await loggingHandler.LogInformationAsync("audio", $"Bot has stopped playback.")
                Dim embed As New EmbedBuilder With {
                    .Description = $":white_check_mark: Stopped playback.",
                    .Color = Color.Green
                }

                Return embed.Build

            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result
            End If

        Catch ex As Exception
            Return embedHandler.errorEmbed("Audio - Stop", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function restartAsync(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Restart", "Could not find player.").Result
                End If
                If Not player.PlayerState = PlayerState.Playing Then
                    Return embedHandler.errorEmbed("Audio - Restart", "I need to be playing something in order to repeat.").Result
                End If

                Dim timespan As TimeSpan = TimeSpan.Zero
                Await player.SeekAsync(timespan)
                'Await loggingHandler.LogInformationAsync("audio", $"{player.Track.Title} has been repeated.")
                Dim embed As New EmbedBuilder With {
                    .Description = $":white_check_mark: I have repeated {player.Track.Title}",
                    .Color = Color.Green,
                    .Footer = New EmbedFooterBuilder With {
                        .Text = $"Restarted {player.Track.Title}"
                    }
                }

                Return embed.Build
            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result
            End If

        Catch ex As Exception
            'loggingHandler.LogCriticalAsync("audio", ex.Message)
            Return embedHandler.errorEmbed("Audio - Restart", ex.Message).Result

        End Try

    End Function

    Public Shared Async Function seekAsync(guild As IGuild, timeSpan As TimeSpan, channel As ITextChannel) As Task(Of Embed)

        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Seek", "Could not find player.").Result
                End If
                If Not player.PlayerState = PlayerState.Playing Then
                    Return embedHandler.errorEmbed("Audio - Seek", "I need to be playing something in order to repeat.").Result
                End If

                Await loggingHandler.LogInformationAsync("audio", $"{player.Track.Title} has been seeked to {timeSpan}.")
                Await player.SeekAsync(timeSpan)
                Dim embed As New EmbedBuilder With {
                    .Description = $"**{player.Track.Title}** has been seeked to *{timeSpan}*.",
                    .Color = Color.Green,
                    .ThumbnailUrl = If(YouTubeService.GetThumbnail(player.Track.Url), "https://i.imgur.com/Kl2Qrd2.png"),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = $"{player.Track.Url}"
                    }
                }

                Return embed.Build
            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

            End If

        Catch ex As Exception
            'loggingHandler.LogCriticalAsync("audio", ex.Message)
            Return embedHandler.errorEmbed("Audio - Seek", ex.Message).Result
        End Try


    End Function

    Public Shared Async Function shuffleAsync(guild As IGuild, voiceState As IVoiceState, channel As ITextChannel) As Task(Of Embed)
        Try

            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(guild)
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Shuffle", "Player could not be found.").Result
                End If
                If player.Queue.Count = 0 Or player.Queue.Count = 1 Then
                    Return embedHandler.errorEmbed("Audio - Shuffle", "Not enough tracks for a shuffle").Result
                End If
                Dim users As Integer = player.VoiceChannel.GetUsersAsync.FlattenAsync.Result.Count(Function(x) Not x.IsBot)
                If voiceState.VoiceChannel Is player.VoiceChannel Or ((voiceState.VoiceChannel IsNot player.VoiceChannel) And (users = 0 Or player.Track Is Nothing)) Then
                    player.Queue.Shuffle()

                    Dim embed As New EmbedBuilder With {
                        .Description = $"Queue has been shuffled!",
                        .Color = Color.Green
                    }

                    Return embed.Build
                End If
            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

            End If

        Catch ex As InvalidOperationException
            Return embedHandler.errorEmbed("Audio - Shuffle", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function nowPlayingAsync(g As IGuild, ctx As SocketCommandContext, channel As ITextChannel) As Task(Of Embed)
        Try
            If channel.Id = _channel.Id Then

                Dim player = _lavaNode.GetPlayer(g)
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - NowPlaying", "Player could not be found").Result
                End If

                If player.PlayerState = PlayerState.Playing Or player.PlayerState = PlayerState.Paused Then
                    Return embedHandler.victoriaNowPlayingEmbed(player.Track.Title, player.Track).Result
                Else
                    Return embedHandler.victoriaNoQueueEmbed().Result
                End If
            Else
                Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

            End If

        Catch ex As Exception
            Return embedHandler.errorEmbed("Audio", ex.Message).Result
        End Try

    End Function

    Public Shared Async Function repeatAsync(g As IGuild, channel As ITextChannel) As Task(Of Embed)

        If channel.Id = _channel.Id Then

            Try
                If Not _lavaNode.HasPlayer(g) Then
                    Return embedHandler.errorEmbed("Audio - Repeat", "I'm not connected to a voice channel.").Result
                End If

                Dim player = _lavaNode.GetPlayer(g)
                If player Is Nothing Then
                    Return embedHandler.errorEmbed("Audio - Repeat", "Player could not be found.").Result
                End If

                Dim repeat As Object
                If player.PlayerState = PlayerState.Playing Then
                    If Not _repeatTokens.TryGetValue(player.VoiceChannel.Id, repeat) Then
                        repeat = True
                        _repeatTokens.TryAdd(player.VoiceChannel.Id, True)
                    Else
                        _repeatTokens.TryUpdate(player.VoiceChannel.Id, Not repeat, repeat)
                        repeat = _repeatTokens(player.VoiceChannel.Id)
                    End If

                    Dim embed As New EmbedBuilder With {
                    .Description = If(repeat, "**Repeat has been enabled**", "**Repeat has been disabled**"),
                    .Color = Color.Green,
                    .Footer = New EmbedFooterBuilder With {
                            .Text = "Joined Voice Channel"
                        }
                    }

                    Return embed.Build
                Else
                    Dim embed As New EmbedBuilder With {
                    .Description = "No tracks to enable repeat",
                    .Color = Color.Green,
                    .Footer = New EmbedFooterBuilder With {
                            .Text = "Joined Voice Channel"
                        }
                    }

                    Return embed.Build

                End If

            Catch ex As Exception
                Return embedHandler.errorEmbed("Audio - Repeat", ex.Message).Result
            End Try
        Else
            Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

        End If

    End Function

#Region "Audio Events"
    Public Shared Async Function trackEnded(args As TrackEndedEventArgs) As Task

        If Not args.Reason.ShouldPlayNext Then
            Return
        End If
        Dim repeat As Object
        If _repeatTokens.TryGetValue(args.Player.VoiceChannel.Id, repeat) AndAlso repeat Then
            Dim currentTrack = args.Track
            Await args.Player.PlayAsync(currentTrack)
            Return
        End If

        Dim player = args.Player
        Dim queueable As LavaTrack
        If Not player.Queue.TryDequeue(queueable) Then
            Await args.Player.TextChannel.SendMessageAsync("Playback Finished")
            Return
        End If
        Dim tempVar As Boolean = TypeOf queueable Is LavaTrack
        Dim track As LavaTrack = If(tempVar, queueable, Nothing)
        If Not tempVar Then
            Await player.TextChannel.SendMessageAsync("Next item in the queue is not a track")
            Return
        End If
        Await player.PlayAsync(track)
        Await player.TextChannel.SendMessageAsync($"Now Playing *{track.Title}* - **{track.Author}**")

    End Function

    Public Shared Async Function trackStart(args As TrackStartEventArgs) As Task
        Dim value As Object
        If Not _disconnectTokens.TryGetValue(args.Player.VoiceChannel.Id, value) Then
            Return
        End If

        If value.IsCancellationRequested Then
            Return
        End If
        value.Cancel(True)
        Await loggingHandler.LogInformationAsync("audio", "Auto disconnect has been cancelled.")

    End Function

#End Region

End Class