Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Discord.Addons.Interactive
Imports Microsoft.Extensions.DependencyInjection

<Name("Admin")>
<Summary("Commands only for admins.")>
Public Class Admin
    Inherits InteractiveBase(Of SocketCommandContext)
    Private ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    <Command("purge")>
    <Summary("Purge messages from the last 14 days.")>
    <RequireBotPermission(GuildPermission.ManageMessages)>
    <Remarks("\purge <number> | 1-1000 could leave blank to delete 1000 messages.")>
    Public Async Function purgeCmd(Optional amount As Integer = 1000) As Task(Of RuntimeResult)

        Try
            If DirectCast(Context.Message.Author, SocketGuildUser).GuildPermissions.ManageMessages Then
                If amount <= 0 Then
                    Await ReplyAsync("The amount of messages to remove must be positive.")
                    Return Ok()
                End If

                If amount > 1000 Then
                    Await ReplyAsync("Please keep the number below 1000")
                    Return Ok()
                End If

                Dim messages = Await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount + 1).FlattenAsync()
                Dim filteredMessages = messages.Where(Function(x) (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14)
                Dim count = filteredMessages.Count()

                If count = 0 Then
                    Await ReplyAsync("Nothing to delete.")
                Else
                    Await TryCast(Context.Channel, ITextChannel).DeleteMessagesAsync(filteredMessages)

                    Dim embed = New EmbedBuilder With {
                        .Title = "I've done my job",
                        .Description = $":recycle: {count} {If(count > 1, "messages have", "message has")} been dealt with.",
                        .Color = Color.Green,
                        .ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl
                    }

                    Await ReplyAndDeleteAsync("", embed:=embed.Build, timeout:=New TimeSpan(0, 0, 5))
                    Threading.Thread.Sleep(1000)
                    Await TryCast(Context.Channel, ITextChannel).DeleteMessageAsync(Context.Message.Id)
                    Return Ok()
                End If
            Else
                Await Context.Message.Channel.SendMessageAsync("You do not have the required permissions to use this command.")
                Return Ok()
            End If
        Catch ex As Exception
            loggingHandler.ErrorLog("admin", ex.Message)
            Return Ok()
        End Try

        Return Ok()
    End Function

    <Command("slowmode")>
    <Summary("Slows down the channel to a certain interval.")>
    <RequireUserPermission(GuildPermission.ManageChannels)>
    <RequireBotPermission(GuildPermission.ManageChannels)>
    <Remarks("\slowmode 5 | slowmode with a 5 second interval - leave empty to remove slowmode.")>
    Public Async Function slowMode(Optional interval As Integer = 0) As Task
        Await TryCast(Context.Channel, SocketTextChannel).ModifyAsync(Sub(x) x.SlowModeInterval = interval)
        Dim description = If(interval = 0, ":clock1: slowmode has been removed", $":clock1: Slowmode interval has been adjusted to {interval} seconds")
        Dim embed = New EmbedBuilder With {
            .Title = "Slowmode interval adjustment",
            .Description = description,
            .Color = Color.Green,
            .ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl
        }

        Await ReplyAsync(embed:=embed.Build)
    End Function

    <Command("ban")>
    <Summary("Ban the mentioned user.")>
    <RequireUserPermission(GuildPermission.BanMembers)>
    <RequireBotPermission(GuildPermission.BanMembers)>
    <Remarks("\ban @user <reason> | reason is optional")>
    Public Async Function cmdBanMember(user As IGuildUser, <Remainder> Optional reason As String = Nothing) As Task

        Try

            Dim settings = Lyuze.Settings.Data
            Dim channel = Context.Guild.GetTextChannel(settings.IDs.KickId)
            If reason Is Nothing Then
                reason = "No reason given."
            End If

            Await Context.Guild.AddBanAsync(user, 1, reason)
            Dim embed As New EmbedBuilder With {
                .Description = $":hammer: {user.Mention} was banned by {Context.User.Mention}.{Environment.NewLine} **Reason** {reason}",
                .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
                .Color = Color.DarkRed,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "User Ban Log"
                }
            }

            channel.SendMessageAsync(embed:=embed.Build)

        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try

    End Function

    <Command("kick")>
    <Summary("Kicks the mentioned user.")>
    <RequireUserPermission(GuildPermission.KickMembers)>
    <RequireBotPermission(GuildPermission.KickMembers)>
    <Remarks("\kick @user <reason> | reason is optional")>
    Public Async Function cmdKickMember(user As IGuildUser, <Remainder> Optional reason As String = Nothing) As Task

        Try
            Dim settings = Lyuze.Settings.Data
            Dim channel = Context.Guild.GetTextChannel(settings.IDs.KickId)
            If reason Is Nothing Then
                reason = "No reason given."
            End If

            Await user.KickAsync(reason)
            Dim embed As New EmbedBuilder With {
                .Description = $":athletic_shoe: {user.Mention} was kicked by {Context.User.Mention}.{Environment.NewLine} **Reason** {reason}",
                .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
                .Color = Color.Red,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "User Kick Log"
                }
            }

            channel.SendMessageAsync(embed:=embed.Build)

        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try

    End Function

    <Command("id")>
    <Summary("Returns the ID of the mentioned user.")>
    <RequireUserPermission(GuildPermission.KickMembers)>
    <Remarks("\id @user")>
    Public Async Function getId(user As IGuildUser) As Task
        Dim m = Context.Message
        Dim u = Context.User
        Dim g = Context.Guild
        Dim embed As New EmbedBuilder With {
            .Author = New EmbedAuthorBuilder With {
                .IconUrl = u.GetAvatarUrl,
                .Name = u.Username
            },
                .Title = $"{user.Username}'s Id",
                .Description = user.Id,
                .Color = New Color(_utils.randomEmbedColor),
                .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
                .Timestamp = m.Timestamp,
                .Footer = New EmbedFooterBuilder With {
                        .Text = "ID Data",
                        .IconUrl = g.IconUrl
                    }
            }

        Await Context.Message.Author.SendMessageAsync("", False, embed.Build())

    End Function

