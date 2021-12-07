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
        Try

            Dim role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
            Dim embed As New EmbedBuilder With {
            .Title = $"Info on {role.Name}",
            .Color = role.Color,
            .ThumbnailUrl = Context.Client.CurrentUser.GetAvatarUrl
            }
            embed.AddField("Position", role.Position, True)
            embed.AddField("Admin", If(role.Permissions.Administrator, "Yes", "No"), True)
            embed.AddField("Members", role.Members.Count, True)
            embed.AddField("Mentionable", role.IsMentionable, True)
            embed.AddField("Creation Date", role.CreatedAt.DateTime.ToShortDateString(), True)
            Await Context.Channel.SendMessageAsync(embed:=embed.Build)

        Catch ex As Exception
            loggingHandler.ErrorLog("Roles", ex.Message)
        End Try

    End Function

    <Command("addrole")>
    <[Alias]("arole")>
    <Summary("Add a role to yourself")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function addRole(Optional user As SocketGuildUser = Nothing) As Task
        Dim role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
        Dim bots = Context.Guild.GetRole(729128623818276947)
        Dim god = Context.Guild.GetRole(691099101172858982)
        Dim [mod] = Context.Guild.GetRole(711113596272115712)
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

            If role Is bots Then
                Context.Channel.SendMessageAsync("This role is only for bots.")
                Return
            End If
            If role Is god Or role Is [mod] Then
                If Not _user.GuildPermissions.BanMembers Then
                    Context.Channel.SendMessageAsync($"You can not add {role.Name} to {user.Mention}.")
                    Return
                End If
            End If

            'Dim user = Context.Guild.GetUser(Context.Message.MentionedUsers.First.Id)

            Await user.AddRoleAsync(role)
            Await Context.Channel.SendMessageAsync($"Added {role.Name} to {user.Mention}")

        Catch ex As Exception
            loggingHandler.ErrorLog("Roles", ex.Message)
        End Try

    End Function

    <Command("removerole")>
    <[Alias]("rrole")>
    <Summary("Remove a role from yourself")>
    <Remarks("\grole @role")>
    <RequireBotPermission(GuildPermission.ManageRoles)>
    Public Async Function removeRole(Optional user As SocketGuildUser = Nothing) As Task
        Dim role = Context.Guild.GetRole(Context.Message.MentionedRoles.First.Id)
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
            loggingHandler.ErrorLog("Roles", ex.Message)
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
