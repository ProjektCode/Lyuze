Imports Discord
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

        Dim sum = num1 + num2

        Dim embed = New EmbedBuilder With {
            .Title = $"Problem {num1} + {num2}",
            .Description = $"Total: {sum}",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .Text = "Addition"
            }
        }

        Await ReplyAsync(embed:=embed.Build)

    End Function

    <Command("subtract")>
    <[Alias]("sub")>
    <Summary(" sub | Subtract two numbers.")>
    <Remarks("\sub 1 4")>
    Public Async Function cmdSub(num1 As Integer, num2 As Integer) As Task

        Dim sum = num1 - num2
        Dim user = Context.User


        Dim embed = New EmbedBuilder With {
            .Title = $"Problem {num1} - {num2}",
            .Description = $"Total: {sum}",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .Text = "Subtraction"
            }
        }

        Await ReplyAsync(embed:=embed.Build)

    End Function

    <Command("multiply")>
    <[Alias]("multi")>
    <Summary("multi | Multiply two numbers together.")>
    <Remarks("\multi 1 4")>
    Public Async Function cmdMulti(num1 As Integer, num2 As Integer) As Task

        If num1 = 0 Or num2 = 0 Then
            Await ReplyAsync("Anything multiplied by zero is zero.")
            Return
        End If
        Dim sum = num1 * num2

        Dim embed = New EmbedBuilder With {
            .Title = $"Problem {num1} * {num2}",
            .Description = $"Total: {sum}",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .Text = "Multiplication"
            }
        }

        Await ReplyAsync(embed:=embed.Build)

    End Function

    <Command("divide")>
    <[Alias]("div")>
    <Summary("dib | Divide two numbers together.")>
    <Remarks("\div 4 1")>
    Public Async Function cmdDivide(num1 As Integer, num2 As Integer) As Task

        Dim sum = num1 / num2
        Dim user = Context.User

        If num1 = 0 Or num2 = 0 Then
            Await ReplyAsync("Cannot divide anything by zero.")
            Return
        End If

        Dim embed = New EmbedBuilder With {
                .Title = $"Problem {num1} / {num2}",
                .Description = $"Total: {sum}",
                .Color = Color.Purple,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Division"
                }
        }

        Await ReplyAsync(embed:=embed.Build)

    End Function

End Class
