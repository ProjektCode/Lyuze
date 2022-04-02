Imports Newtonsoft.Json

Partial Public Class [Operator]
	<JsonProperty("Profile")>
	Public Property Profile() As Profile

	<JsonProperty("Tags")>
	Public Property Tags() As List(Of String)

	<JsonProperty("Traits")>
	Public Property Traits() As Traits

	<JsonProperty("Potentials")>
	Public Property Potentials() As List(Of Potential)

	<JsonProperty("Talents")>
	Public Property Talents() As List(Of Talent)

	<JsonProperty("Skills")>
	Public Property Skills() As List(Of Skill)
End Class

Partial Public Class Potential
	<JsonProperty("Potential 1")>
	Public Property Potential1() As String

	<JsonProperty("Potential 2")>
	Public Property Potential2() As String

	<JsonProperty("Potential 3")>
	Public Property Potential3() As String

	<JsonProperty("Potential 4")>
	Public Property Potential4() As String

	<JsonProperty("Potential 5")>
	Public Property Potential5() As String

	<JsonProperty("Potential 6")>
	Public Property Potential6() As String
End Class

Partial Public Class Profile
	<JsonProperty("Name")>
	Public Property Name() As String

	<JsonProperty("Position")>
	Public Property Position() As String

	<JsonProperty("Attack Type")>
	Public Property AttackType() As String

	<JsonProperty("Role")>
	Public Property Role() As String

	<JsonProperty("Sub Role")>
	Public Property SubRole() As String

	<JsonProperty("Rarity")>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property Rarity() As Long

	<JsonProperty("Alternative Forms")>
	Public Property AlternativeForms() As String

	<JsonProperty("Description")>
	Public Property Description() As String

	<JsonProperty("Quote")>
	Public Property Quote() As String

	<JsonProperty("Bio")>
	Public Property Bio() As String

	<JsonProperty("Artist")>
	Public Property Artist() As String

	<JsonProperty("CV")>
	Public Property Cv() As String

	<JsonProperty("Gender")>
	Public Property Gender() As String

	<JsonProperty("Place of Birth")>
	Public Property PlaceOfBirth() As String

	<JsonProperty("Birthday")>
	Public Property Birthday() As String

	<JsonProperty("Race")>
	Public Property Race() As String

	<JsonProperty("Infection Status")>
	Public Property InfectionStatus() As String

	<JsonProperty("Group")>
	Public Property Group() As String

	<JsonProperty("Release Date")>
	Public Property ReleaseDate() As ReleaseDate
End Class

Partial Public Class ReleaseDate
	<JsonProperty("CN")>
	Public Property Cn() As String

	<JsonProperty("NA")>
	Public Property Na() As String
End Class

Partial Public Class Skill
	<JsonProperty("Skill 1")>
	Public Property Skill1() As Skill1_Class

	<JsonProperty("Skill 2")>
	Public Property Skill2() As Skill2

	<JsonProperty("Skill 3")>
	Public Property Skill3() As Skill1_Class
End Class

Partial Public Class Skill1_Class
	<JsonProperty("Name")>
	Public Property Name() As String

	<JsonProperty("SP Cost")>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property SpCost() As Long

	<JsonProperty("Initial SP")>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property InitialSp() As Long

	<JsonProperty("SP Charge Type")>
	Public Property SpChargeType() As String

	<JsonProperty("Skill Activation")>
	Public Property SkillActivation() As String

	<JsonProperty("Duration")>
	Public Property Duration() As String

	<JsonProperty("Description")>
	Public Property Description() As List(Of Skill1_Description)
End Class

Partial Public Class Skill1_Description
	<JsonProperty("Level: 1")>
	Public Property Level1() As String

	<JsonProperty("Level: 2")>
	Public Property Level2() As String

	<JsonProperty("Level: 3")>
	Public Property Level3() As String

	<JsonProperty("Level: 4")>
	Public Property Level4() As String

	<JsonProperty("Level: 5")>
	Public Property Level5() As String

	<JsonProperty("Level: 6")>
	Public Property Level6() As String

	<JsonProperty("Level: 7")>
	Public Property Level7() As List(Of Level7)
End Class

Partial Public Class Level7
	<JsonProperty("Mastery 0")>
	Public Property Mastery0() As String

	<JsonProperty("Mastery 1")>
	Public Property Mastery1() As String

	<JsonProperty("Mastery 2")>
	Public Property Mastery2() As String

	<JsonProperty("Mastery 3")>
	Public Property Mastery3() As String
End Class

Partial Public Class Skill2
	<JsonProperty("Name")>
	Public Property Name() As String

	<JsonProperty("SP Cost")>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property SpCost() As Long

	<JsonProperty("Initial SP")>
	<JsonConverter(GetType(ParseStringConverter))>
	Public Property InitialSp() As Long

	<JsonProperty("SP Charge Type")>
	Public Property SpChargeType() As String

	<JsonProperty("Skill Activation")>
	Public Property SkillActivation() As String

	<JsonProperty("Duration")>
	Public Property Duration() As String

	<JsonProperty("Description")>
	Public Property Description() As List(Of Skill2_Description)
End Class

Partial Public Class Skill2_Description
	<JsonProperty("Level: 1")>
	Public Property Level1() As String

	<JsonProperty("Level: 2")>
	Public Property Level2() As String

	<JsonProperty("Level: 3")>
	Public Property Level3() As String

	<JsonProperty("Level: 4")>
	Public Property Level4() As String

	<JsonProperty("Level: 5")>
	Public Property Level5() As String

	<JsonProperty("Level: 6")>
	Public Property Level6() As String

	<JsonProperty("Level: 7")>
	Public Property Level7() As Level7
End Class

Partial Public Class Talent
	<JsonProperty("Elite 0")>
	Public Property Elite0() As List(Of Elite)

	<JsonProperty("Elite 1")>
	Public Property Elite1() As List(Of Elite)

	<JsonProperty("Elite 2")>
	Public Property Elite2() As List(Of Elite)
End Class

Partial Public Class Elite
	<JsonProperty("Ambush")>
	Public Property Ambush() As String

	<JsonProperty("Insult to Injury")>
	Public Property InsultToInjury() As String
End Class

Partial Public Class Traits
	<JsonProperty("Description")>
	Public Property Description() As String

	<JsonProperty("Effect")>
	Public Property Effect() As Effect
End Class

Partial Public Class Effect
	<JsonProperty("Name")>
	Public Property Name() As String

	<JsonProperty("Description")>
	Public Property Description() As String
End Class


Partial Public Class [Operator]
	Public Shared Function FromJson(ByVal json As String) As [Operator]
		Return JsonConvert.DeserializeObject(Of [Operator])(json, Converter.Settings)
	End Function
End Class