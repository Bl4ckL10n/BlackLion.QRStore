using System.Windows.Input;
using Xamarin.Forms;

namespace BlackLion.QRStore.Controls
{
    internal class SearchBar : Xamarin.Forms.SearchBar
    {
        public static BindableProperty TextChangedCommandProperty = BindableProperty.Create(
                                                 propertyName: "Command",
                                                 returnType: typeof(ICommand),
                                                 declaringType: typeof(SearchBar),
                                                 defaultValue: null,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: TextChangedCommandPropertyChanged);

        public static readonly BindableProperty TextChangedCommandParameterProperty = BindableProperty.Create(
                                                nameof(TextChangedCommand),
                                                typeof(object),
                                                typeof(object),
                                                propertyChanged: CommandParameterUpdated);

        public ICommand TextChangedCommand
        {
            get => (ICommand)GetValue(TextChangedCommandProperty);
            set => SetValue(TextChangedCommandProperty, value);
        }

        public object TextChangedCommandParameter
        {
            get => GetValue(TextChangedCommandParameterProperty);
            set => SetValue(TextChangedCommandParameterProperty, value);
        }

        public static void TextChangedCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as SearchBar).TextChangedCommand = (ICommand)newValue;
        }

        private static void CommandParameterUpdated(object sender, object oldValue, object newValue)
        {
            if (sender is SearchBar searchBar && newValue != null && oldValue != null)
            {
                if ((newValue as string).Trim() != (oldValue as string).Trim())
                {
                    searchBar.TextChangedCommand.Execute(newValue as string);
                }
            }
        }
    }
}
