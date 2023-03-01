Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Discord.Addons.Interactive
Imports Microsoft.Extensions.DependencyInjection
Imports System.Drawing

<Name("Admin")>
<Summary("Commands only for admins.")>
Public Class Admin
    Inherits InteractiveBase(Of SocketCommandContext)


    <Command("purge")>
    <Summary("Purge messages from the last 14 days.")>
    <RequireBotPermission(GuildPermission.ManageMessages)>
    <Remarks("\purge <number> | 1-1000 could leave blank to delete 1000 messages.")>
    Public Async Function purgeCmd(Optional amount As Integer = 1000) As Task(Of RuntimeResult)
        'Changes bot's avatar > Context.Client.CurrentUser.ModifyAsync(Function(x) x.Avatar = TryCast("a", Drawing.Image))
        'Dim auditLogs = Context.Guild.GetAuditLogsAsync(100, actionType:=ActionType.MessageDeleted)

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
                    Context.Message.DeleteAsync()
                    Await ReplyAndDeleteAsync("Nothing to delete.", timeout:=New TimeSpan(0, 0, 3))
                Else
                    Await TryCast(Context.Channel, ITextChannel).DeleteMessagesAsync(filteredMessages)

                    Dim embed = New EmbedBuilder With {
                        .Title = "I've done my job",
                        .Description = $":recycle: {count} {If(count > 1, "messages have", "message has")} been dealt with.",
                        .Color = Discord.Color.Green,
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
            Dim _settings = Lyuze.Settings.Data

            If _settings.IDs.ErrorId = 0 Then
                loggingHandler.LogCriticalAsync("Admin", ex.Message)
            Else
                Dim chnl = Context.Guild.GetTextChannel(_settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("Admin - Purge", ex.Message).Result)
            End If
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
        Await ReplyAsync(embed:=Await AdminService.Slowmode(interval, Context))
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

            Await ReplyAsync(embed:=Await AdminService.Ban(user, reason, Context))
        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try

    End Function

    <Command("kick")>
    <Summary("Kicks the mentioned user.")>
    <RequireUserPermission(GuildPermission.KickMembers)>
    <RequireBotPermission(GuildPermission.KickMembers)>
    <Remarks("\kick @user <reason> | reason is optional")>
    Public Async Function cmdKickMember(user As IGuildUser, <Remainder> Optional reason As String = "No reason given.") As Task
        Try
            Dim settings = Lyuze.Settings.Data
            Dim channel = Context.Guild.GetTextChannel(settings.IDs.KickId)
            Await user.KickAsync(reason)
            Await ReplyAsync(embed:=Await AdminService.Kick(user, reason, Context))
        Catch ex As Exception
            ReplyAsync(ex.Message)
        End Try

    End Function

    <Command("id")>
    <Summary("Returns the ID of the mentioned user.")>
    <RequireUserPermission(GuildPermission.KickMembers)>
    <Remarks("\id @user")>
    Public Async Function getId(user As IGuildUser) As Task
        Await ReplyAsync(embed:=Await AdminService.ID(user, Context))
    End Function

    <Command("changebackground")>
    <[Alias]("cbg")>
    <Summary("Change the background of someone's profile")>
    <RequireUserPermission(GuildPermission.KickMembers)>
    <Remarks("\cbg @user https://i.imgur.com/5c0jfZS.png")>
    Public Async Function ChangeBackground(user As IGuildUser, url As String) As Task
        Await ReplyAsync(embed:=Await AdminService.ChangeBackground(user, url))
    End Function

    <Command("infraction")>
    <Summary("Give a user a infraction, if max is reached it will ban them for 1 day")>
    <Remarks("\infraction @user <message_id>")>
    Public Async Function Infraction(user As SocketGuildUser, msgid As ULong) As Task

        Await ReplyAsync(embed:=Await AdminService.Infraction(user, msgid, Context))

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
        _utils.winHide()
        Await ReplyAsync($"Don't worry I'll still be here.")
    End Function

    <Command("show")>
    <Summary("Shows command window.")>
    <RequireOwner>
    Public Async Function cmdShow() As Task
        _utils.winShow()
        Await Context.Channel.SendMessageAsync($"{Context.Message.Author.Mention} hello again :smiley:.")
    End Function

    <Command("kill", RunMode.Async)>
    <Summary("Kills the bot process.")>
    <RequireOwner>
    Public Async Function cmdKill() As Task
        For Each p As Process In Process.GetProcesses
            If p.ProcessName = "javaw" Then
                Try
                    p.Kill()
                Catch ex As Exception
                    Continue For
                End Try
            End If
        Next
        For Each p As Process In Process.GetProcesses
            If p.ProcessName = "LyuzeBOT" Then
                Try
                    Await ReplyAsync("Goodbye for now.")
                    p.Kill()

                Catch ex As Exception
                    Continue For
                End Try
            End If
        Next
    End Function

End Class