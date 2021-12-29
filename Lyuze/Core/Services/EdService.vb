Imports Discord

NotInheritable Class EdService

    Public Shared Async Function Addition(num1 As Integer, num2 As Integer) As Task(Of Embed)
        Dim sum = num1 + num2

        Dim embed = New EmbedBuilder With {
            .Title = $"Problem {num1} + {num2}",
            .Description = $"Total: {sum}",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .Text = "Addition"
            }
        }

        Return embed.Build
    End Function

    Public Shared Async Function Subtraction(num1 As Integer, num2 As Integer) As Task(Of Embed)
        Dim sum = num1 - num2


        Dim embed = New EmbedBuilder With {
            .Title = $"Problem {num1} - {num2}",
            .Description = $"Total: {sum}",
            .Color = Color.Purple,
            .Footer = New EmbedFooterBuilder With {
                .Text = "Subtraction"
            }
        }

        Return embed.Build
    End Function

    Public Shared Async Function Multiplication(num1 As Integer, num2 As Integer) As Task(Of Embed)
        If num1 = 0 Or num2 = 0 Then
            Return embedHandler.errorEmbed("Ed - Multiply", "Anything multiplied by zero is zero.").Result
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

        Return embed.Build
    End Function

    Public Shared Async Function Division(num1 As Integer, num2 As Integer) As Task(Of Embed)
        If num1 = 0 Or num2 = 0 Then
            Return embedHandler.errorEmbed("Ed - Divide", "Cannot divide anything by zero.").Result
        End If

        Dim sum = num1 / num2

        Dim embed = New EmbedBuilder With {
                .Title = $"Problem {num1} / {num2}",
                .Description = $"Total: {sum}",
                .Color = Color.Purple,
                .Footer = New EmbedFooterBuilder With {
                    .Text = "Division"
                }
        }

        Return embed.Build
    End Function

End Class
