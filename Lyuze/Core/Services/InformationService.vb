﻿Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class InformationService

    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function GetProfile(user As IGuildUser, ctx As SocketCommandContext) As Task(Of Embed)

        Dim url As String = "https://raw.githubusercontent.com/Projekt-Dev/Projekt-Dev/main/Profile.png"
        Try

            Dim embed As New EmbedBuilder With {
                .Title = $"{user.Username}'s Profile",
                .ImageUrl = url,
                .Color = New Color(_imgs.RandomColorFromURL(url).Result),
                .ThumbnailUrl = user.GetAvatarUrl(ImageFormat.Auto, 256),
                .Timestamp = ctx.Message.Timestamp,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Profile Data",
                    .IconUrl = ctx.Guild.IconUrl
                }
            }
            If user.Nickname IsNot Nothing Then
                embed.AddField("Nickname",
                                user.Nickname, True)
            End If
            embed.AddField("Current status",
                           user.Status, True)
            embed.AddField("Account Creation",
                           user.CreatedAt.DateTime.Date.ToShortDateString, True)
            embed.AddField("Joined Server",
                           If(user.JoinedAt.Value.DateTime.ToShortDateString, "N/A"), True)
            embed.AddField("Current Activity",
                           If(user.Activity, "N/A"), True)
            embed.AddField("Avatar URL",
                           user.GetAvatarUrl(ImageFormat.Auto, 512))

            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Info - Profile", ex.Message).Result
        End Try
    End Function

    Public Shared Async Function GetServer(g As IGuild, ctx As SocketCommandContext) As Task(Of Embed)
        Dim url As String = "https://raw.githubusercontent.com/Projekt-Dev/Projekt-Dev/main/Profile.png"

        Try

            Dim embed As New EmbedBuilder With {
    .Title = $"{g.Name}'s information",
    .ImageUrl = url,
    .Color = New Color(_imgs.RandomColorFromURL(url).Result),
    .ThumbnailUrl = g.IconUrl,
    .Timestamp = ctx.Message.Timestamp,
    .Footer = New EmbedFooterBuilder With {
            .Text = "Server Data",
            .IconUrl = g.IconUrl
        }
    }

            embed.AddField("Server's Creation Date", g.CreatedAt.DateTime.ToShortDateString(), True)
            embed.AddField("Preferred Locale", g.PreferredLocale, True)
            embed.AddField("Nitro Tier", g.PremiumTier, True)
            embed.AddField("Nitro Boosters", g.PremiumSubscriptionCount, True)
            embed.AddField("Role Count", g.Roles.Count, True)
            embed.AddField("User Count", ctx.Guild.MemberCount, True)
            embed.AddField("Emote Count", g.Emotes.Count, True)
            embed.AddField("AFK Timeout Time", $"{_utils.timeOut(g)} minutes", True)

            Return embed.Build()

        Catch ex As Exception
            Return embedHandler.errorEmbed("Info - Server", ex.Message).Result
        End Try
    End Function

End Class