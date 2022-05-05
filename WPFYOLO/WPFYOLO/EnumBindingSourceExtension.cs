using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace WPFYOLO
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; private set; }

        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
                throw new Exception("incorrect EnumType or EnumType is null");
            EnumType = enumType;
        }
        public override object ProvideValue(IServiceProvider _)
        {
            return Enum.GetValues(EnumType);
        }


    }
}
