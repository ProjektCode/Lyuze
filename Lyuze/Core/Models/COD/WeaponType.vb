Imports Newtonsoft.Json

Public Class WeaponType
    Public Shared Function FromJson(ByVal json As String) As List(Of String)
        Return JsonConvert.DeserializeObject(Of List(Of String))(json, Converter.Settings)
    End Function
End Class

Public Module Serialize
    <System.Runtime.CompilerServices.Extension>
    Public Function ToJson(ByVal self As List(Of String)) As String
        Return JsonConvert.SerializeObject(self, Converter.Settings)
    End Function
End Module