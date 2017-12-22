using BasicUI.Controls;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BasicUI
{
    public class Binder<TViewModel> where TViewModel : INotifyPropertyChanged
    {
        private TViewModel _viewModel;

        public Binder(TViewModel viewModel) 
        {
            _viewModel = viewModel;
        }

        public void BindPreRender<T>(T targetControl, Action<TViewModel, T> setter) where T : ControlBase
        {
            targetControl.PreRender += c => setter.Invoke(_viewModel, targetControl);
        }

        public void BindPropChanged<T>(T targetControl, Action<TViewModel, T> setter) where T : ControlBase
        {
            _viewModel.PropertyChanged += (sender, args) => setter.Invoke(_viewModel, targetControl);
        }

        public void BindSpecificPropChanged<T>(Expression<Func<TViewModel, object>> prop, T targetControl, Action<TViewModel, T> setter) where T : ControlBase
        {
            if (prop.Body is MemberExpression mExp)
            {
                BindSpecificPropChanged(mExp.Member.Name, targetControl, setter);
            }

            if (prop.Body is UnaryExpression uExp)
            {
                BindSpecificPropChanged((uExp.Operand.Reduce() as MemberExpression).Member.Name, targetControl, setter);
            }
        }

        public void BindSpecificPropChanged<T>(string propName, T targetControl, Action<TViewModel, T> setter) where T : ControlBase
        {
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == propName)
                {
                    setter.Invoke(_viewModel, targetControl);
                }
            };

            setter.Invoke(_viewModel, targetControl);
        }

        public void BindPropChangedTwoWay<T>(T targetControl, Action<T, TViewModel> controlSetter, Action<TViewModel, T> vmSetter) where T : INotifyPropertyChanged
        {
            _viewModel.PropertyChanged += (sender, args) => controlSetter.Invoke(targetControl, _viewModel);
            targetControl.PropertyChanged += (sender, args) => vmSetter.Invoke(_viewModel, targetControl);
        }
    }
}
