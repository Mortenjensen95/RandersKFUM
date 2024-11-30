using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RandersKFUM.Utilities
{
    public class NavigationService
    {
        private readonly Dictionary<Type, Type> _viewModelViewMap = new();

        private Frame _mainFrame;

        public void Configure(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void Register<TViewModel, TView>()
            where TView : Page, new()
        {
            _viewModelViewMap[typeof(TViewModel)] = typeof(TView);
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            if (_viewModelViewMap.TryGetValue(typeof(TViewModel), out var viewType))
            {
                var view = (Page)Activator.CreateInstance(viewType);
                _mainFrame.Navigate(view);
            }
            else
            {
                throw new InvalidOperationException($"No view registered for {typeof(TViewModel)}.");
            }
        }
    }
}
