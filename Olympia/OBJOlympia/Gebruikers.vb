
Namespace OBJOlympia

    Public Class Gebruikers
        Private PFX_IdLid As Integer
        Private PFX_Naam As String
        Private PFX_Voornaam As String
        Private PFX_VolledigeNaam As String
        Private PFX_Paswoord As String
        Private PFX_Email As String
        Private PFX_GSM As String
        Private PFX_GebDatum As Date
        Private PFX_Active As Integer
        Private PFX_Geslacht As String
        Private PFX_rekeningnummer As String
        Private PFX_Gemeente As String
        Private PFX_Postcode As String
        Private PFX_straat As String
        Private PFX_huisnr As String
        Private PFX_info As String

        Public Sub New()
           
        End Sub

        Public Property IdLid() As Integer
            Get
                Return PFX_IdLid
            End Get
            Set(ByVal value As Integer)
                PFX_IdLid = value
            End Set
        End Property

        Public Property Naam() As String
            Get
                Return PFX_Naam
            End Get
            Set(ByVal value As String)
                PFX_Naam = value
            End Set
        End Property

        Public Property Voornaam() As String
            Get
                Return PFX_Voornaam
            End Get
            Set(ByVal value As String)
                PFX_Voornaam = value
            End Set
        End Property

        Public Property VolledigeNaam() As String
            Get
                Return PFX_VolledigeNaam
            End Get
            Set(ByVal value As String)
                PFX_VolledigeNaam = value
            End Set
        End Property

        Public Property Paswoord() As String
            Get
                Return PFX_Paswoord
            End Get
            Set(ByVal value As String)
                PFX_Paswoord = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return PFX_Email
            End Get
            Set(ByVal Value As String)
                PFX_Email = Value
            End Set
        End Property

        Public Property GSM() As String
            Get
                Return PFX_GSM
            End Get
            Set(ByVal Value As String)
                PFX_GSM = Value
            End Set
        End Property

        Public Property Geslacht() As String
            Get
                Return PFX_Geslacht
            End Get
            Set(ByVal Value As String)
                PFX_Geslacht = Value
            End Set
        End Property

        Public Property GebDatum() As Date
            Get
                Return PFX_GebDatum
            End Get
            Set(ByVal Value As Date)
                PFX_GebDatum = Value
            End Set
        End Property

        Public Property Active() As Integer
            Get
                Return PFX_Active
            End Get
            Set(ByVal Value As Integer)
                PFX_Active = Value
            End Set
        End Property

        Public Property Rekeningnummer() As String
            Get
                Return PFX_rekeningnummer
            End Get
            Set(ByVal Value As String)
                PFX_rekeningnummer = Value
            End Set
        End Property
        Public Property Info() As String
            Get
                Return PFX_info
            End Get
            Set(ByVal Value As String)
                PFX_info = Value
            End Set
        End Property
        Public Property Gemeente() As String
            Get
                Return PFX_Gemeente
            End Get
            Set(ByVal Value As String)
                PFX_Gemeente = Value
            End Set
        End Property
        Public Property Postcode() As String
            Get
                Return PFX_Postcode
            End Get
            Set(ByVal Value As String)
                PFX_Postcode = Value
            End Set
        End Property
        Public Property Straat() As String
            Get
                Return PFX_straat
            End Get
            Set(ByVal Value As String)
                PFX_straat = Value
            End Set
        End Property
        Public Property Huisnr() As String
            Get
                Return PFX_huisnr
            End Get
            Set(ByVal Value As String)
                PFX_huisnr = Value
            End Set
        End Property
    End Class

    Public Class Logging
        Private PFX_gebruiker As Gebruikers
        Private PFX_Datum As Date
        Private PFX_Event As String
        Private PFX_Type As Integer


        Public Sub New()
            PFX_gebruiker = New Gebruikers
        End Sub

        Public Property Gebruiker() As Gebruikers
            Get
                Return PFX_gebruiker
            End Get
            Set(ByVal value As Gebruikers)
                PFX_gebruiker = value
            End Set
        End Property

        Public Property Datum() As Date
            Get
                Return PFX_Datum
            End Get
            Set(ByVal value As Date)
                PFX_Datum = value
            End Set
        End Property

        Public Property EventLogging() As String
            Get
                Return PFX_Event
            End Get
            Set(ByVal value As String)
                PFX_Event = value
            End Set
        End Property

        Public Property Type() As Integer
            Get
                Return PFX_Type
            End Get
            Set(ByVal value As Integer)
                PFX_Type = value
            End Set
        End Property

    End Class

    Public Class Handelingen
        Private PFX_Id As Integer
        Private PFX_gebruiker As Gebruikers
        Private PFX_Datum As Date
        Private PFX_Discipline As pic_Disciplines
        Private PFX_Groep As pic_Trainingsgroepen
        Private PFX_actie As pic_Acties
        Private PFX_Aantal As String
        Private PFX_Bedrag As String
        Private PFX_info As String
        Private PFX_validate As Integer

        Public Sub New()
            PFX_gebruiker = New Gebruikers
            PFX_Groep = New pic_Trainingsgroepen
            PFX_actie = New pic_Acties
            PFX_Discipline = New pic_Disciplines
        End Sub

        Public Property Id() As Integer
            Get
                Return PFX_Id
            End Get
            Set(ByVal value As Integer)
                PFX_Id = value
            End Set
        End Property

        Public Property Gebruiker() As Gebruikers
            Get
                Return PFX_gebruiker
            End Get
            Set(ByVal value As Gebruikers)
                PFX_gebruiker = value
            End Set
        End Property
        Public Property Datum() As Date
            Get
                Return PFX_Datum
            End Get
            Set(ByVal value As Date)
                PFX_Datum = value
            End Set
        End Property
        Public Property Discipline() As pic_Disciplines
            Get
                Return PFX_Discipline
            End Get
            Set(ByVal value As pic_Disciplines)
                PFX_Discipline = value
            End Set
        End Property
        Public Property Groep() As pic_Trainingsgroepen
            Get
                Return PFX_Groep
            End Get
            Set(ByVal value As pic_Trainingsgroepen)
                PFX_Groep = value
            End Set
        End Property
        Public Property Actie() As pic_Acties
            Get
                Return PFX_actie
            End Get
            Set(ByVal value As pic_Acties)
                PFX_actie = value
            End Set
        End Property
        Public Property Aantal() As String
            Get
                Return PFX_Aantal
            End Get
            Set(ByVal value As String)
                PFX_Aantal = value
            End Set
        End Property
        Public Property Bedrag() As String
            Get
                Return PFX_Bedrag
            End Get
            Set(ByVal value As String)
                PFX_Bedrag = value
            End Set
        End Property
        Public Property Info() As String
            Get
                Return PFX_info
            End Get
            Set(ByVal value As String)
                PFX_info = value
            End Set
        End Property
        Public Property Validate() As Integer
            Get
                Return PFX_validate
            End Get
            Set(ByVal value As Integer)
                PFX_validate = value
            End Set
        End Property

    End Class

    Public Class Rechten
        Private PFX_Id As Integer
        Private PFX_gebruiker As Gebruikers
        Private PFX_actie As pic_Acties
        Private PFX_groep As pic_Trainingsgroepen
        Private PFX_validate As Integer

        Public Sub New()
            PFX_gebruiker = New Gebruikers
            PFX_actie = New pic_Acties
            PFX_groep = New pic_Trainingsgroepen
        End Sub

        Public Property Id() As Integer
            Get
                Return PFX_Id
            End Get
            Set(ByVal value As Integer)
                PFX_Id = value
            End Set
        End Property

        Public Property Gebruiker() As Gebruikers
            Get
                Return PFX_gebruiker
            End Get
            Set(ByVal value As Gebruikers)
                PFX_gebruiker = value
            End Set
        End Property

        Public Property Actie() As pic_Acties
            Get
                Return PFX_actie
            End Get
            Set(ByVal value As pic_Acties)
                PFX_actie = value
            End Set
        End Property

        Public Property Groep() As pic_Trainingsgroepen
            Get
                Return PFX_groep
            End Get
            Set(ByVal value As pic_Trainingsgroepen)
                PFX_groep = value
            End Set
        End Property

        Public Property Validate() As Integer
            Get
                Return PFX_validate
            End Get
            Set(ByVal value As Integer)
                PFX_validate = value
            End Set
        End Property


    End Class

    Public Class Aanwezigheid
        Private PFX_Id As Integer
        Private PFX_gebruiker As Gebruikers
        Private PFX_groep As pic_Trainingsgroepen
        Private PFX_Aanwezig As Integer
        Private PFX_Datum As Date
        Private PFX_Opmerking As String
        Private PFX_functie As Integer

        Public Sub New()
            PFX_gebruiker = New Gebruikers
            PFX_groep = New pic_Trainingsgroepen
        End Sub

        Public Property Id() As Integer
            Get
                Return PFX_Id
            End Get
            Set(ByVal value As Integer)
                PFX_Id = value
            End Set
        End Property

        Public Property Gebruiker() As Gebruikers
            Get
                Return PFX_gebruiker
            End Get
            Set(ByVal value As Gebruikers)
                PFX_gebruiker = value
            End Set
        End Property

        Public Property Groep() As pic_Trainingsgroepen
            Get
                Return PFX_groep
            End Get
            Set(ByVal value As pic_Trainingsgroepen)
                PFX_groep = value
            End Set
        End Property

        Public Property Aanwezig() As Integer
            Get
                Return PFX_Aanwezig
            End Get
            Set(ByVal value As Integer)
                PFX_Aanwezig = value
            End Set
        End Property
        Public Property Datum() As Date
            Get
                Return PFX_Datum
            End Get
            Set(ByVal value As Date)
                PFX_Datum = value
            End Set
        End Property
        Public Property Opmerking() As String
            Get
                Return PFX_Opmerking
            End Get
            Set(ByVal value As String)
                PFX_Opmerking = value
            End Set
        End Property
        Public Property Functie() As Integer
            Get
                Return PFX_functie
            End Get
            Set(ByVal value As Integer)
                PFX_functie = value
            End Set
        End Property
    End Class

    Public Class pic_Disciplines
        Private PFX_Id As Integer
        Private PFX_beschrijving As String
        Private PFX_active As Boolean

        Public Sub New()

        End Sub

        Public Property Id() As Integer
            Get
                Return PFX_Id
            End Get
            Set(ByVal value As Integer)
                PFX_Id = value
            End Set
        End Property

        Public Property beschrijving() As String
            Get
                Return PFX_beschrijving
            End Get
            Set(ByVal value As String)
                PFX_beschrijving = value
            End Set
        End Property

        Public Property active() As Boolean
            Get
                Return PFX_active
            End Get
            Set(ByVal value As Boolean)
                PFX_active = value
            End Set
        End Property
    End Class

    Public Class pic_Trainingsgroepen
        Private PFX_Id As Integer
        Private PFX_beschrijving As String
        Private PFX_Discipline As pic_Disciplines
        Private PFX_active As Boolean

        Public Sub New()
            PFX_Discipline = New pic_Disciplines
        End Sub

        Public Property Id() As Integer
            Get
                Return PFX_Id
            End Get
            Set(ByVal value As Integer)
                PFX_Id = value
            End Set
        End Property

        Public Property beschrijving() As String
            Get
                Return PFX_beschrijving
            End Get
            Set(ByVal value As String)
                PFX_beschrijving = value
            End Set
        End Property
        Public Property Discipline() As pic_Disciplines
            Get
                Return PFX_Discipline
            End Get
            Set(ByVal value As pic_Disciplines)
                PFX_Discipline = value
            End Set
        End Property
        Public Property active() As Boolean
            Get
                Return PFX_active
            End Get
            Set(ByVal value As Boolean)
                PFX_active = value
            End Set
        End Property

    End Class

    Public Class pic_Acties
        Private PFX_Id As Integer
        Private PFX_beschrijving As String
        Private PFX_menu As String

        Public Sub New()

        End Sub

        Public Property Id() As Integer
            Get
                Return PFX_Id
            End Get
            Set(ByVal value As Integer)
                PFX_Id = value
            End Set
        End Property

        Public Property beschrijving() As String
            Get
                Return PFX_beschrijving
            End Get
            Set(ByVal value As String)
                PFX_beschrijving = value
            End Set
        End Property
        Public Property menu() As String
            Get
                Return PFX_menu
            End Get
            Set(ByVal value As String)
                PFX_menu = value
            End Set
        End Property

    End Class

End Namespace