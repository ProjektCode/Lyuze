Imports Discord.Commands

<Name("Mini Games")>
<Summary("Lets play some games!")>
Public Class cmd_mGames
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("war")>
    <Summary("Battle the bot with the classic card game war. The player with the higher number wins.")>
    Public Async Function warCmd() As Task
        Dim user = Context.User
        Dim rand As New Random
        Dim botNum As Integer = rand.Next(1, 21)
        Dim userNum As Integer = rand.Next(1, 21)

        If userNum > botNum Then
            Await Context.Channel.SendMessageAsync($"{user.Mention} You win. My number was {botNum} and your number was {userNum}")

        Else
            If userNum = botNum Then
                Await Context.Channel.SendMessageAsync($"It would seem we have tied {user.Mention}. Our number was {userNum}")

            Else
                Await Context.Channel.SendMessageAsync($"Looks like you are the loser {user.Mention}. My number was {botNum} and your number was {userNum}")

            End If
        End If

    End Function

    <Command("rnum")>
    <Summary("Get a random number between 1 and a given interger.")>
    <Remarks("\rnum 100")>
    Public Async Function randomNum(num As Integer) As Task
        Dim user = Context.User
        Dim channel = Context.Channel
        Dim randNum As Integer
        Dim rand As New Random

        If num <= 1 Then
            Await channel.SendMessageAsync($"{user.Mention} the number you picked is less than or equal to 1. Please pick a number higher than 1.")
        Else
            randNum = rand.Next(1, num)

            Await channel.SendMessageAsync($"{user.Mention} the random number was {randNum}")
        End If

    End Function

End Class
