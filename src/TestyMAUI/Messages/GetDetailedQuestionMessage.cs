using CommunityToolkit.Mvvm.Messaging.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.Messages;

public class GetDetailedQuestionMessage : ValueChangedMessage<PytanieSearchEntryUI>
{
    public GetDetailedQuestionMessage(PytanieSearchEntryUI value) : base(value)
    {
    }
}
