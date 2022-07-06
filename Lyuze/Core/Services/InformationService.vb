Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class InformationService

    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly rand As New Random
    Private Shared ReadOnly defaultImage As String = "https://i.imgur.com/Kl2Qrd2.png"
    Private Shared ReadOnly apiurl As String = "http://lyuze-api.projektcode.com"

    Public Shared Async Function GetProfile(user As IGuildUser, ctx As SocketCommandContext) As Task(Of Embed)
        Dim number = rand.Next(1, 3)
        Dim aurl = apiurl
        Dim url As New Uri($"{aurl}/images/banners/info/profile/user-{number}", UriKind.Absolute)
        Try

            Dim embed As New EmbedBuilder With {
                .Title = $"{user.Username}'s Profile",
                .ImageUrl = url.AbsoluteUri,
                .Color = New Color(Await _imgs.RandomColorFromURL(url.AbsoluteUri)),
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
        Dim number = rand.Next(1, 3)
        Dim aurl = apiurl
        Dim url As New Uri($"{aurl}/images/banners/info/server/server-{number}", UriKind.Absolute)

        Try

            Dim embed As New EmbedBuilder With {
                .Title = $"{g.Name}'s information",
                .ImageUrl = url.AbsoluteUri,
                .Color = New Color(Await _imgs.RandomColorFromURL(url.AbsoluteUri)),
                .ThumbnailUrl = If(g.IconUrl, defaultImage),
                .Timestamp = ctx.Message.Timestamp,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Server Data",
                    .IconUrl = If(g.IconUrl, defaultImage)
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