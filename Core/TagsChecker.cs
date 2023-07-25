using System.Collections.ObjectModel;

namespace Core;

internal class TagsChecker
{
    // TODO: record-dictionary?
    private readonly ReadOnlyDictionary<string, CheckerRecord> cssSelectors;

    public TagsChecker(string cssSelectors)
    {
        const string delimeter = ";";
        this.cssSelectors = cssSelectors.Split(delimeter)
            .ToDictionary(x => x, x => new CheckerRecord(x))
            .AsReadOnly();
    }
    
    public string Check(string htmlCssTag)
    {
        // just a draft
        const string retYes = "yes";
        const string retFinally = "finally";

        throw new NotImplementedException();
    }
}