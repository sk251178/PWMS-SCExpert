Imports System.Collections
Imports System.Text
Imports System.IO
Imports System.Web.UI


''' <summary>
''' Parses the view state, constructing a viaully-accessible object graph.
''' </summary>
Public Class ViewStateParser
    ' private member variables
    Private tw As TextWriter
    Private m_indentString As String = "   "

#Region "Constructor"
    ''' <summary>
    ''' Creates a new ViewStateParser instance, specifying the TextWriter to emit the output to.
    ''' </summary>
    Public Sub New(writer As TextWriter)
        tw = writer
    End Sub
#End Region

#Region "Methods"
#Region "ParseViewStateGraph Methods"
    ''' <summary>
    ''' Emits a readable version of the view state to the TextWriter passed into the object's constructor.
    ''' </summary>
    ''' <param name="viewState">The view state object to start parsing at.</param>
    Public Overridable Sub ParseViewStateGraph(viewState As Object)
        ParseViewStateGraph(viewState, 0, String.Empty)
    End Sub

    ''' <summary>
    ''' Emits a readable version of the view state to the TextWriter passed into the object's constructor.
    ''' </summary>
    ''' <param name="viewStateAsString">A base-64 encoded representation of the view state to parse.</param>
    Public Overridable Sub ParseViewStateGraph(viewStateAsString As String)
        ' First, deserialize the string into a Triplet
        Dim los As New LosFormatter()
        Dim viewState As Object = los.Deserialize(viewStateAsString)

        ParseViewStateGraph(viewState, 0, String.Empty)
    End Sub

    ''' <summary>
    ''' Recursively parses the view state.
    ''' </summary>
    ''' <param name="node">The current view state node.</param>
    ''' <param name="depth">The "depth" of the view state tree.</param>
    ''' <param name="label">A label to display in the emitted output next to the current node.</param>
    Protected Overridable Sub ParseViewStateGraph(node As Object, depth As Integer, label As String)
        tw.Write(System.Environment.NewLine)

        If node Is Nothing Then
            tw.Write([String].Concat(Indent(depth), label, "NODE IS NULL"))
        ElseIf TypeOf node Is Triplet Then
            tw.Write([String].Concat(Indent(depth), label, "TRIPLET"))
            ParseViewStateGraph(DirectCast(node, Triplet).First, depth + 1, "First: ")
            ParseViewStateGraph(DirectCast(node, Triplet).Second, depth + 1, "Second: ")
            ParseViewStateGraph(DirectCast(node, Triplet).Third, depth + 1, "Third: ")
        ElseIf TypeOf node Is Pair Then
            tw.Write([String].Concat(Indent(depth), label, "PAIR"))
            ParseViewStateGraph(DirectCast(node, Pair).First, depth + 1, "First: ")
            ParseViewStateGraph(DirectCast(node, Pair).Second, depth + 1, "Second: ")
        ElseIf TypeOf node Is ArrayList Then
            tw.Write([String].Concat(Indent(depth), label, "ARRAYLIST"))

            ' display array values
            For i As Integer = 0 To DirectCast(node, ArrayList).Count - 1
                ParseViewStateGraph(DirectCast(node, ArrayList)(i), depth + 1, [String].Format("({0}) ", i))
            Next
        ElseIf node.[GetType]().IsArray Then
            tw.Write([String].Concat(Indent(depth), label, "ARRAY "))
            tw.Write([String].Concat("(", node.[GetType]().ToString(), ")"))
            Dim e As IEnumerator = DirectCast(node, Array).GetEnumerator()
            Dim count As Integer = 0
            While e.MoveNext()
                ParseViewStateGraph(e.Current, depth + 1, [String].Format("({0}) ", System.Math.Max(System.Threading.Interlocked.Increment(count), count - 1)))
            End While
        ElseIf node.[GetType]().IsPrimitive OrElse TypeOf node Is String Then
            tw.Write([String].Concat(Indent(depth), label))
            tw.Write(node.ToString() + " (" + node.[GetType]().ToString() + ")")
        Else
            tw.Write([String].Concat(Indent(depth), label, "OTHER - "))
            tw.Write(node.[GetType]().ToString())
        End If
    End Sub
#End Region

    ''' <summary>
    ''' Returns a string containing the <see cref="IndentString"/> property value a specified number of times.
    ''' </summary>
    ''' <param name="depth">The number of times to repeat the <see cref="IndentString"/> property.</param>
    ''' <returns>A string containing the <see cref="IndentString"/> property value a specified number of times.</returns>
    Protected Overridable Function Indent(depth As Integer) As String
        Dim sb As New StringBuilder(IndentString.Length * depth)
        For i As Integer = 0 To depth - 1
            sb.Append(IndentString)
        Next

        Return sb.ToString()
    End Function
#End Region

#Region "Properties"
    ''' <summary>
    ''' Specifies the indentation to use for each level when displaying the object graph.
    ''' </summary>
    ''' <value>A string value; the default is three blank spaces.</value>
    Public Property IndentString() As String
        Get
            Return m_indentString
        End Get
        Set(value As String)
            m_indentString = value
        End Set
    End Property
#End Region
End Class

