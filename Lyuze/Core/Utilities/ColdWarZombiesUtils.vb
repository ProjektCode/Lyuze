Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket

NotInheritable Class ColdWarZombiesUtils
    Private Shared ReadOnly basePath As String = AppDomain.CurrentDomain.BaseDirectory
    Private Shared ReadOnly zombies As String = $"{basePath}Resources\Data\COD\CW\ZM"
    Private Shared ReadOnly rand As New Random

    Private Shared wea
    Private Shared opt
    Private Shared muz
    Private Shared bar
    Private Shared bod
    Private Shared und
    Private Shared mag
    Private Shared han
    Private Shared sto

    Public Shared Async Function AssualtRifle(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\assualt-rifles.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim ar = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(ar.Weapon.Count)
        opt = rand.Next(ar.Optic.Count)
        muz = rand.Next(ar.Muzzle.Count)
        bar = rand.Next(ar.Barrel.Count)
        bod = rand.Next(ar.Body.Count)
        und = rand.Next(ar.Underbarrel.Count)
        mag = rand.Next(ar.Magazine.Count)
        han = rand.Next(ar.Handle.Count)
        sto = rand.Next(ar.Stock.Count)

        Dim weapon = $"{ar.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{ar.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{ar.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{ar.Barrel(bar)} - {bar + 1}"
        Dim body = $"{ar.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{ar.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{ar.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{ar.Handle(han)} - {han + 1}"
        Dim stock = $"{ar.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function SubMachineGun(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\smgs.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim smg = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(smg.Weapon.Count)
        opt = rand.Next(smg.Optic.Count)
        muz = rand.Next(smg.Muzzle.Count)
        bar = rand.Next(smg.Barrel.Count)
        bod = rand.Next(smg.Body.Count)
        und = rand.Next(smg.Underbarrel.Count)
        mag = rand.Next(smg.Magazine.Count)
        han = rand.Next(smg.Handle.Count)
        sto = rand.Next(smg.Stock.Count)

        Dim weapon = $"{smg.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{smg.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{smg.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{smg.Barrel(bar)} - {bar + 1}"
        Dim body = $"{smg.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{smg.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{smg.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{smg.Handle(han)} - {han + 1}"
        Dim stock = $"{smg.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function TacticalRifle(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\tactical-rifles.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim tr = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(tr.Weapon.Count)
        opt = rand.Next(tr.Optic.Count)
        muz = rand.Next(tr.Muzzle.Count)
        bar = rand.Next(tr.Barrel.Count)
        bod = rand.Next(tr.Body.Count)
        und = rand.Next(tr.Underbarrel.Count)
        mag = rand.Next(tr.Magazine.Count)
        han = rand.Next(tr.Handle.Count)
        sto = rand.Next(tr.Stock.Count)

        Dim weapon = $"{tr.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{tr.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{tr.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{tr.Barrel(bar)} - {bar + 1}"
        Dim body = $"{tr.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{tr.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{tr.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{tr.Handle(han)} - {han + 1}"
        Dim stock = $"{tr.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function LightMachineGun(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\lmgs.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim lmg = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(lmg.Weapon.Count)
        opt = rand.Next(lmg.Optic.Count)
        muz = rand.Next(lmg.Muzzle.Count)
        bar = rand.Next(lmg.Barrel.Count)
        bod = rand.Next(lmg.Body.Count)
        und = rand.Next(lmg.Underbarrel.Count)
        mag = rand.Next(lmg.Magazine.Count)
        han = rand.Next(lmg.Handle.Count)
        sto = rand.Next(lmg.Stock.Count)

        Dim weapon = $"{lmg.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{lmg.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{lmg.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{lmg.Barrel(bar)} - {bar + 1}"
        Dim body = $"{lmg.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{lmg.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{lmg.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{lmg.Handle(han)} - {han + 1}"
        Dim stock = $"{lmg.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function SniperRifle(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\snipers.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim sp = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(sp.Weapon.Count)
        opt = rand.Next(sp.Optic.Count)
        muz = rand.Next(sp.Muzzle.Count)
        bar = rand.Next(sp.Barrel.Count)
        bod = rand.Next(sp.Body.Count)
        und = rand.Next(sp.Underbarrel.Count)
        mag = rand.Next(sp.Magazine.Count)
        han = rand.Next(sp.Handle.Count)
        sto = rand.Next(sp.Stock.Count)

        Dim weapon = $"{sp.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{sp.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{sp.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{sp.Barrel(bar)} - {bar + 1}"
        Dim body = $"{sp.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{sp.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{sp.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{sp.Handle(han)} - {han + 1}"
        Dim stock = $"{sp.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function Pistol(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\pistols.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim ps = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(ps.Weapon.Count)
        opt = rand.Next(ps.Optic.Count)
        muz = rand.Next(ps.Muzzle.Count)
        bar = rand.Next(ps.Barrel.Count)
        bod = rand.Next(ps.Body.Count)
        und = rand.Next(ps.Underbarrel.Count)
        mag = rand.Next(ps.Magazine.Count)
        han = rand.Next(ps.Handle.Count)
        sto = rand.Next(ps.Stock.Count)

        Dim weapon = $"{ps.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{ps.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{ps.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{ps.Barrel(bar)} - {bar + 1}"
        Dim body = $"{ps.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{ps.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{ps.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{ps.Handle(han)} - {han + 1}"
        Dim stock = $"{ps.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function Shotgun(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\shotguns.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim sg = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(sg.Weapon.Count)
        opt = rand.Next(sg.Optic.Count)
        muz = rand.Next(sg.Muzzle.Count)
        bar = rand.Next(sg.Barrel.Count)
        bod = rand.Next(sg.Body.Count)
        und = rand.Next(sg.Underbarrel.Count)
        mag = rand.Next(sg.Magazine.Count)
        han = rand.Next(sg.Handle.Count)
        sto = rand.Next(sg.Stock.Count)

        Dim weapon = $"{sg.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{sg.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{sg.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{sg.Barrel(bar)} - {bar + 1}"
        Dim body = $"{sg.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{sg.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{sg.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{sg.Handle(han)} - {han + 1}"
        Dim stock = $"{sg.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

    Public Shared Async Function Special(user As SocketUser) As Task(Of Embed)
        Dim json As String = String.Empty
        Using r As New StreamReader($"{zombies}\assualt-rifles.json")
            json = Await r.ReadToEndAsync
        End Using

        Dim sp = ColdWar_Zombies_RandomClass.FromJson(json)

        wea = rand.Next(sp.Weapon.Count)
        opt = rand.Next(sp.Optic.Count)
        muz = rand.Next(sp.Muzzle.Count)
        bar = rand.Next(sp.Barrel.Count)
        bod = rand.Next(sp.Body.Count)
        und = rand.Next(sp.Underbarrel.Count)
        mag = rand.Next(sp.Magazine.Count)
        han = rand.Next(sp.Handle.Count)
        sto = rand.Next(sp.Stock.Count)

        Dim weapon = $"{sp.Weapon(wea)} - {wea + 1}"
        Dim optic = $"{sp.Optic(opt)} - {opt + 1}"
        Dim muzzle = $"{sp.Muzzle(muz)} - {muz + 1}"
        Dim barrel = $"{sp.Barrel(bar)} - {bar + 1}"
        Dim body = $"{sp.Body(bod)} - {bod + 1}"
        Dim underbarrel = $"{sp.Underbarrel(und)} - {und + 1}"
        Dim magazine = $"{sp.Magazine(mag)} - {mag + 1}"
        Dim handle = $"{sp.Handle(han)} - {han + 1}"
        Dim stock = $"{sp.Stock(sto)} - {sto + 1}"

        Return embedHandler.ColdWarZombiesClass(user, weapon, optic, muzzle, barrel, body, underbarrel, magazine, handle, stock).Result
    End Function

End Class
