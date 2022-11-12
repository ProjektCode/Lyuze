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

        Await ReplyAsync(embed:=Await InformationService.GetServer(g, Context))
    End Function

    <Command("profile")>
    <Summary("Gives info about your discord profile.")>
    <Remarks("/profile <@user> | Can leave the argument blank to bring up your profile.")>
    Public Async Function profileCmd(Optional user As IGuildUser = Nothing) As Task
        user = If(user, Context.User)

        Await ReplyAsync(embed:=Await InformationService.GetProfile(user, Context))

    End Function

End Class

<Group("set")>
<Name("Database Information")>
<Summary("/set <command> <argument> | set stuff for your profile.")>
Public Class SetInformation
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("about")>
    <Summary("Configure your profile's about me.")>
    <Remarks("\set about This is a test. | your about me will be 'This is a test.'")>
    Public Async Function ConfigAboutMe(<Remainder> about As String) As Task
        Await ReplyAsync(embed:=Await PlayerService.UpdateAboutMe(Context.User, about))
    End Function

    <Command("background")>
    <Summary("Configure your profile's background Preferred dimensions are 1299x512.")>
    <Remarks("\set about This is a test. | your about me will be 'This is a test.'")>
    Public Async Function ConfigBackground(url As String) As Task
        Await ReplyAsync(embed:=Await PlayerService.UpdateBackground(Context.User, url))
    End Function

    <Command("favchar")>
    <Summary("Configure your profile's favorite character.")>
    <Remarks("\set favchar Yoimiya. | your favorite character is Yoimiya")>
    Public Async Function ConfigFavChar(<Remainder> [char] As String) As Task
        Await ReplyAsync(embed:=Await PlayerService.UpdateFavCharacter(Context.User, [char]))
    End Function

    <Command("profile")>
    <Summary("Configure your profile's publicity state.")>
    <Remarks("\set profile public. | your profile would be set to public having other users view your profile.")>
    Public Async Function ConfigProfile(state As String) As Task
        Await ReplyAsync(embed:=Await PlayerService.UpdateProfile(Context.User, state))
    End Function

End Class