Imports System.Net.Http
Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive
Imports Microsoft.Extensions.DependencyInjection

<Name("General")>
<Summary("Commands that don't deserve their own place.")>
Public Class General
    Inherits InteractiveBase(Of SocketCommandContext)

    'Grabbing required services
    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)
    Private Shared ReadOnly _httpClientFactory As IHttpClientFactory = serviceHandler.provider.GetRequiredService(Of IHttpClientFactory)
    Private Shared ReadOnly _img As Images = serviceHandler.provider.GetRequiredService(Of Images)

    <Command("emotes")>
    <Summary("Returns all available guild emotes.")>
    Public Async Function guildEmotesCmd() As Task 'Rework for pagination
        Await ReplyAsync(embed:=Await GeneralService.GuildEmotes(Context))
    End Function

    <Command("report")>
    <Summary("Report messages.")>
    <Remarks("\report @messageid | you get the id by right-clicking the message and then you'll see an option to copy the message id.")>
    Public Async Function Report(id As ULong) As Task
        Await ReplyAsync(embed:=Await ModService.Report(id, Context))
    End Function

    <Command("test")>
    Public Async Function test() As Task
        Try

            Dim httpClient = _httpClientFactory.CreateClient
            Dim response = Await httpClient.GetStringAsync("http://localhost:5000/characters/w")
            Dim c = [Operator].FromJson(response)
            Dim potential As String
            Dim skill As String

            If c Is Nothing Then
                ReplyAsync("Something went wrong.")
                Return
            End If

            Dim embed = New EmbedBuilder With {
                .Title = c.Profile.Name,
                .Description = c.Profile.Description,
                .ImageUrl = "https://api.arknights.dev/characters/w/splash_e2",
                .Color = New Color(_utils.RandomEmbedColor)
            }
            embed.AddField("Position", c.Profile.Position, True)
            embed.AddField("Role", c.Profile.AttackType, True)
            embed.AddField("Role", c.Profile.Role, True)
            embed.AddField("Role", c.Profile.SubRole, True)
            embed.AddField("Role", c.Profile.Rarity, True)
            embed.AddField("Role", c.Profile.AlternativeForms, True)
            embed.AddField("Role", c.Profile.Quote, True)
            embed.AddField("Role", c.Profile.Artist, True)
            embed.AddField("Role", c.Profile.Cv, True)
            embed.AddField("Role", c.Profile.Gender, True)
            embed.AddField("Role", c.Profile.PlaceOfBirth, True)
            embed.AddField("Role", c.Profile.Birthday, True)
            embed.AddField("Role", c.Profile.Race, True)
            embed.AddField("Role", c.Profile.InfectionStatus, True)
            embed.AddField("Role", c.Profile.Group, True)
            embed.AddField("Role", $"CN: {c.Profile.ReleaseDate.Cn}{Environment.NewLine}NA: {c.Profile.ReleaseDate.Na}", True)
            embed.AddField("Potentials", potential)
            embed.AddField("Skills", skill)

            ReplyAsync(embed:=embed.Build)

        Catch ex As Exception

        End Try
    End Function

End Class
