﻿Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class embedHandler

    Private Shared ReadOnly _imgs As Images = serviceHandler.provider.GetRequiredService(Of Images)
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function basicEmbed(src As String, msg As String) As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"{src}") _
            .WithDescription($"{msg}") _
            .WithColor(New Color(_utils.RandomEmbedColor())) _
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

    Public Shared Async Function ColdWarZombiesClass(user As SocketUser, weapon As String, optic As String, muzzle As String, barrel As String, body As String, under As String, magazine As String, handle As String, stock As String) As Task(Of Embed)
        Dim embed As New EmbedBuilder With {
            .Title = $"{user.Username}'s Class",
            .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
            .Description = $"{user.Mention}'s Randomly generated class. Have fun!",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .IconUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
                .Text = "Random Cold War Zombies Class"
            }
        }
        embed.AddField("Weapon", weapon, True)
        embed.AddField("Optic", optic, True)
        embed.AddField("Muzzle", muzzle, True)
        embed.AddField("Barrel", barrel, True)
        embed.AddField("Body", body, True)
        embed.AddField("UnderBarrel", under, True)
        embed.AddField("Magazine", magazine, True)
        embed.AddField("Handle", handle, True)
        embed.AddField("Stock", stock, True)


        Return embed.Build
    End Function

    Public Shared Async Function ProfileEmbed(user As SocketGuildUser, ctx As SocketCommandContext, p As PlayerModel, lvl As LevelingSystem) As Task(Of Embed)
        Dim embed As New EmbedBuilder With {
            .Title = $"{user.Username}'s Profile | Level - {p.Level}",
            .ImageUrl = p.Background,
            .Color = New Color(Await _imgs.RandomColorFromURL(p.Background)),
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
        embed.AddField("XP",
                       $"{p.XP}/{lvl.LevelEquation(p.Level)}", True)
        embed.AddField("Account Creation",
                       user.CreatedAt.DateTime.Date.ToShortDateString, True)
        embed.AddField("Joined Server",
                       If(user.JoinedAt.Value.DateTime.ToShortDateString, "N/A"), True)
        embed.AddField("Current Activity",
                       If(user.Activity, "N/A"), True)
        embed.AddField("Favorite Character",
                       p.FavChar, True)
        embed.AddField("About Me",
                       p.AboutMe)

        Return embed.Build
    End Function

End Class
