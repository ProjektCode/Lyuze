Imports Discord
Imports Discord.Commands
Imports Discord.Addons.Interactive

<Name("General")>
<Summary("Commands that don't deserve their own place.")>
Public Class General
    Inherits InteractiveBase(Of SocketCommandContext)

    <Command("emotes")>
    <Summary("Returns all available guild emotes.")>
    Public Async Function guildEmotesCmd() As Task 'Rework for pagination
        Await ReplyAsync(embed:=Await GeneralService.GuildEmotes(Context))
    End Function

    <Command("report")>
    <Summary("Report messages.")>
    <Remarks("\report @messageid")>
    Public Async Function Report(id As ULong) As Task
        Await ReplyAsync(embed:=Await ModService.Report(id, Context))
    End Function

End Class
