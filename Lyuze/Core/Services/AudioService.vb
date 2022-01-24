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
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Private Shared ReadOnly _disconnectTokens = New ConcurrentDictionary(Of ULong, CancellationTokenSource)
    Private Shared ReadOnly _repeatTokens = New ConcurrentDictionary(Of ULong, Boolean)
    Private Shared ReadOnly _listLimit = 10

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

        If _channel IsNot Nothing Then

            Try
                'Checks if the text channel id the command was executed from matches the channel ID where the join command was
                '   executed. if it matches it leaves the voice channel if not it will since a embed saying the command cannot be
                '       used in the text channel.
                If channel.Id = _channel.Id Then

                    Dim player = _lavaNode.GetPlayer(guild)

                    If player Is Nothing Then 'Check if player is null
                        Return Await embedHandler.errorEmbed("Audio - Leave", "Player not found")
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
                        _channel = Nothing
                        Return embed.Build
                    End If

                Else
                    Return Await embedHandler.victoriaInvalidUsageEmbed(channel)

                End If

            Catch ex As InvalidOperationException
                Return embedHandler.errorEmbed("Audio - Leave", ex.Message).Result
            End Try

        Else
            Return Await embedHandler.victoriaEmptyChannel()
        End If

    End Function

    Public Shared Async Function setVolumeAsync(guild As IGuild, vol As Integer, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel()
        End If

    End Function

    Public Shared Async Function togglePauseAsync(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel()
        End If

    End Function

    Public Shared Async Function skipTrack(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel()

        End If
    End Function

    Public Shared Async Function listTracks(guild As IGuild, channel As ITextChannel) As Task(Of Embed)
        If _channel IsNot Nothing Then

            If channel.Id = _channel.Id Then

                Try
                    Dim page = String.Empty

                    Dim player = _lavaNode.GetPlayer(guild)

                    Dim title = $"**{player.Track.Title}**{Environment.NewLine}*{player.Track.Url}*{Environment.NewLine}------------------------------------------------------------{Environment.NewLine}"
                    page = title

                    If player Is Nothing Then 'Check if player is null
                        Return Await embedHandler.errorEmbed("Music - List", "No player found.")
                    End If

                    If player.PlayerState = PlayerState.Playing Or player.PlayerState = PlayerState.Paused Then

                        If player.Queue.Count < 1 And player.Track IsNot Nothing Then
                            Return Await embedHandler.victoriaNowPlayingEmbed(player.Track)
                        Else

                            Dim trackNum = 1
                            Dim listNum = 0
                            Dim avgNum = player.Queue.Count / _listLimit
                            Dim queueAvg = MathF.Round(avgNum)

                            For Each track As LavaTrack In player.Queue
                                If page.Length > 1024 Then
                                    Exit For
                                End If
                                If listNum = queueAvg Then
                                    Exit For
                                End If

                                page += $"**[{trackNum}]** `{track.Title}`{Environment.NewLine}"
                                trackNum += 1
                                listNum += 1
                            Next
                            Dim embed = New EmbedBuilder With {
                                .Title = $"The next {queueAvg}/{player.Queue.Count} songs",
                                .Description = page,
                                .ThumbnailUrl = YouTube.GetThumbnail(player.Track.Url),
                                .Color = New Color(_utils.RandomEmbedColor),
                                .Footer = New EmbedFooterBuilder With {
                                    .IconUrl = YouTube.GetThumbnail(player.Track.Url),
                                    .Text = $"{player.Queue.Count} queued songs"
                                }
                            }


                            Return embed.Build
                        End If
                    End If

                Catch ex As Exception
                    Return embedHandler.errorEmbed("Music - List", ex.Message).Result
                End Try

            End If

        Else
            Return Await embedHandler.victoriaEmptyChannel()
        End If
    End Function

    Public Shared Async Function clearTracks(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel
        End If

    End Function

    Public Shared Async Function stopAsync(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel
        End If

    End Function

    Public Shared Async Function restartAsync(guild As IGuild, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel
        End If

    End Function

    Public Shared Async Function seekAsync(guild As IGuild, timeSpan As TimeSpan, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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
                        .ThumbnailUrl = If(YouTube.GetThumbnail(player.Track.Url), "https://i.imgur.com/Kl2Qrd2.png"),
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

        Else
            Return Await embedHandler.victoriaEmptyChannel
        End If

    End Function

    Public Shared Async Function shuffleAsync(guild As IGuild, voiceState As IVoiceState, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel
        End If

    End Function

    Public Shared Async Function nowPlayingAsync(g As IGuild, ctx As SocketCommandContext, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

            Try
                If channel.Id = _channel.Id Then

                    Dim player = _lavaNode.GetPlayer(g)
                    If player Is Nothing Then
                        Return embedHandler.errorEmbed("Audio - NowPlaying", "Player could not be found").Result
                    End If

                    If player.PlayerState = PlayerState.Playing Or player.PlayerState = PlayerState.Paused Then
                        Return embedHandler.victoriaNowPlayingEmbed(player.Track).Result
                    Else
                        Return embedHandler.victoriaNoQueueEmbed().Result
                    End If
                Else
                    Return embedHandler.victoriaInvalidUsageEmbed(channel).Result

                End If

            Catch ex As Exception
                Return embedHandler.errorEmbed("Audio", ex.Message).Result
            End Try

        Else
            Return Await embedHandler.victoriaEmptyChannel
        End If

    End Function

    Public Shared Async Function repeatAsync(g As IGuild, channel As ITextChannel) As Task(Of Embed)

        If _channel IsNot Nothing Then

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

        Else
            Return Await embedHandler.victoriaEmptyChannel
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