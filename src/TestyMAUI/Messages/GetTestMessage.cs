using CommunityToolkit.Mvvm.Messaging.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.Messages;

public class GetTestMessage : ValueChangedMessage<ZestawSearchEntryUI>
{
    public GetTestMessage(ZestawSearchEntryUI value) : base(value)
    {
    }
}
