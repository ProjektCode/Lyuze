Imports Discord.Commands

<Name("Mini Games")>
<Summary("Lets play some games!")>
Public Class cmd_mGames
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("war")>
    <Summary("Battle the bot with the classic card game war. The player with the higher number wins.")>
    Public Async Function warCmd() As Task
        Await ReplyAsync(embed:=Await MiniGamesService.WarGame(Context))
    End Function

    <Command("rnum")>
    <Summary("Get a random number between 1 and a given integer.")>
    <Remarks("\rnum 100")>
    Public Async Function randomNum(num As Integer) As Task
        Await ReplyAsync(embed:=Await MiniGamesService.RandomNumber(num, Context))
    End Function

End Class
