Imports Newtonsoft.Json

'Properties of ActivityEvent
Partial Public Class ActivityEvent
    'Description of the queried activity
    <JsonProperty("activity")>
    Public Property Activity() As String
    'A factor describing how possible an event is to do with zero being the most accessible [0.0, 1.0]
    <JsonProperty("accessibility")>
    Public Property Accessibility() As Double
    'Type of the activity ["education", "recreational", "social", "diy", "charity", "cooking", "relaxation", "music", "busywork"]
    <JsonProperty("type")>
    Public Property Type() As String
    'The number Of people that this activity could involve [0, n]
    <JsonProperty("participants")>
    Public Property Participants() As Long
    'A factor describing the cost of the event with zero being free [0, 1]
    <JsonProperty("price")>
    Public Property Price() As Double
    'Gets or sets the link of the activity
    <JsonProperty("link")>
    Public Property Link() As Uri
    'A unique numeric id [1000000, 9999999]
    <JsonProperty("key")>
    <JsonConverter(GetType(ParseStringConverter))>
    Public Property Key() As Long
End Class

'An Activity
Partial Public Class ActivityEvent
    'Converts Json to ActivityEvent then returns a Converted ActivityEvent
    Public Shared Function FromJson(ByVal json As String) As ActivityEvent
        Return JsonConvert.DeserializeObject(Of ActivityEvent)(json, Converter.Settings)
    End Function
End Class