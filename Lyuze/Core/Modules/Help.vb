﻿Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Microsoft.Extensions.DependencyInjection

<Name("Help")>
<Summary("Get help on commands.")>
Public Class cmdHelp
	Inherits InteractiveBase(Of SocketCommandContext)
	Private ReadOnly _service As CommandService = serviceHandler.provider.GetRequiredService(Of CommandService)
	Private ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

	<Command("help")>
    <Summary("Gives a short description on all available commands | Can give a command name for some extra help - have to use full command name")>
    Public Async Function helpAsync(<Remainder> Optional name As String = Nothing) As Task

        If name Is Nothing Then

            Dim pages As New List(Of String)
            Dim user = Context.Guild.GetUser(Context.User.Id)

            For Each mods In _service.Modules

                Dim page = $"**{mods.Name}**{Environment.NewLine}*{mods.Summary}*{Environment.NewLine}{Environment.NewLine}"
                For Each command In mods.Commands
                    page += $"`\{command.Name}` - *{command.Summary}*{Environment.NewLine}"
                Next
                pages.Add(page)

            Next

            Dim pageMessage As New PaginatedMessage With {
                .Pages = pages,
                .Color = New Color(_utils.RandomEmbedColor),
                .AlternateDescription = "Help Command"
            }

            Await PagedReplyAsync(pages)
        Else
            'See if there's a better way to filter through commands to find the one mentioned in name
            For Each mods In _service.Modules
                For Each command In mods.Commands
                    If name = command.Name Then
                        Dim embed = New EmbedBuilder With {
                            .Title = command.Aliases.FirstOrDefault,
                            .Description = $"{command.Summary}{Environment.NewLine}{Environment.NewLine}{If(command.Remarks, "No parameters needed for this")}",
                            .Color = New Color(_utils.RandomEmbedColor)
                        }

                        ReplyAsync(embed:=embed.Build)
                    End If
                Next

            Next

        End If

    End Function

End Class
