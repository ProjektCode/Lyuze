Imports Discord
Imports Discord.Commands
Imports Microsoft.Extensions.DependencyInjection

NotInheritable Class MiniGamesService

    Private Shared ReadOnly _utils As MasterUtils = serviceHandler.provider.GetRequiredService(Of MasterUtils)

    Public Shared Async Function WarGame(ctx As SocketCommandContext) As Task(Of Embed)
        Dim user = ctx.User
        Dim rand As New Random
        Dim botNum As Integer = rand.Next(1, 21)
        Dim userNum As Integer = rand.Next(1, 21)

        If userNum > botNum Then
            Dim wembed = New EmbedBuilder With {
                .Title = "Winner",
                .Description = $"✅ {user.Mention} You win. My number was {botNum} and your number was {userNum}",
                .Color = Color.Green,
                .ThumbnailUrl = ctx.Guild.CurrentUser.GetAvatarUrl
            }
            Return wembed.Build
        ElseIf userNum = botNum Then
            Dim tembed = New EmbedBuilder With {
                .Title = "Tied",
                .Description = $"It would seem we have tied {user.Mention}. Our number was {userNum}",
                .Color = Color.DarkGrey,
                .ThumbnailUrl = ctx.Guild.CurrentUser.GetAvatarUrl
            }
            Return tembed.Build
        Else
            Dim lembed = New EmbedBuilder With {
                .Title = "Loser",
                .Description = $"Looks like you are the loser {user.Mention}. My number was {botNum} and your number was {userNum}",
                .Color = Color.Red,
                .ThumbnailUrl = ctx.Guild.CurrentUser.GetAvatarUrl
            }
            Return lembed.Build
        End If
    End Function

    Public Shared Async Function RandomNumber(num As Integer, ctx As SocketCommandContext) As Task(Of Embed)
        Dim randNum As Integer
        Dim rand As New Random

        If num <= 1 Then
            Return embedHandler.errorEmbed("Minigame - Random Number", $"{ctx.User.Mention} the number you picked is less than or equal to 1. Please pick a number higher than 1.").Result
        Else
            randNum = rand.Next(1, num)
            Dim embed = New EmbedBuilder With {
                .Title = "Random Number",
                .Description = $"{ctx.User.Mention} the random number was {randNum}.",
                .Color = New Color(_utils.RandomEmbedColor),
                .ThumbnailUrl = ctx.Guild.CurrentUser.GetAvatarUrl
            }
            Return embed.Build
        End If
    End Function

End Class
