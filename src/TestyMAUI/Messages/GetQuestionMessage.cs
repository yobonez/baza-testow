using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestyMAUI.UIModels;

namespace TestyMAUI.Messages
{
    public class GetQuestionMessage : ValueChangedMessage<PytanieSearchEntryUI>
    {
        public GetQuestionMessage(PytanieSearchEntryUI value) : base(value)
        {
        }
    }
}
