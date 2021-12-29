Imports Discord.Commands

<Name("Genshin Impact")>
<Group("genshin")>
<Summary("Get information from anything Genshin Impact related.")>
Public Class Genshin
    Inherits ModuleBase(Of SocketCommandContext)

    <Command("types")>
    <Summary("Get the available types for the genshin commands")>
    Public Async Function GenshinTypes() As Task
        Await ReplyAsync(embed:=Await GenshinService.GenshinTypes)
    End Function

    <Command("character")>
    <Summary("Get information on a specific character.")>
    <[Alias]("char")>
    <Remarks("\genshin char albedo | Returns information on albedo. if empty will return a list of all available characters. Anything involving the traveler will use the anemo information.")>
    Public Async Function GenshinCharacter(<Remainder> Optional name As String = "default") As Task
        Await ReplyAsync(embed:=Await GenshinService.GetCharacter(name))
    End Function

    <Command("weapon")>
    <Summary("Get information on a specific weapon.")>
    <[Alias]("weap")>
    <Remarks("\genshin weap wolf's gravestone | Returns information on wolf's gravestone | if weapons have 's that's required to get information or you do wolf-s-gravestone.")>
    Public Async Function GenshinWeapon(<Remainder> Optional name As String = "default") As Task
        Await ReplyAsync(embed:=Await GenshinService.GetWeapon(name))
    End Function

    <Command("artifact")>
    <Summary("Get information on a specific artifact.")>
    <[Alias]("art")>
    <Remarks("\genshin art wanderer's troupe | Returns information on wanderer's troupe | if artifacts have **'s** that's required to get information or you do wanderer-s-troupe.")>
    Public Async Function GenshinArtifact(<Remainder> Optional name As String = "default") As Task
        Await ReplyAsync(embed:=Await GenshinService.GetArtifact(name))
    End Function

    <Command("enemy")>
    <[Alias]("ene")>
    <Summary("Gives information for enemies")>
    <Remarks("/genshin ene <slime> | can leave empty to get a list of available enemies.")>
    Public Async Function GenshinEnemy(<Remainder> Optional name As String = "default") As Task
        Await ReplyAsync(embed:=Await GenshinService.GetEnemy(name))
    End Function

    <Command("elements")>
    <[Alias]("ele")>
    <Summary("Gives information for enemies")>
    <Remarks("/genshin ene <slime> | can leave empty to get a list of available enemies.")>
    Public Async Function GenshinElements(<Remainder> Optional name As String = "default") As Task
        Await ReplyAsync(embed:=Await GenshinService.GetElement(name))
    End Function

End Class
