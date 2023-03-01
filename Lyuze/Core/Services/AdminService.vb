﻿Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class AdminService
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _settings As Settings = serviceHandler.provider.GetRequiredService(Of Settings)
    Public Shared Async Function Report(id As ULong, ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Dim message = ctx.Channel.GetMessageAsync(id)
            Dim [embed] = New EmbedBuilder With {
                .Title = ":pencil: Report",
                .Description = $"Author: ***{message.Result.Author}***{Environment.NewLine}Message:**{message.Result.Content}**{Environment.NewLine}------------------------------
                    {Environment.NewLine}By:{ctx.Message.Author.Mention}",
                .Color = Color.Red,
                .ThumbnailUrl = If(message.Result.Author.GetAvatarUrl, message.Result.Author.GetDefaultAvatarUrl),
                .Footer = New EmbedFooterBuilder With {
                    .IconUrl = ctx.Client.CurrentUser.GetAvatarUrl,
                    .Text = "Submitted Report"
                }
            }
            Try
                ctx.User.SendMessageAsync(embed:=[embed].Build)
            Catch ex As Exception

            End Try
            ctx.Message.DeleteAsync()
            Return [embed].Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Gen - Report", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function Slowmode(interval As Integer, ctx As SocketCommandContext) As Task(Of Embed)
        Await TryCast(ctx.Channel, SocketTextChannel).ModifyAsync(Sub(x) x.SlowModeInterval = interval)
        Dim description = If(interval = 0, ":clock1: slowmode has been removed", $":clock1: Slowmode interval has been adjusted to {interval} seconds")
        Dim embed = New EmbedBuilder With {
            .Title = "Slowmode interval adjustment",
            .Description = description,
            .Color = Color.Green,
            .ThumbnailUrl = ctx.Client.CurrentUser.GetAvatarUrl
        }

        Return embed.Build
    End Function

    Public Shared Async Function Ban(user As IGuildUser, <Remainder> reason As String, ctx As SocketCommandContext) As Task(Of Embed)
        Await ctx.Guild.AddBanAsync(user, 1, reason)
        Dim embed As New EmbedBuilder With {
            .Description = $":hammer: {user.Mention} was banned by {ctx.User.Mention}.{Environment.NewLine} **Reason** {reason}",
            .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
            .Color = Color.DarkRed,
            .Footer = New EmbedFooterBuilder With {
                .Text = "User Ban Log"
            }
        }

        Return embed.Build
    End Function

    Public Shared Async Function Kick(user As IGuildUser, <Remainder> reason As String, ctx As SocketCommandContext) As Task(Of Embed)
        Dim embed As New EmbedBuilder With {
            .Description = $":athletic_shoe: {user.Mention} was kicked by {ctx.User.Mention}.{Environment.NewLine} **Reason** {reason}",
            .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
            .Color = Color.Red,
            .Footer = New EmbedFooterBuilder With {
                .Text = "User Kick Log"
            }
        }

        Return embed.Build
    End Function

    Public Shared Async Function ID(user As IGuildUser, ctx As SocketCommandContext) As Task(Of Embed)
        Dim m = ctx.Message
        Dim u = ctx.User
        Dim g = ctx.Guild

        Dim embed As New EmbedBuilder With {
            .Author = New EmbedAuthorBuilder With {
                .IconUrl = u.GetAvatarUrl,
                .Name = u.Username
            },
                .Title = $"{user.Username}'s Id",
                .Description = user.Id,
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
                .Timestamp = m.Timestamp,
                .Footer = New EmbedFooterBuilder With {
                        .Text = "ID Data",
                        .IconUrl = g.IconUrl
                    }
            }
        ctx.Message.DeleteAsync()
        Return embed.Build
    End Function

    Public Shared Async Function ChangeBackground(user As SocketGuildUser, url As String) As Task(Of Embed)
        Try
            Dim p As PlayerModel = Await Player.GetUser(user)
            p.Background = url
            Await Player.UpdateUser(user, p)

            Dim embed As New EmbedBuilder With {
                .Title = $"Background of {user.Username} has been changed.",
                .Description = "Background has been changed to the image below.",
                .ImageUrl = url,
                .Color = New Color(Await _imgs.RandomColorFromURL(url))
            }

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Admin - Change Background", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function Infraction(user As SocketGuildUser, msg As ULong, ctx As SocketCommandContext) As Task(Of Embed)

        If user.Id = ctx.Channel.GetMessageAsync(msg).Result.Author.Id Then
            Return embedHandler.errorEmbed("Infraction", "Can't report your own message.").Result
        End If

        Try
            Dim p As PlayerModel = Await Player.GetUser(user)
            If p.InfractionCount + 1 < 3 Then
                p.InfractionMessages.Add(msg)
                p.InfractionCount += 1
                Dim em As New EmbedBuilder With {
                    .Title = "Infraction given",
                    .Description = $"An infraction has been given to {user.Mention}. Infraction has been sent to a mod channel.",
                    .Color = New Color(_utils.ConvertToDiscordColor("#dc143c")),
                    .Footer = New EmbedFooterBuilder With {
                        .Text = "Infraction Given"
                    }
                }
                Player.UpdateUser(user, p)

                'Dim reportchannel = ctx.Guild.GetTextChannel(_settings.IDs.ReportId)
                'reportchannel.SendMessageAsync(embed:=)

                Return em.Build
            Else
                Dim infractionMsgs As String
                For Each i In p.InfractionMessages
                    infractionMsgs += $"{i}{Environment.NewLine}"
                Next

                Dim e As New EmbedBuilder With {
                    .Title = $"Max Infractions For @{If(user.Nickname, user.Username)} Has Been Reached",
                    .Color = New Color(_utils.ConvertToDiscordColor("#dc143c")),
                    .Description = infractionMsgs,
                    .Footer = New EmbedFooterBuilder With {
                        .Text = "Infractions"
                    }
                }
                Player.UpdateUser(user, p)
                Return e.Build
            End If
        Catch ex As Exception

        End Try
    End Function
End Class
