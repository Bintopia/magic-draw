using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Johnothing.MagicDraw.Extensions
{
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = (propertyExpression.Body as MemberExpression).Member.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
