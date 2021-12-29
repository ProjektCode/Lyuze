Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

<Name("Roles")>
<Summary("Get information on roles and get/remove roles.")>
Public Class Roles 'Change messages from string to embeds.
    Inherits ModuleBase(Of SocketCommandContext)
    Dim _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    <Command("getrole")>
    <[Alias]("grole")>
    <Summary("Get information on the mentioned role.")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function getRole() As Task
        Dim role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
        Await ReplyAsync(embed:=Await RolesService.GetRole(role, Context))
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

        Await ReplyAsync(embed:=Await RolesService.AddRole(user, role, Context))
    End Function

    <Command("removerole")>
    <[Alias]("rrole")>
    <Summary("Remove a role from yourself")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function removeRole(role As SocketRole, Optional user As SocketGuildUser = Nothing) As Task
        role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
        Dim _user = Context.Guild.GetUser(Context.User.Id)

        If _user.GuildPermissions.ManageRoles Then
            If user Is Nothing Then
                user = Context.Guild.GetUser(Context.Message.Author.Id)
            Else
                user = Context.Guild.GetUser(Context.Message.MentionedUsers.First.Id)
            End If
        Else
            user = Context.Guild.GetUser(Context.Message.Author.Id)
        End If

        Await ReplyAsync(embed:=Await RolesService.RemoveRole(user, role, Context))
    End Function

    <Command("createrole")>
    <[Alias]("crole")>
    <Summary("Creates a role if you have the required permissions.")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    <RequireUserPermission(GuildPermission.ManageRoles)>
    Public Async Function createRole(color As String, <Remainder> name As String) As Task
        Await ReplyAsync(embed:=Await RolesService.CreateRole(name, color, Context))
    End Function

End Class
