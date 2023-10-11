using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags.Data;

public record class TagsHistoryRecord(
    int TagOffset, 
    int? InnerOffset, 
    OpeningTag Metadata);