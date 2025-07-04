using CommunityToolkit.Mvvm.Messaging.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.Messages;

public class GetSimpleQuestionMessage : ValueChangedMessage<PytanieSearchEntryUI>
{
    public GetSimpleQuestionMessage(PytanieSearchEntryUI value) : base(value)
    {
    }
}
