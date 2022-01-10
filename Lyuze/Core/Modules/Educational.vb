Imports Discord.Commands

<Name("Educational")>
<Summary("For quick math equations.")>
Public Class Educational
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("addition")>
    <[Alias]("add")>
    <Summary("add | Add two numbers together.")>
    <Remarks("\add 1 4")>
    Public Async Function cmdAdd(num1 As Integer, num2 As Integer) As Task
        Await ReplyAsync(embed:=Await EdService.Addition(num1, num2))
    End Function

    <Command("subtract")>
    <[Alias]("sub")>
    <Summary(" sub | Subtract two numbers.")>
    <Remarks("\sub 1 4")>
    Public Async Function cmdSub(num1 As Integer, num2 As Integer) As Task
        Await ReplyAsync(embed:=Await EdService.Subtraction(num1, num2))
    End Function

    <Command("multiply")>
    <[Alias]("multi")>
    <Summary("multi | Multiply two numbers together.")>
    <Remarks("\multi 1 4")>
    Public Async Function cmdMulti(num1 As Integer, num2 As Integer) As Task
        Await ReplyAsync(embed:=Await EdService.Multiplication(num1, num2))
    End Function

    <Command("divide")>
    <[Alias]("div")>
    <Summary("div | Divide two numbers together.")>
    <Remarks("\div 4 1")>
    Public Async Function cmdDivide(num1 As Integer, num2 As Integer) As Task
        Await ReplyAsync(embed:=Await EdService.Division(num1, num2))
    End Function

End Class
