using WebScrap.Common.Tags;

namespace WebScrap.Core.Tags;

public record class OpenedTag(
    int TagOffset, 
    int? InnerOffset, 
    OpeningTag Metadata);