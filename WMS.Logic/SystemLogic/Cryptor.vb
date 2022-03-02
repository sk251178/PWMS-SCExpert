Imports System.Security.Cryptography

<CLSCompliant(False)> Public Class Cryptor
    Private Const encKey As String = "made4net"
    Private Const encIV As String = "12345678"

    Public Shared Function Encrypt(ByVal pStr As String) As String
        Dim i As Int32
        Dim DesCrypto As New DESCryptoServiceProvider
        Dim Encripted As String = ""
        Dim tmpArr() As Byte = System.Text.Encoding.Unicode.GetBytes(pStr)
        Dim EncArr() As Byte
        DesCrypto.Key = System.Text.Encoding.UTF8.GetBytes("Made4Net")
        DesCrypto.Mode = CipherMode.CBC
        DesCrypto.IV = System.Text.Encoding.UTF8.GetBytes("12345678")
        EncArr = DesCrypto.CreateEncryptor().TransformFinalBlock(tmpArr, 0, tmpArr.Length)
        For i = 0 To EncArr.Length - 1
            Encripted += EncArr(i).ToString("X")
        Next
        'Encripted = System.Text.Encoding.Unicode.GetString(EncArr)
        Return Encripted
    End Function

    Private Shared Function Decrypt(ByVal pStr As String) As String
        Dim DesCrypto As New DESCryptoServiceProvider
        Dim Decripted As String
        Dim tmpArr() As Byte = System.Text.Encoding.Unicode.GetBytes(pStr)
        Dim DecArr() As Byte
        DesCrypto.Key = System.Text.Encoding.Default.GetBytes("Made4Net")
        DesCrypto.Mode = CipherMode.CBC
        DesCrypto.IV = System.Text.Encoding.Default.GetBytes("12345678")
        DecArr = DesCrypto.CreateDecryptor().TransformFinalBlock(tmpArr, 0, tmpArr.Length)
        Decripted = System.Text.Encoding.Unicode.GetString(DecArr)
        Return Decripted
    End Function

End Class
