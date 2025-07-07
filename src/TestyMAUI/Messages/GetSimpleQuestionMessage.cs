using CommunityToolkit.Mvvm.Messaging.Messages;
using TestyMAUI.UIModels;

namespace TestyMAUI.Messages;

public class GetSimpleQuestionMessage : ValueChangedMessage<PytanieUI>
{
    public GetSimpleQuestionMessage(PytanieUI value) : base(value)
    {
    }
}
