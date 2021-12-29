Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class GeneralService

    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function GuildEmotes(ctx As SocketCommandContext) As Task(Of Embed)
        Dim m = ctx.Message
        Dim u = ctx.User
        Dim g = ctx.Guild
        Dim c = ctx.Client

        Try

            Dim embed As New EmbedBuilder With {
                .Title = $"All guild emotes for usage do :EmoteName:",
                .ImageUrl = "https://i.imgur.com/vc241Ku.jpeg",
                .Description = "The full list of our custom guild emotes",
                .Color = New Color(_Utils.randomEmbedColor),
                .ThumbnailUrl = g.IconUrl,
                .Timestamp = ctx.Message.Timestamp,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Emote Data",
                    .IconUrl = g.IconUrl
                }
            }


            Dim emotes As String = ""
            Dim row As Integer = 0
            For Each emote As Emote In DirectCast(ctx.Message.Channel, IGuildChannel).Guild.Emotes
                If emotes.Length + emote.Name.Length + 5 > 256 Then
                    row += 1
                    embed.AddField($"List #{row}", emotes, True)

                    emotes = String.Empty
                End If

                emotes = emotes + "<:" + emote.Name + $":{emote.Id}>" + $" ***{emote.Name}***" + Environment.NewLine
            Next
            embed.AddField($"List #{row + 1}", emotes, True)

            Return embed.Build

        Catch ex As Exception
            Return embedHandler.errorEmbed("General - Emotes", ex.Message).Result
        End Try
    End Function

End Class
