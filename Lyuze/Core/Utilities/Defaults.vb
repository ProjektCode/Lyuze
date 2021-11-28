Imports System.Text
Imports JikanDotNet

NotInheritable Class Defaults
    Public Shared Function defaultValue(str As String)
        If str Is Nothing Then
            Return "N/A"
        End If
        Return str
    End Function
End Class
