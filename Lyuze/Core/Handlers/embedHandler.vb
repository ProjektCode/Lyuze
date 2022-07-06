Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket

NotInheritable Class embedHandler

    Public Shared Async Function basicEmbed(src As String, msg As String, clr As String) As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"{src}") _
            .WithDescription($"{msg}") _
            .WithColor(New Color(clr)) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function errorEmbed(_source As String, _error As String) As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"ERROR | {_source}") _
            .WithDescription($"{_error}") _
            .WithColor(Color.Red) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function ColdWarZombiesClass(user As SocketUser, weapon As String, optic As String, muzzle As String, barrel As String, body As String, under As String, magazine As String, handle As String, stock As String) As Task(Of Embed)
        Dim embed As New EmbedBuilder With {
            .Title = $"{user.Username}'s Class",
            .ThumbnailUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
            .Description = $"{user.Mention}'s Randomly generated class. Have fun!",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .IconUrl = If(user.GetAvatarUrl, user.GetDefaultAvatarUrl),
                .Text = "Random Cold War Zombies Class"
            }
        }
        embed.AddField("Weapon", weapon, True)
        embed.AddField("Optic", optic, True)
        embed.AddField("Muzzle", muzzle, True)
        embed.AddField("Barrel", barrel, True)
        embed.AddField("Body", body, True)
        embed.AddField("UnderBarrel", under, True)
        embed.AddField("Magazine", magazine, True)
        embed.AddField("Handle", handle, True)
        embed.AddField("Stock", stock, True)


        Return embed.Build
    End Function

End Class
