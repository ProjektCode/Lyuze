Imports Discord.Commands

<Name("Call of Duty")>
<Summary("Anything COD Related")>
Public Class CallOfDuty
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("coldwarzombies")>
    <[Alias]("cwzm")>
    <Summary("Get a randomly generated class for zombies.")>
    <Remarks("\cwzm")>
    Public Async Function CWZombies() As Task
        Await ReplyAsync(embed:=Await ColdWarZombiesService.RandomClass(Context.User))
    End Function

End Class