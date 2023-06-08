using System.IO.Pipelines;

namespace DeadlockHoliday.HtmlScrap.Core.Readers;

public class HtmlPipeReader
{
    // IDEA:
    // - scan entire stream.
    // - build a tree of marks (tag:start:end).
    // - scan entire stream again and process chunks.
    // PROBLEM: double scan is way underperformant.
}