
Namespace OBJOlympia

    Public Enum Vergoedingen
        Aanwezigheden = 1
        Onkosten = 10
        Jury = 11
        Wedstrijd = 12
        Verplaatsingen = 13
        Andere = 14
    End Enum

    Public Enum Disciplines
        Kleuters = 1
        TTM = 2
        Acro = 3
        Tumbling = 4
        Trampoline = 5
        Dans = 6
        Volwassenen = 7
    End Enum

    Public Enum Functies
        Gymnast = 1
        Trainer = 2
        Vrijwilliger = 3
        Bestuur = 4
    End Enum

    Public Enum UseHeader
        ''' <summary>
        ''' Indicates that the first row contains columnnames, no data
        ''' </summary>
        ''' <remarks></remarks>
        Yes
        ''' <summary>
        ''' Indicates that the first row does not contain columnnames
        ''' </summary>
        ''' <remarks></remarks>
        No
    End Enum
    Public Enum ExcelImex
        TryScan = 0
        Resolve = 1
    End Enum

    Public Enum TypeImportExt
        Trainingsgroepen = 1
        Gebruikers = 2
        Wedstrijden = 3
    End Enum

    Public Enum FeitenCatSourceType As Integer
        '// FeitenCatSourceType
        CrimVerslag = 0
        Vatting = 1
    End Enum

    Public Enum enmSearchPart As Integer
        '// enmSearchPart
        None = 0
        KomtVoorIn = 1
        BegintMet = 2
        EindigtMet = 3
        multizoek = 4
    End Enum

    Public Enum KISSScheduledStatus
        '// KISSScheduledStatus
        ToBeExecuted = 0
        Busy = 1
        Finished = 2
        Cancelled = 3
        ErrorOcc = 4
    End Enum

    Public Enum Rechten_Lid
        schrijven = 1
        lezen = 2
        geen_toegang = 3

    End Enum
    Public Enum TypeLogging
        login = 1
        beheer = 2
        vergoedingen = 3
        gebruikers = 4
        import = 5
    End Enum

    Public Enum KissLockType
        '// KissLockType
        None = 0
        Entiteiten = 1
    End Enum


    Public Enum LogType
        '// LogType
        Loggin = 1
        Registratie = 2
        Beheer = 3
        AutoProcedures = 4
        Import = 5

    End Enum

    
    Public Enum BoodschapStatus
        '// BoodschapStatus
        None = 0
        Gelezen = 10
        Ongelezen = 20
    End Enum

    

    Public Enum UploadModules
        '// UploadModules
        None = 0
        VattingDocumenten = 1
        OmaCgsu = 2
        Entiteiten = 3
        Vragenregister = 4
        Cti = 5
        Ks = 6
        Cid = 7
        Fin = 8
        Ibn = 9
        Bts = 10
        Ia = 11
        Chronologie = 13
        LogBoek = 14
    End Enum

    Public Enum UploadDocTypes
        ' // UploadDocTypes
        None = 0
        Afbeeldingen = 1
        Pdf = 2
        Word = 3
        Xls = 4
        Rtf = 5
        Exp = 6
        Txt = 9
        ZevenZ = 10
        Tif = 11
        Zip = 12
    End Enum

    Public Enum TaalCode
        '// TaalCode
        None = 0
        Nederlands = 1
        Frans = 2
        Duits = 3
        Engels = 4
    End Enum

   

End Namespace