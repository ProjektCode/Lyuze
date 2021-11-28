Imports Discord
Imports Discord.Commands

NotInheritable Class ModService

    Public Shared Async Function Report(id As ULong, ctx As SocketCommandContext) As Task(Of Embed)
        Try
            Dim message = ctx.Channel.GetMessageAsync(id)
            Dim embed = New EmbedBuilder With {
                .Title = ":pencil: Report",
                .Description = $"Author: ***{message.Result.Author}***{Environment.NewLine}Message:**{message.Result.Content}**{Environment.NewLine}------------------------------
                    {Environment.NewLine}By:{ctx.Message.Author.Mention}",
                .Color = Color.Red,
                .ThumbnailUrl = If(message.Result.Author.GetAvatarUrl, message.Result.Author.GetDefaultAvatarUrl),
                .Footer = New EmbedFooterBuilder With {
                    .IconUrl = ctx.Client.CurrentUser.GetAvatarUrl,
                    .Text = "Submitted Report"
                }
            }

            ctx.Message.DeleteAsync()
            Return embed.Build
        Catch ex As Exception
            Return embedHandler.errorEmbed("Gen - Report", ex.Message).Result
        End Try
    End Function

End Class
