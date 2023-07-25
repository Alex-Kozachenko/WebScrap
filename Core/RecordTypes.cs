namespace Core.Records;

// Initial
// candidate tag met? -> Candidate

// Candidate
// candidate tag met? -> Candidate
// candidate tag broken? -> Initial
// all candidates met? -> Closing
// closing tag found -> Completed

internal record class InitialRecord 
{
    internal string CssSelector { get; }

    public InitialRecord(string cssSelector)
    {
        this.CssSelector = cssSelector;
    }
}

internal record class CandidateRecord
    : InitialRecord
{
    private readonly Queue<string> cssSelectorTags;
    private readonly int arrayPosition;

    public CandidateRecord(string cssSelector, int arrayPosition)
        : base(cssSelector)
    {
        cssSelectorTags = new Queue<string>(cssSelector.Split(" "));
        this.arrayPosition = arrayPosition;
    }
}

internal record class FinishedRecord
{
    private int Begin {get; set;}
    private int End { get; set; }
}