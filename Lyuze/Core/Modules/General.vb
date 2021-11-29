Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Discord.Addons.Interactive
Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

<Name("Misc")>
<Summary("Commands that don't deserve their own place.")>
Public Class General
    Inherits InteractiveBase(Of SocketCommandContext)
    Private ReadOnly _utils As MasterUtils

    Public Sub New(utils As MasterUtils)
        _utils = utils
    End Sub

    <Command("nickname")>
    <[Alias]("nick")>
    <RequireBotPermission(GuildPermission.ManageNicknames)>
    <Summary("Change your nickname or of another user if you have the required permissions.")>
    <Remarks("\nick i_am_a_nickname | @user newNickname - for any spaces must use underscores.")>
    Public Async Function nick(Optional user As SocketGuildUser = Nothing, Optional n As String = Nothing) As Task
        Dim m = Context.Message.Channel
        Dim name = n
        Try

            'If user is nothing user will be the author of the message
            If user Is Nothing Then
                user = Context.Message.Author
            End If

            If n.Contains("_") Then
                name = n.Replace("_", " ")
            End If
            If name Is Nothing Then
                name = Context.Message.Author.Username
            End If

            'Check if the user option is filled.
            If user IsNot Nothing Then
                'If filled, check if it's not the message author.
                If user IsNot Context.Message.Author Then
                    'If it's not, check if user has the required permissions to change nicknames.
                    If user.GuildPermissions.ChangeNickname Then
                        Await user.ModifyAsync(Sub(u) u.Nickname = name)
                        Await m.SendMessageAsync($"{user.Mention}'s Nickname has been changed.")
                        Return
                    End If
                    Await m.SendMessageAsync("Since you mentioned a user you do not have the permissions to change the nickname.")
                    Return
                End If
            End If

            'This SHOULD be executed if there is no user and the user becomes the author.
            Await user.ModifyAsync(Sub(u) u.Nickname = name)
            Await m.SendMessageAsync($"{user.Mention}'s Nickname has been changed.")

        Catch ex As Exception
            loggingHandler.ErrorLog("Misc", ex.Message)
        End Try

    End Function

    <Command("emotes")>
    <Summary("Returns all available guild emotes.")>
    Public Async Function guildEmotesCmd() As Task 'Rework for pagination
        Dim m = Context.Message
        Dim u = Context.User
        Dim g = Context.Guild
        Dim c = Context.Client

        Try

            Dim embed As New EmbedBuilder With {
    .Title = $"All guild emotes for usage do :EmoteName:",
    .ImageUrl = "https://i.imgur.com/vc241Ku.jpeg",
    .Description = "The full list of our custom guild emotes",
    .Color = New Color(_utils.randomEmbedColor),
    .ThumbnailUrl = g.IconUrl,
    .Timestamp = Context.Message.Timestamp,
    .Footer = New EmbedFooterBuilder With {
            .Text = "Emote Data",
            .IconUrl = g.IconUrl
        }
    }


            Dim emotes As String = ""
            Dim row As Integer = 0
            For Each emote As Emote In DirectCast(Context.Message.Channel, IGuildChannel).Guild.Emotes
                If emotes.Length + emote.Name.Length + 5 > 256 Then
                    row += 1
                    embed.AddField($"List #{row}", emotes, True)

                    emotes = String.Empty
                End If

                emotes = emotes + "<:" + emote.Name + $":{emote.Id}>" + $" ***{emote.Name}***" + Environment.NewLine
            Next
            embed.AddField($"List #{row + 1}", emotes, True)

            Await Context.Message.Channel.SendMessageAsync("", False, embed.Build())

        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try

    End Function

    <Command("report")>
    Public Async Function Report(id As ULong) As Task
        Await ReplyAsync(embed:=ModService.Report(id, Context).Result)
    End Function

    <Command("test")>
    Public Async Function test() As Task
        Try
            Dim settings = Lyuze.Settings.Data

            Dim t = If(settings.IDs.ReportId.ToString, "n/a")
            Await ReplyAsync(t)
        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try
    End Function

End Class
