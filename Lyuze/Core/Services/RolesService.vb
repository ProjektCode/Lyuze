Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class RolesService

    Private Shared _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function AddRole(user As SocketGuildUser, role As SocketRole, ctx As SocketCommandContext) As Task(Of Embed)
        Dim settings = Lyuze.Settings.Data
        Dim errorChnl = ctx.Guild.GetTextChannel(settings.IDs.ErrorId)
        Try
            If role.Position > user.Roles.FirstOrDefault.Position Then
                Return embedHandler.errorEmbed("roles", "This role is not available.").Result
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

    Public Shared Async Function RemoveRole(user As SocketGuildUser, role As SocketRole, ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Await user.RemoveRoleAsync(role)

            Dim embed = New EmbedBuilder With {
            .Title = $"Removed Role",
            .Description = $"Removed {role.Mention} from {user.Mention}"
        }

            Return embed.Build
        Catch ex As Exception
            Dim _settings = Lyuze.Settings.Data

            If _settings.IDs.ErrorId = 0 Then
                loggingHandler.LogCriticalAsync($"roles", ex.Message)
            Else
                Dim chnl = ctx.Guild.GetTextChannel(_settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed($"Roles - Remove", ex.Message).Result)
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

    Public Shared Async Function CreateRole(name As String, color As String, ctx As SocketCommandContext) As Task(Of Embed)
        Dim settings = Lyuze.Settings.Data

        Try

            Dim _color = New Color(_utils.DiscordColor(color))
            Dim newRole = Await ctx.Guild.CreateRoleAsync(name, Nothing, color:=_color, False, Nothing, Nothing)
            Dim role As SocketRole = Nothing
            Dim roleList = ctx.Guild.Roles
            For Each a In roleList
                If a.Name = newRole.Name Then
                    role = ctx.Guild.GetRole(a.Id)
                End If
            Next

            Dim embed = New EmbedBuilder With {
                .Title = "New Role Created",
                .Color = _color,
                .Description = $"New role created named {role.Mention}.",
                .Footer = New EmbedFooterBuilder With {
                    .Text = $"Role created by {ctx.Message.Author}",
                    .IconUrl = ctx.Guild.CurrentUser.GetAvatarUrl
                }
            }

            Return embed.Build
        Catch ex As Exception
            If settings.IDs.ErrorId = Nothing Then
                Return embedHandler.errorEmbed("Roles - Create", $"Role with the name {name} already exists. Please chose a different name.").Result
            Else
                Dim chnl = ctx.Guild.GetTextChannel(settings.IDs.ErrorId)
                chnl.SendMessageAsync(embed:=embedHandler.errorEmbed("Roles - Create", $"Role with the name {name} already exists. Please chose a different name.").Result)
            End If
        End Try

    End Function

End Class