using TestyMAUI.UIModels;

namespace TestyMAUI.DataTemplateSelectors;

public class QuestionContentDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate QuestionTemplate { get; set; }
    public DataTemplate EmptyQuestionTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return ((PytanieUI)item).Tresc == null || ((PytanieUI)item).Tresc.Trim() == string.Empty
            ? EmptyQuestionTemplate
            : QuestionTemplate;
    }
}
