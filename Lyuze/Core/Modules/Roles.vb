Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket

<Name("Roles")>
<Summary("Get information on roles and get/remove roles.")>
Public Class Roles 'Change messages from string to embeds.
    Inherits ModuleBase(Of SocketCommandContext)


    <Command("getrole")>
    <[Alias]("grole")>
    <Summary("Get information on the mentioned role.")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function getRole() As Task
        Dim role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
        Await ReplyAsync(embed:=RolesService.GetRole(role, Context).Result)
    End Function

    <Command("addrole")>
    <[Alias]("arole")>
    <Summary("Add a role to yourself")>
    <Remarks("\arole @role @user | if your not a mod no need to mention a user since it'll default to yourself.")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function addRole(role As SocketRole, Optional user As SocketGuildUser = Nothing) As Task
        role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
        Dim _user = Context.Guild.GetUser(Context.Message.Author.Id)

        If _user.GuildPermissions.ManageRoles Then
            If user Is Nothing Then
                user = Context.Guild.GetUser(Context.Message.Author.Id)
            Else
                user = Context.Guild.GetUser(Context.Message.MentionedUsers.First.Id)
            End If
        Else
            user = Context.Guild.GetUser(Context.Message.Author.Id)
        End If

        Await ReplyAsync(embed:=RolesService.AddRole(user, role, Context).Result)
    End Function

    <Command("removerole")>
    <[Alias]("rrole")>
    <Summary("Remove a role from yourself")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function removeRole(role As SocketRole, Optional user As SocketGuildUser = Nothing) As Task
        role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
        Dim _user = Context.Guild.GetUser(Context.User.Id)

        Try

            If _user.GuildPermissions.ManageRoles Then
                If user Is Nothing Then
                    user = Context.Guild.GetUser(Context.Message.Author.Id)
                Else
                    user = Context.Guild.GetUser(Context.Message.MentionedUsers.First.Id)
                End If
            Else
                user = Context.Guild.GetUser(Context.Message.Author.Id)
            End If


            Await user.RemoveRoleAsync(role)
            Await Context.Channel.SendMessageAsync($"Removed *{role.Name}* from {user.Mention}")

        Catch ex As Exception
            Dim _settings = Lyuze.Settings.Data

            If _settings.IDs.ErrorId = 0 Then
                loggingHandler.LogCriticalAsync($"Roles - Remove", ex.Message)
            Else
                Dim chnl = Context.Guild.GetTextChannel(_settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"Roles - Remove", ex.Message).Result)
            End If
        End Try

    End Function

    <Command("createrole")>
    <[Alias]("crole")>
    <Summary("Creates a role if you have the required permissions.")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    <RequireUserPermission(GuildPermission.ManageRoles)>
    Public Async Function createRole(<Remainder> name As String) As Task
        Dim user = Context.Guild.GetUser(Context.User.Id)

        Await Context.Guild.CreateRoleAsync(name, Nothing, Color.Gold, False, Nothing, Nothing)
        Await Context.Channel.SendMessageAsync($"Created role named {name}")

    End Function

End Class
