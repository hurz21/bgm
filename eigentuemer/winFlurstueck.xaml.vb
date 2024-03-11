Public Class winFlurstueck
    Public Property normflst As New clsFlurstueck
    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub winFlurstueck_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        e.Handled = True
        Dim a = 1
        initGemarkungsCombo()
    End Sub
    Sub initGemarkungsCombo()
        Dim existing As XmlDataProvider = TryCast(Me.Resources("XMLSourceComboBoxgemarkungen"), XmlDataProvider)
        existing.Source = New Uri("\\gis\gdvell\apps\bplankat\gemarkungen.xml")
    End Sub

    Private Sub cmbgemarkung_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbgemarkung.SelectionChanged
        If cmbgemarkung.SelectedItem Is Nothing Then Exit Sub

        Dim myvali$ = CStr(cmbgemarkung.SelectedValue)
        Dim myvalx = CType(cmbgemarkung.SelectedItem, System.Xml.XmlElement)
        Dim myvals$ = myvalx.Attributes(1).Value.ToString

        tbGemarkung.Text = myvals
        normflst.gemcode = CInt(myvali)
        normflst.gemarkungstext = tbGemarkung.Text
        initFlureCombo()
        cmbFlur.IsEnabled = True
        cmbFlur.IsDropDownOpen = True
        e.Handled = True
    End Sub
    Sub initFlureCombo()
        'gemeindeDT
        'dt = modgetdt4sql.getDT4Query(Sql, toolsEigentuemer.paradigmaMsql, hinweis)
        DB_Oracle_sharedfunctions.holeFlureDT()
        cmbFlur.DataContext = myGlobalz.sitzung.postgresREC.dt
    End Sub

    Private Sub cmbFlur_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbFlur.SelectionChanged
        Dim item2 As DataRowView = CType(cmbFlur.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub

        cmbZaehler.IsEnabled = True
        Dim item3$ = item2.Row.ItemArray(0).ToString
        tbflur.Text = item2.Row.ItemArray(0).ToString
        'Me.tbStrasse.Text=item4
        normflst.flur = CInt(item3$)
        ' normflst.gemarkungstext = Me.tbGemarkung.Text
        initZaehlerCombo()
        cmbZaehler.IsDropDownOpen = True
        e.Handled = True
    End Sub
    Sub initZaehlerCombo()
        'gemeindeDT
        DB_Oracle_sharedfunctions.holeZaehlerDT()
        cmbZaehler.DataContext = myGlobalz.sitzung.postgresREC.dt
    End Sub

    Private Sub cmbZaehler_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbZaehler.SelectionChanged
        Dim item2 As DataRowView = CType(cmbZaehler.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Dim item3$ = item2.Row.ItemArray(0).ToString
        cmbNenner.IsEnabled = True
        tbZaehler.Text = item2.Row.ItemArray(0).ToString
        'Me.tbStrasse.Text=item4
        normflst.zaehler = CInt(item3$)
        ' normflst.gemarkungstext = Me.tbGemarkung.Text
        normflst.nenner = Nothing
        initNennerCombo()
        If myGlobalz.sitzung.postgresREC.dt.Rows.Count = 1 Then
            tbNenner.Text = myGlobalz.sitzung.postgresREC.dt.Rows(0).Item(0).ToString
            NennerVerarbeiten()
            cmbFunktionsvorschlaege.IsDropDownOpen = True
        Else
            cmbNenner.IsDropDownOpen = True
        End If
        e.Handled = True
    End Sub

    Sub initNennerCombo()
        'gemeindeDT
        DB_Oracle_sharedfunctions.holeNennerDT()
        cmbNenner.DataContext = myGlobalz.sitzung.postgresREC.dt
    End Sub

    Private Sub NennerVerarbeiten()
        'Dim fst_id% = 0
        normflst.nenner = CInt(tbNenner.Text)
        FST_tools.nennerUndFSPruefen()
        tbCoords.Text = String.Format("{0},{1}", myGlobalz.sitzung.aktFST.punkt.X, myGlobalz.sitzung.aktFST.punkt.Y)
        ' tbFreitext.Text = String.Format("{0} qm", normflst.flaecheqm)
        tbarea.Text = CStr(normflst.flaecheqm)
        lblFS.Text = normflst.FS
        btnSpeichernFlurstueck.IsEnabled = True
    End Sub

    Private Sub cmbNenner_SelectionChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs) Handles cmbNenner.SelectionChanged
        Dim item2 As DataRowView = CType(cmbNenner.SelectedItem, DataRowView)
        If item2 Is Nothing Then Exit Sub
        Try
            'Dim item3$ = item2.Row.ItemArray(0).ToString
        Catch ex As Exception
            Exit Sub
        End Try
        tbNenner.Text = item2.Row.ItemArray(0).ToString
        'Me.tbStrasse.Text=item4
        NennerVerarbeiten()
        cmbFunktionsvorschlaege.IsDropDownOpen = True
        e.Handled = True
    End Sub
End Class
