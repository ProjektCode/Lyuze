Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket

NotInheritable Class RolesService

    Public Shared Async Function AddRole(user As SocketGuildUser, role As SocketRole, ctx As SocketCommandContext) As Task(Of Embed)
        Dim settings = Lyuze.Settings.Data
        Dim errorChnl = ctx.Guild.GetTextChannel(settings.IDs.ErrorId)
        Try
            If role.Position > user.Roles.FirstOrDefault.Position Then
                Return embedHandler.errorEmbed("Roles - AddRole", "This role is not available.").Result
            End If

            Await user.AddRoleAsync(role)

            Dim embed = New EmbedBuilder With {
                .Title = user.Username,
                .Description = $"Added {role.Mention} to {user.Mention}",
                .Color = Color.Green,
                .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl)
            }

            Return embed.Build
        Catch ex As Exception

            If settings.IDs.ErrorId = 0 Then
                loggingHandler.LogCriticalAsync("roles", ex.Message)
            Else
                errorChnl.SendMessageAsync(embed:=embedHandler.errorEmbed("Roles - AddRole", "Attempting to add a forbidden role.").Result)
            End If

        End Try

    End Function

    Public Shared Async Function GetRole(role As SocketRole, ctx As SocketCommandContext) As Task(Of Embed)
        Dim settings = Lyuze.Settings.Data
        Dim errorChnl = ctx.Guild.GetTextChannel(settings.IDs.ErrorId)
        Try
            Dim embed As New EmbedBuilder With {
            .Title = $"Info on {role.Name}",
            .Color = role.Color,
            .ThumbnailUrl = If(ctx.Client.CurrentUser.GetAvatarUrl, ctx.Client.CurrentUser.GetDefaultAvatarUrl)
            }
            embed.AddField("Admin", If(role.Permissions.Administrator, "Yes", "No"), True)
            embed.AddField("Position", role.Position, True)
            embed.AddField("Members", role.Members.Count, True)
            embed.AddField("Mentionable", role.IsMentionable, True)
            embed.AddField("Managed By Discord", role.IsManaged, True)
            embed.AddField("Creation Date", role.CreatedAt.DateTime.ToShortDateString(), True)

            Return embed.Build
        Catch ex As Exception

            If settings.IDs.ErrorId = 0 Then
                loggingHandler.LogCriticalAsync("roles", ex.Message)
            Else
                errorChnl.SendMessageAsync(embed:=embedHandler.errorEmbed("Roles - GetRole", ex.Message).Result)
            End If

        End Try
    End Function

End Class