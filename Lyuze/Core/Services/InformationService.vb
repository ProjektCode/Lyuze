Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class InformationService

    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _lvl As LevelingSystem = serviceHandler.provider.GetRequiredService(Of LevelingSystem)
    Private Shared ReadOnly rand As New Random
    Private Shared ReadOnly defaultImage As String = "https://i.imgur.com/Kl2Qrd2.png"
    Private Shared ReadOnly apiurl As String = "http://lyuze-api.projektcode.com"

    Public Shared Async Function GetProfile(user As SocketGuildUser, ctx As SocketCommandContext) As Task(Of Embed)
        Dim p As PlayerModel = Await Player.GetUser(user)
        Dim number = rand.Next(1, 3)
        Try
            If user.IsBot Then
                Return embedHandler.errorEmbed("Profile", "Profile you're trying to view is a bot.").Result
            End If


            Dim u = ctx.Guild.GetUser(p.DiscordID)
            If Not p.DiscordID = user.Id Then
                If Not p.PublicProfile.ToLower = "public" Then
                    Return embedHandler.errorEmbed("Profile", $"{u.Username}'s profile is set to private. You can not view it.").Result
                End If

                Return embedHandler.ProfileEmbed(u, ctx, p, _lvl).Result
            End If
            Return embedHandler.ProfileEmbed(user, ctx, p, _lvl).Result
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