End Class

<Name("Owner")>
<Summary("Commands only for the bot owner.")>
Public Class Owner
    Inherits ModuleBase(Of SocketCommandContext)
    Private ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    <Command("hide")>
    <Summary("Hides command window.")>
    <RequireOwner>
    Public Async Function cmdHide() As Task
        Dim author = Context.Message.Author
        Dim settings = Lyuze.Settings.Data

        If DirectCast(author, SocketGuildUser).Id = settings.IDs.OwnerId Then
            _utils.winHide()

            Await Context.Channel.SendMessageAsync($"Don't worry I'll still be here.")
        Else
            Await Context.Channel.SendMessageAsync("This is only for the bot owner.")
        End If
    End Function

    <Command("show")>
    <Summary("Shows command window.")>
    <RequireOwner>
    Public Async Function cmdShow() As Task
        Dim author = Context.Message.Author
        Dim settings = Lyuze.Settings.Data

        If DirectCast(author, SocketGuildUser).Id = Settings.IDs.OwnerId Then
            _utils.winShow()

            Await Context.Channel.SendMessageAsync($"{author.Mention} hello again :smiley:.")
        Else
            Await Context.Channel.SendMessageAsync("This is only for the bot owner.")
        End If
    End Function

    <Command("kill", RunMode.Async)>
    <Summary("Kills the bot process.")>
    <RequireOwner>
    Public Async Function cmdKill() As Task
        Dim author = Context.Message.Author
        Dim settings = Lyuze.Settings.Data

        If DirectCast(author, SocketGuildUser).Id = Settings.IDs.OwnerId Then
            For Each p As Process In Process.GetProcesses
                If p.ProcessName = "javaw" Then
                    Try
                        p.Kill()
                    Catch ex As Exception
                        Continue For
                    End Try
                End If
                If p.ProcessName = "LyuzeBOT" Then
                    Try
                        Await Context.Channel.SendMessageAsync("Goodbye for now.")
                        p.Kill()
                    Catch ex As Exception
                        Continue For
                    End Try
                End If
            Next
        Else
            Await Context.Channel.SendMessageAsync("This is only for the bot owner.")
        End If
    End Function

End Class