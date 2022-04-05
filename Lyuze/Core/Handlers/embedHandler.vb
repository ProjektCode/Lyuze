Imports Discord

NotInheritable Class embedHandler

    Public Shared Async Function basicEmbed(src As String, msg As String, clr As String) As Task(Of Embed)
        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"{src}") _
            .WithDescription($"{msg}") _
            .WithColor(New Color(clr)) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

    Public Shared Async Function errorEmbed(_source As String, _error As String) As Task(Of Embed)

        Dim embed = Await Task.Run(Function() New EmbedBuilder() _
            .WithTitle($"ERROR | {_source}") _
            .WithDescription($"{_error}") _
            .WithColor(Color.Red) _
            .WithCurrentTimestamp() _
            .Build())

        Return embed
    End Function

End Class
