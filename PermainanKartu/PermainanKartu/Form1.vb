Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading

Public Class Form1

    Dim cardList As New List(Of Image)
    Dim selfCardList As New List(Of PictureBox)
    Dim enemyCardList As New List(Of PictureBox)
    Dim cardListToSend As New List(Of Integer)
    Dim cardSelected As Boolean = False
    Dim firstSelectedCard As Image
    Dim secondSelectedCard As Image
    Dim receivedString As String
    Dim firstPlay As Boolean = True
    Dim isPlaying As Boolean = False
    Dim r As New Random
    Dim tmpName As String
    Dim tmpCard2 As String
    Dim card1 As Image = PermainanKartu.My.Resources._006
    Dim card2 As Image = PermainanKartu.My.Resources._015
    Dim card3 As Image = PermainanKartu.My.Resources._017
    Dim card4 As Image = PermainanKartu.My.Resources._026
    Dim card5 As Image = PermainanKartu.My.Resources._065
    Dim card6 As Image = PermainanKartu.My.Resources._068
    Dim card7 As Image = PermainanKartu.My.Resources._075
    Dim card8 As Image = PermainanKartu.My.Resources._098
    Dim card9 As Image = PermainanKartu.My.Resources._104
    Dim card10 As Image = PermainanKartu.My.Resources._113
    Dim card11 As Image = PermainanKartu.My.Resources._128
    Dim card12 As Image = PermainanKartu.My.Resources._147
    Dim cardPair1 As Image = PermainanKartu.My.Resources._164
    Dim cardPair2 As Image = PermainanKartu.My.Resources._164
    Dim thdUDPServer = New Thread(New ThreadStart(AddressOf serverThread))


    Event checkWin()

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Panel1.Enabled = False
        lblPort.Text = r.Next(1500, 2000)
        thdUDPServer.Start()
        card1.Tag = "a"
        card2.Tag = "b"
        card3.Tag = "c"
        card4.Tag = "d"
        card5.Tag = "e"
        card6.Tag = "f"
        card7.Tag = "g"
        card8.Tag = "h"
        card9.Tag = "i"
        card10.Tag = "j"
        card11.Tag = "k"
        card12.Tag = "l"
        cardPair1.Tag = "m"
        cardPair2.Tag = "n"
        cardList.Add(card1)
        cardList.Add(card2)
        cardList.Add(card3)
        cardList.Add(card4)
        cardList.Add(card5)
        cardList.Add(card6)
        cardList.Add(card7)
        cardList.Add(card8)
        cardList.Add(card9)
        cardList.Add(card10)
        cardList.Add(card11)
        cardList.Add(card12)
        cardList.Add(cardPair1)
        cardList.Add(cardPair2)
        selfCardList.Add(selfCard1)
        selfCardList.Add(selfCard2)
        selfCardList.Add(selfCard3)
        selfCardList.Add(selfCard4)
        selfCardList.Add(selfCard5)
        selfCardList.Add(selfCard6)
        selfCardList.Add(selfCard7)
        enemyCardList.Add(enemyCard1)
        enemyCardList.Add(enemyCard2)
        enemyCardList.Add(enemyCard3)
        enemyCardList.Add(enemyCard4)
        enemyCardList.Add(enemyCard5)
        enemyCardList.Add(enemyCard6)
        enemyCardList.Add(enemyCard7)
        selfCard1.Image = card1
        selfCard2.Image = card2
        selfCard3.Image = card3
        selfCard4.Image = card4
        selfCard5.Image = cardPair1
        selfCard6.Image = card5
        selfCard7.Image = card6
        enemyCard1.BackgroundImage = card7
        enemyCard2.BackgroundImage = card8
        enemyCard3.BackgroundImage = card9
        enemyCard4.BackgroundImage = card10
        enemyCard5.BackgroundImage = card11
        enemyCard6.BackgroundImage = card12
        enemyCard7.BackgroundImage = cardPair2
    End Sub

    Private Sub selfCardClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles selfCard1.Click, selfCard2.Click, selfCard3.Click, selfCard4.Click, selfCard5.Click, selfCard6.Click, selfCard7.Click
        Dim selected As PictureBox = CType(sender, PictureBox)
        If Not cardSelected Then
            cardSelected = True
            firstSelectedCard = selected.Image
            tmpName = selected.Name
        Else
            secondSelectedCard = selected.Image
            selected.Image = firstSelectedCard
            selected = CType(Me.Controls.Find(tmpName, True)(0), PictureBox)
            selected.Image = secondSelectedCard
            cardSelected = False
        End If
        RaiseEvent checkWin()
    End Sub

    Private Sub enemyCardClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles enemyCard1.Click, enemyCard2.Click, enemyCard3.Click, enemyCard4.Click, enemyCard5.Click, enemyCard6.Click, enemyCard7.Click
        Dim selected As PictureBox = CType(sender, PictureBox)
        If cardSelected Then
            secondSelectedCard = selected.BackgroundImage
            selected.BackgroundImage = firstSelectedCard
            selected = CType(Me.Controls.Find(tmpName, True)(0), PictureBox)
            selected.Image = secondSelectedCard
            cardSelected = False
        End If
        Dim stringToSend As String = _
            selfCard1.Image.Tag & _
            selfCard2.Image.Tag & _
            selfCard3.Image.Tag & _
            selfCard4.Image.Tag & _
            selfCard5.Image.Tag & _
            selfCard6.Image.Tag & _
            selfCard7.Image.Tag & _
            enemyCard1.BackgroundImage.Tag & _
            enemyCard2.BackgroundImage.Tag & _
            enemyCard3.BackgroundImage.Tag & _
            enemyCard4.BackgroundImage.Tag & _
            enemyCard5.BackgroundImage.Tag & _
            enemyCard6.BackgroundImage.Tag & _
            enemyCard7.BackgroundImage.Tag

        Dim udpClient As New Sockets.UdpClient
        udpClient.Connect("127.0.0.1", Val(txtBindPort.Text))
        Dim sendBytes As Byte()
        sendBytes = Encoding.ASCII.GetBytes(stringToSend)
        udpClient.Send(sendBytes, sendBytes.Length)

        'Panel1.Enabled = False
        RaiseEvent checkWin()
    End Sub

    'Private Sub UpdateListBox(ByVal teks As String)
    '    If Me.InvokeRequired Then
    '        Dim args() As String = {teks}
    '        Me.Invoke(New Action(Of String)(AddressOf UpdateListBox), args)
    '    Else
    '        lbHasil.Items.Add(vbCrLf & teks)
    '    End If
    'End Sub

    Public Sub serverThread()
        Dim udpClient As New UdpClient(CInt(lblPort.Text)) 'LISTEN
        While True
            Dim RemoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)
            Dim receiveBytes As Byte()
            receiveBytes = udpClient.Receive(RemoteIpEndPoint)
            Dim returnData As String = Encoding.ASCII.GetString(receiveBytes)
            lblTest.Text = returnData.ToString
            If returnData.ToString = "playing" Then
                MsgBox("Sedang bermain!")
                txtBindPort.ReadOnly = False
                btnKirim.Enabled = True
                Panel1.Enabled = False
            End If
            If isPlaying And returnData.ToString.Length < 14 Then
                Dim udpClientSocket As New Sockets.UdpClient
                udpClientSocket.Connect("127.0.0.1", Val(returnData.ToString)) 'SEND
                Dim sendBytes As Byte()
                sendBytes = Encoding.ASCII.GetBytes("playing")
                udpClientSocket.Send(sendBytes, sendBytes.Length)
                Exit Sub
            End If
            If returnData.ToString = "closed" Then
                lblStatus.Text = "WIN"
                Exit Sub
            End If
            If firstPlay Then
                txtBindPort.Text = returnData.ToString
                txtBindPort.ReadOnly = True
                btnKirim.Enabled = False
                isPlaying = True
                'Dim thdUDPServer = New Thread(New ThreadStart(AddressOf serverThread))
                'thdUDPServer.Start()
                firstPlay = False
            Else
                receivedString = returnData.ToString
                For i As Integer = 0 To 6
                    For Each card As Image In cardList
                        If card.Tag = receivedString(i) Then
                            enemyCardList(i).BackgroundImage = card
                        End If
                    Next
                    For Each card As Image In cardList
                        If card.Tag = receivedString(i + 7) Then
                            selfCardList(i).Image = card
                        End If
                    Next
                Next
                Panel1.Enabled = True
                RaiseEvent checkWin()
            End If
        End While
    End Sub

    Private Sub btnKirim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKirim.Click
        isPlaying = True
        txtBindPort.ReadOnly = True
        btnKirim.Enabled = False
        Panel1.Enabled = True
        lblStatus.Text = "---"
        firstPlay = False
        'Dim thdUDPServer = New Thread(New ThreadStart(AddressOf serverThread))
        'thdUDPServer.Start()

        Dim udpClient As New Sockets.UdpClient
        udpClient.Connect("127.0.0.1", Val(txtBindPort.Text)) 'SEND
        Dim sendBytes As Byte()
        sendBytes = Encoding.ASCII.GetBytes(lblPort.Text)
        udpClient.Send(sendBytes, sendBytes.Length)

    End Sub

    Private Sub checkWinHandler() Handles Me.checkWin
        selfCardList(0) = selfCard1
        selfCardList(1) = selfCard2
        selfCardList(2) = selfCard3
        selfCardList(3) = selfCard4
        selfCardList(4) = selfCard5
        selfCardList(5) = selfCard6
        selfCardList(6) = selfCard7
        For i As Integer = 0 To 6
            For j As Integer = i + 1 To 6
                If selfCardList(i).Image.Tag = selfCardList(j).Image.Tag Then
                    lblStatus.Text = "WIN"
                End If
            Next
        Next
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Dim udpClient As New Sockets.UdpClient
        udpClient.Connect("127.0.0.1", Val(txtBindPort.Text)) 'SEND
        Dim sendBytes As Byte()
        sendBytes = Encoding.ASCII.GetBytes("closed")
        udpClient.Send(sendBytes, sendBytes.Length)
    End Sub
End Class
