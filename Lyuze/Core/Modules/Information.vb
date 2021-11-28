Imports Discord.Commands
Imports Discord

<Name("Information")>
<Summary("Get information on certain things.")>
Public Class Information
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("server")>
    <Summary("Gives you information about the server.")>
    Public Async Function serverCmd() As Task
        Dim g = Context.Guild

        Await ReplyAsync(embed:=InformationService.GetServer(g, Context).Result)
    End Function

    <Command("profile")>
    <Summary("Gives info about your discord profile.")>
    <Remarks("/profile <@user> | Can leave the argument blank to bring up your profile.")>
    Public Async Function profileCmd(Optional user As IGuildUser = Nothing) As Task
        user = If(user, Context.User)

        Await ReplyAsync(embed:=InformationService.GetProfile(user, Context).Result)

    End Function

End Class